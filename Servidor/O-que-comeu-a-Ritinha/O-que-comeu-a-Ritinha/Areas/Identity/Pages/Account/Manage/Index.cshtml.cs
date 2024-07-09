// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using O_que_comeu_a_Ritinha.Data;
using O_que_comeu_a_Ritinha.Models;

namespace O_que_comeu_a_Ritinha.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Display(Name = "Nome")]
			[Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
			[StringLength(60)]
			public string Name { get; set; }

			[Display(Name = "Data de Nascimento")]
			[DataType(DataType.Date)] // informa a View de como deve tratar este atributo
			[Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
			public DateOnly Birthday { get; set; }

			[Display(Name = "Telemóvel")]
			[StringLength(9)]
			// 913456789
			// +351913456789
			// 00351913456789
			[RegularExpression("9[1236][0-9]{7}",
			 ErrorMessage = "O {0} só aceita 9 digitos")]
			[Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
			public string Phone { get; set; }
        }

            private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.UserId == user.Id);
            if (utilizador != null)
            {
                Input = new InputModel
                {
                    Name = utilizador.Name,
                    Birthday = utilizador.Birthday,
                    Phone = utilizador.Phone
                };
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Atualizar dados do utilizador na base de dados
            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.UserId == user.Id);
            if (utilizador != null)
            {
                utilizador.Name = Input.Name;
                utilizador.Birthday = Input.Birthday;
                utilizador.Phone = Input.Phone;

                _context.Update(utilizador);
                await _context.SaveChangesAsync();
            }


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "O teu perfil foi alterado.";
            return RedirectToPage();
        }
    }
}
