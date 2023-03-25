using AppOpener.DependencyResolver;
using AppOpener.Services.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppOpener.Authorization
{
	public class WebJobAuthorizeAttribute : TypeFilterAttribute
	{
		public WebJobAuthorizeAttribute() : base(typeof(WebJobAuthorizeFilter))
		{

		}

	}

	public class WebJobAuthorizeFilter : IAuthorizationFilter
	{
		private readonly IClientService clientervice;

		//As you can see I'm using a constructor injection here
		public WebJobAuthorizeFilter(IClientService clientervice)
		{
			this.clientervice = clientervice;
		}


		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var token = context.HttpContext.Request.Headers.Where(x => x.Key.ToLower().Equals("authorization")).FirstOrDefault().Value;
			if ((string.IsNullOrEmpty(token)))
			{
				context.Result = new UnauthorizedResult();
			}
			else
			{
				var userToken = Regex.Replace(token, "Bearer", "", RegexOptions.IgnoreCase).Trim();
				if ((string.IsNullOrEmpty(userToken) || (!clientervice.ValidateWebJobToken(userToken))))
				{
					context.Result = new UnauthorizedResult();
				}
			}
		}
	}
}
