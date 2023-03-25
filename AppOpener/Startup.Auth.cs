using AppOpener.Data.Models;
using AppOpener.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AppOpener
{
	public partial class Startup
	{
		private SymmetricSecurityKey _signingKey;
		private TokenValidationParameters _tokenValidationParameters;

		private TokenProviderOptions _tokenProviderOptions;

        private MongoDbDatabaseSettings _mongoDbDatabaseSettings;

        private void ConfigureAuth(IServiceCollection services)
		{
            
            services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.Events = new JwtBearerEvents
				{
					OnTokenValidated = context =>
					{
						var authType = context.Principal.Claims.Where(x => x.Type.Equals("typ")).FirstOrDefault();
						if (authType != null)
						{
							if (authType.Value == "client")
							{
								var clientService = context.HttpContext.RequestServices.GetRequiredService<dynamic>();

								var clientId = context.Principal.Identity.Name;
								var client = clientService.GetClientById(clientId);

								if (client == null)
								{
									// return unauthorized if user no longer exists
									context.Fail("Unauthorized");
								}
							}
							if (authType.Value == "user")
							{
								var userService = context.HttpContext.RequestServices.GetRequiredService<dynamic>();
								var email = context.Principal.Identity.Name;
								var user = userService.GetUserByEmail(email);
								if (user == null)
								{
									// return unauthorized if user no longer exists
									context.Fail("Unauthorized");
								}
							}
						}

						return Task.CompletedTask;
					}
				};
				options.TokenValidationParameters = _tokenValidationParameters;
			})
			.AddCookie(options =>
			{
				options.Cookie.Name = Configuration.GetSection("TokenAuthentication:CookieName").Value;
				options.TicketDataFormat = new CustomJwtDataFormat(SecurityAlgorithms.HmacSha256, _tokenValidationParameters);
			});

		}
	}
}
