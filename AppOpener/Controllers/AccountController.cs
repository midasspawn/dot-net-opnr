using AppOpener.Core.BusinessEntities.Client;
using AppOpener.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AppOpener.Authorization;
using AppOpener.Core;
using AppOpener.Core.BusinessEntities.Client;
using AppOpener.Core.BusinessEntities.Configuration;

using AppOpener.Providers;
using AppOpener.Services.Client;
using AppOpener.Services.Configuration;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
namespace AppOpener.Controllers
{
	[ApiController]

	public class AccountController : ControllerBase
	{
		private readonly TokenProviderOptions tokenProviderOptions;
		private readonly IClientService clientService;
		/// <summary>
		/// constructor
		/// </summary>
		public AccountController(TokenProviderOptions tokenProviderOptions,
            IClientService clientService)
		{
			this.tokenProviderOptions = tokenProviderOptions;
			this.clientService = clientService;
		}

		public static long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
		private string GetToken(string user, int tokenType = 1)
		{
			var handler = new JwtSecurityTokenHandler();

			// Here, you should create or look up an identity for the user which is being authenticated.
			// For now, just creating a simple generic identity.

			var now = DateTime.UtcNow;

			// Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
			// You can add other claims here, if you want:
			var claims = new Claim[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user),
				new Claim(JwtRegisteredClaimNames.Typ, tokenType==1 ? "client":"user"),
				new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64)
			};


			ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user, "TokenAuth"), claims);
			var securityToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
			{
				Issuer = tokenProviderOptions.Issuer,
				Audience = tokenProviderOptions.Audience,
				SigningCredentials = tokenProviderOptions.SigningCredentials,
				Subject = identity,
				Expires = now.Add(tokenProviderOptions.Expiration)
			});
			return handler.WriteToken(securityToken);
		}

		[HttpPost]
		[Route("token")]
		[AllowAnonymous]
		public IActionResult ClientToken([FromBody]ClientModel clientModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			else
			{
                var result= clientService.ValidateClient(clientModel.ClientKey, clientModel.ClientSecret);
				if (result.Success)
				{
					var client = result.Data;

					string encodedJwt = string.Empty;
					var clientToken = clientService.GetClientTokenById(client.ClientKey);
					if (clientToken == null || clientToken.ExpiresOn < System.DateTime.UtcNow)
					{
						encodedJwt = GetToken(client.ClientKey);
						System.DateTime today = System.DateTime.UtcNow;

						var userToken = new ClientTokenViewModel()
						{
							ClientId =Convert.ToString(client.ClientId),
							AuthToken = encodedJwt,
							IssuedOn = DateTime.UtcNow,
							ExpiresOn = DateTime.UtcNow.Add(tokenProviderOptions.Expiration)
						};
						clientService.InsertToken(userToken);
					}
					else
					{
						encodedJwt = clientToken.AuthToken;
					}
					var response = new
					{
						access_token = encodedJwt,
						expires_in = (int)tokenProviderOptions.Expiration.TotalSeconds
					};
					var data = new
					{
						success = true,
						message = JsonConvert.SerializeObject(response)
					};
					return new JsonResult(data);
				}
				else
				{
					var data = new
					{
						success = false,
						message = result.ErrorMessages
					};
					return new JsonResult(data);

				}
			}
		}

		

		
	}
}
