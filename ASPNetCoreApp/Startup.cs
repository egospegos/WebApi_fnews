using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ASPNetCoreApp.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using ASPNetCoreApp.DAL;

namespace ASPNetCoreApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }


		public void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<ILikeRepos, LikeRepos>();

			services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<fnewsContext>();
			var connection = Configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<fnewsContext>(options => options.UseSqlServer(connection));
			services.AddMvc().AddJsonOptions(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			});

			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.Name = "SimpleWebApp";
				options.LoginPath = "/";
				options.AccessDeniedPath = "/";
				options.LogoutPath = "/";
				options.Events.OnRedirectToLogin = context =>
				{
					context.Response.StatusCode = 401;
					return Task.CompletedTask;
				};
				options.Events.OnRedirectToAccessDenied = context =>
				{
					context.Response.StatusCode = 401;
				    return Task.CompletedTask;
				};
			});
		}



		private async Task CreateUserRoles(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
			// Создание ролей администратора и пользователя
			if (await roleManager.FindByNameAsync("admin") == null)
			{
				await roleManager.CreateAsync(new IdentityRole("admin"));
			}
			if (await roleManager.FindByNameAsync("user") == null)
			{
				await roleManager.CreateAsync(new IdentityRole("user"));
			}
			// Создание Администратора
			string adminEmail = "admin@mail.com";
			string adminPassword = "Aa123456!";
			if (await userManager.FindByNameAsync(adminEmail) == null)
			{
				User admin = new User
				{
					Email = adminEmail,
					UserName = adminEmail
				};
				IdentityResult result = await
				userManager.CreateAsync(admin, adminPassword);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(admin, "admin");
				}
			}
			// Создание Пользователя
			string userEmail = "user@mail.com";
			string userPassword = "Aa123456!";
			if (await userManager.FindByNameAsync(userEmail) == null)
			{
				User user = new User
				{
					Email = userEmail,
					UserName = userEmail
				};
				IdentityResult result = await
				userManager.CreateAsync(user, userPassword);
				if (result.Succeeded)
				{
				await userManager.AddToRoleAsync(user, "user");
				}
			}
		}




		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider services)
		{
			CreateUserRoles(services).Wait();

			app.UseAuthentication();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseMvc();
		}
	}
}
