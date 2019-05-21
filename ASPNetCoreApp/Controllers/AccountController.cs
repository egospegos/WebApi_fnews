using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNetCoreApp.Models;
using Microsoft.AspNetCore.Identity;

using System.Diagnostics;
using System.Reflection;

namespace ASPNetCoreApp.Controllers
{
	[Produces("application/json")]
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		public AccountController(UserManager<User> userManager,
		SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[HttpPost]
		[Route("api/Account/Register")]
		public async Task<IActionResult> Register([ FromBody ] RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				User user = new User
				{
					Email = model.Email,
					UserName = model.Email
				};
				// Добавление нового пользователя
				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(user, "user");
					// установка куки
					await _signInManager.SignInAsync(user, false);
					var msg = new
					{
						message = "Добавлен новый пользователь: " + user.UserName
					};
					return Ok(msg);
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty,
						error.Description);
					}
					var errorMsg = new
					{
						message = "Пользователь не добавлен.",
						error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
					};
					return Ok(errorMsg);
				}
			}
			else
			{
				var errorMsg = new
				{
					message = "Неверные входные данные.",
					error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
				};
				return Ok(errorMsg);
			}
		}


		[HttpPost]
		[Route("api/Account/Login")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login([ FromBody ] LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result =
				await
				_signInManager.PasswordSignInAsync(model.Email, model.Password,
				model.RememberMe, false);
				if (result.Succeeded)
				{
					var msg = new
					{
						message = "Выполнен вход пользователем: " +
					model.Email
					};
					return Ok(msg);
				}
				else
				{
					ModelState.AddModelError("", "Неправильный логин и (или) пароль" );
					var errorMsg = new
					{
						message = "Вход не выполнен.",
						error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
					};
					return Ok(errorMsg);
				}
			}
			else
			{
				var errorMsg = new
				{
					message = "Вход не выполнен.",
					error = ModelState.Values.SelectMany(e =>
					e.Errors.Select(er => er.ErrorMessage))
				};
				return Ok(errorMsg);
			}
		}
		[HttpPost]
		[Route("api/Account/LogOff")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogOff()
		{
			// Удаление куки
			await _signInManager.SignOutAsync();
			var msg = new
			{
				message = "Выполнен выход."
			};
			return Ok(msg);
		}

		[HttpPost]
		[Route("api/Account/isAuthenticated")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogisAuthenticatedOff()
		{
			User usr = await GetCurrentUserAsync();
			var message = usr == null ? "Вы Гость. Пожалуйста, выполните вход." : "Вы вошли как: " + usr.UserName;
             var msg = new
				{
					message
				};
			return Ok(msg);
		}
		private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

		[HttpPost]
		[Route("api/Account/isAdmin")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> isAdmin()
		{
			User usr = await GetCurrentUserAsync();
			IList<string> roles;
			try
			{
				roles = await _userManager.GetRolesAsync(usr);
			}
			catch
			{
				roles = null;
			}
			var message = usr == null ? "Вы гость" : roles.FirstOrDefault();
			var msg = new
			{
				message
			};
			return Ok(msg);
		}


		IList<string> x;
		string role;
		[Route("api/Account/GetRole")]
		public async Task<string> GetUserRole()
		{
			try
			{
				User usr = await GetCurrentUserAsync();
				if(usr !=null)
				{
					x = await _userManager.GetRolesAsync(usr);
					role = x.FirstOrDefault();
					
				}
			}
			catch(Exception ex)
			{
				Log.Write(ex);
			}
			return role;
		}

		IList<string> x2;
		string roleID;
		[Route("api/Account/GetRoleID")]
		public async Task<string> GetUserRoleID()
		{
			try
			{
				User usr = await GetCurrentUserAsync();
				if (usr != null)
				{
					x2 = await _userManager.GetRolesAsync(usr);
					roleID = usr.Id;

				}
			}
			catch (Exception ex)
			{
				Log.Write(ex);
			}
			return roleID;
		}
	}
}
