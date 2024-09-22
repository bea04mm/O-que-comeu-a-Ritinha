using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using NuGet.Common;
using O_que_comeu_a_Ritinha.Data;
using O_que_comeu_a_Ritinha.Models;

namespace O_que_comeu_a_Ritinha.Controllers.API
{
	[Route("api/[controller]")]
	[ApiController]
	public class AutenticationController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public UserManager<IdentityUser> _userManager;
		public SignInManager<IdentityUser> _signInManager;
		private readonly IEmailSender _emailSender;

		public AutenticationController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IEmailSender emailSender)
		{
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
			_emailSender = emailSender;
		}

		[HttpPost]
		[Route("Register")]
		public async Task<ActionResult> Register([FromForm] Utilizadores utilizador, [FromForm] string Email, [FromForm] string Password)
		{
			if (utilizador == null)
			{
				return BadRequest("Please insert a user");
			}

			try
			{
				IdentityUser existingUser = await _userManager.FindByEmailAsync(Email);
				if (existingUser != null)
				{
					return BadRequest("Email already has an account");
				}

				IdentityUser newUser = new IdentityUser
				{
					UserName = Email,
					Email = Email,
					Id = Guid.NewGuid().ToString(),
				};

				var result = await _userManager.CreateAsync(newUser, Password);
				if (!result.Succeeded)
				{
					return BadRequest(result.Errors);
				}

				Utilizadores userApp = new Utilizadores
				{
					Name = utilizador.Name,
					Birthday = utilizador.Birthday,
					Phone = utilizador.Phone,
					UserId = newUser.Id,
				};

				_context.Utilizadores.Add(userApp);
				await _context.SaveChangesAsync();

				var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
				await _emailSender.SendEmailAsync(Email, "Confirma a tua conta (token)",
					$"Confirma a tua conta usando este token: {token}");

				// Retorne o UserId após o registro
				return Ok(new { Message = "User registered successfully", UserId = newUser.Id });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Route("ConfirmEmail")]
		public async Task<ActionResult> ConfirmEmail([FromForm] string UserId, [FromForm] string Token)
		{
			if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Token))
			{
				return BadRequest("UserId and Token are required.");
			}

			try
			{
				IdentityUser user = await _userManager.FindByIdAsync(UserId);

				if (user == null)
				{
					return NotFound("User not found.");
				}

				var result = await _userManager.ConfirmEmailAsync(user, Token);

				if (result.Succeeded)
				{
					var email = await _userManager.GetEmailAsync(user); // Obtém o e-mail do utilizador
					await _userManager.AddToRoleAsync(user, "Utilizador");
					var callbackUrl = Url.Page(
							"/Account/ResetPasswordConfirmation",
							pageHandler: null,
							values: new { userId = UserId },
							protocol: Request.Scheme);
						await _emailSender.SendEmailAsync(
							email, // O e-mail do destinatário
							"Bem-vindo! 💝",
							$"Obrigado por te juntares à família. ❤️");

					return Ok(new { message = "Email confirmed successfully." });
				}

				return BadRequest(result.Errors);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("Login")]
		public async Task<ActionResult> Login([FromForm] string Email, [FromForm] string Password, [FromForm] bool RememberMe)
		{
			if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
			{
				return BadRequest("Email and password are required.");
			}

			var result = await _signInManager.PasswordSignInAsync(Email, Password, RememberMe, lockoutOnFailure: false);

			if (result.Succeeded)
			{
				var user = await _userManager.FindByEmailAsync(Email);
				var roles = await _userManager.GetRolesAsync(user);
				Console.WriteLine($"User roles for {Email}: {string.Join(", ", roles)}");

				return Ok(new
				{
						userId = user.Id,  // Aqui estamos garantindo que o userId seja incluído
						email = user.Email,
						roles
			});
			}

			return Unauthorized(new { message = "Invalid login attempt." });
		}

		//[HttpGet]
		//[Route("Login")]
		//public async Task<ActionResult> Login([FromQuery] string email, [FromQuery] string password, [FromQuery] bool remainder)
		//{
		//	try
		//	{

		//		IdentityUser resultUser = await _userManager.FindByEmailAsync(email);

		//		if (resultUser != null)
		//		{

		//			if (resultUser.EmailConfirmed == false)
		//			{
		//				return BadRequest("Please Confirm Email First");
		//			}

		//			PasswordVerificationResult passWorks = new PasswordHasher<IdentityUser>().VerifyHashedPassword(resultUser, resultUser.PasswordHash, password);

		//			if (passWorks == PasswordVerificationResult.Success)
		//			{

		//				await _signInManager.SignInAsync(resultUser, remainder); // 'remainder' determines if the user should be remembered for 14 days.


		//				Utilizadores user = _context.Utilizadores.FirstOrDefault(u => u.UserId == resultUser.Id);

		//				if (user != null)
		//				{
		//					// user to DTO
		//					Utilizadores userdto = new Utilizadores
		//					{
		//						Id = user.Id,
		//						Name = user.Name,
		//						Birthday = user.Birthday,
		//						Phone = user.Phone
		//					};

		//					return Ok(userdto);
		//				}
		//				else
		//				{
		//					return BadRequest("User details not found");
		//				}
		//			}
		//			else
		//			{
		//				return BadRequest("Invalid password");
		//			}
		//		}
		//		else
		//		{
		//			return BadRequest("User not found");
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		return BadRequest(ex.Message); // Return exception message
		//	}
		//}
	}
}