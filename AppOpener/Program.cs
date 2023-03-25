using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AppOpener
{
	public class Program
	{
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
            

        }

		public static IWebHost BuildWebHost(string[] args) =>
		 WebHost.CreateDefaultBuilder(args)
				.UseKestrel(options =>
				{
					options.Limits.MaxRequestBodySize = null;
					options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30); // request timeout 
				})
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseSetting("detailedErrors", "true")
				.UseIISIntegration()
				.UseStartup<Startup>()
				.CaptureStartupErrors(true)
				.Build();
	}
}
