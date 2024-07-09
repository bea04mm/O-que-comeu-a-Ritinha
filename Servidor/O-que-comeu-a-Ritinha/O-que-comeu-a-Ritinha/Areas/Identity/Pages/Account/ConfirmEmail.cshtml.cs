using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace O_que_comeu_a_Ritinha.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

		private readonly IEmailSender _emailSender;

		private readonly ILogger<ConfirmEmailModel> _logger;

		public ConfirmEmailModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, ILogger<ConfirmEmailModel> logger)
        {
            _userManager = userManager;
			_emailSender = emailSender;
			_logger = logger;
		}

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;

		public class InputModel
        {
            [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
			public string Token { get; set; }

            [Required]
            public string UserId { get; set; }
        }

        public Task<IActionResult> OnGetAsync(string userId)
        {
            if (userId == null)
            {
				return Task.FromResult<IActionResult>(RedirectToPage("/Index"));
			}

			Input = new InputModel
            {
                UserId = userId
            };

			return Task.FromResult<IActionResult>(Page());
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var user = await _userManager.FindByIdAsync(Input.UserId);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{Input.UserId}'.");
			}

			var result = await _userManager.ConfirmEmailAsync(user, Input.Token);
			if (result.Succeeded)
			{
				StatusMessage = "Obrigada por confirmares o teu email. Podes realizar o seu";

				try
				{
					var email = await _userManager.GetEmailAsync(user); // Obtém o e-mail do utilizador
					var callbackUrl = Url.Page(
						"/Account/ResetPasswordConfirmation",
						pageHandler: null,
						values: new { userId = Input.UserId },
						protocol: Request.Scheme);
					await _emailSender.SendEmailAsync(
						email, // O e-mail do destinatário
						"Bem-vindo! 💝",
						$"Obrigado por te juntares à família. ❤️");
				}
				catch (Exception ex)
				{
					_logger.LogError($"Erro ao enviar e-mail: {ex.Message}");
					StatusMessage = "O email foi confirmado, mas houve um erro ao enviar o e-mail de boas-vindas.";
				}
			}
			else
			{
				StatusMessage = "Erro ao confirmar o teu email.";
				return Page();
			}

			return Page();
		}
	}
}