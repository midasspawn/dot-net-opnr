using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using AppOpener.DependencyResolver;
using AppOpener.Providers;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Text;
using AppOpener.Data.Models;
using AppOpener.Services;
using Autofac.Core;
using System.Runtime;
using AppOpener.Services.Client;
using AppOpener.Services.Configuration;

namespace AppOpener
{
	public partial class Startup
	{
		/// <summary>
		/// application startup
		/// </summary>
		/// <param name="configuration"></param>
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();



			Configuration = builder.Build();

           

            _signingKey =
				new SymmetricSecurityKey(
					Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));

			_tokenValidationParameters = new TokenValidationParameters
			{
				// The signing key must match!
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = _signingKey,
				// Validate the JWT Issuer (iss) claim
				ValidateIssuer = true,
				ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
				// Validate the JWT Audience (aud) claim
				ValidateAudience = true,
				ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
				// Validate the token expiry
				ValidateLifetime = true,
				//// If you want to allow a certain amount of clock drift, set that here:
				// ClockSkew = TimeSpan.Zero
			};


			_tokenProviderOptions = new TokenProviderOptions
			{
				Path = Configuration.GetSection("TokenAuthentication:TokenPath").Value,
				Audience = Configuration.GetSection("TokenAuthentication:Audience").Value,
				Issuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
				Expiration = !string.IsNullOrEmpty(Configuration.GetSection("TokenAuthentication:Expiration").Value) ? TimeSpan.FromSeconds(Convert.ToDouble(Configuration.GetSection("TokenAuthentication:Expiration").Value)) : TimeSpan.FromSeconds(300),
				SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
				// IdentityResolver = GetIdentity
			};



            _mongoDbDatabaseSettings = new  MongoDbDatabaseSettings
            {
                ConnectionString = Configuration.GetSection("NoSqlDatabase:ConnectionString").Value,
                DatabaseName = Configuration.GetSection("NoSqlDatabase:DatabaseName").Value,
                BasePlanCollectionName = Configuration.GetSection("NoSqlDatabase:BasePlanCollectionName").Value,
                LinksCollectionName = Configuration.GetSection("NoSqlDatabase:LinksCollectionName").Value,
                GoogleUserCollectionName = Configuration.GetSection("NoSqlDatabase:GoogleUserCollectionName").Value,
            };



           
        }

		/// get application configuration
		/// </summary>
		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		/// <summary>
		/// configure services
		/// </summary>
		/// <param name="services"></param>
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.Configure<FormOptions>(options =>
			{  
				options.MultipartBodyLengthLimit = 60000000;
                
			});
           
            services.AddSingleton<TokenProviderOptions>(_tokenProviderOptions);
            // Enable the use of an [Authorize("Bearer")] attribute on methods and
            // classes to protect.
            services.AddAuthorization(auth =>
			{
				auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
					.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
					.RequireAuthenticatedUser().Build());
			});
            services.Configure<MongoDbDatabaseSettings>(options => Configuration.GetSection("NoSqlDatabase").Bind(options));
            services.Configure<GoogleCredentials>(options => Configuration.GetSection("NoSqlDatabase").Bind(options));

            ConfigureAuth(services);

			services.AddMvc(options =>
			{
				options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
			});

			services.AddCors(c =>
			{
				c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Title = "AppOpener APIs",
				});

            });

			services.AddTransient<IGoogleOAuthService, GoogleOAuthService>();
            services.AddTransient<IPlatformService, PlatformService>();
            services.AddTransient<IURLService, URLService>();
            services.AddSingleton<IClientService, ClientService>();
            services.AddSingleton<ISettingService, SettingService>();
            services.AddSingleton<TokenProviderOptions>();
            
            //Now register our services with Autofac container
            var builder = new ContainerBuilder();
			builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().AsSelf().SingleInstance();

            builder.Populate(services);
			var container = builder.Build();
			//Create the IServiceProvider based on the container.
			return new AutofacServiceProvider(container);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// <summary>
		/// configure application environment
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="loggerFactory"></param>
		/// <param name="context"></param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			AppDependencyResolver.Init(app.ApplicationServices);

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseFileServer(enableDirectoryBrowsing: false);
			app.UseAuthentication();

			var fileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "upload"));
			var requestPath = "/upload";

			// Enable displaying browser links.
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = fileProvider,
				RequestPath = requestPath
			});

			app.UseDirectoryBrowser(new DirectoryBrowserOptions
			{
				FileProvider = fileProvider,
				RequestPath = requestPath
			});

			app.UseCors(builder =>
			{
				builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
			});

           

            app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("../swagger/v1/swagger.json", "AppOpener APIs");
            });
            

        }
	}
}
