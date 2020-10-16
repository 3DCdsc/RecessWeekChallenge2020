using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using _3DC.RecessWeekChallenge.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace _3DC.RecessWeekChallenge.Areas.Identity.Pages.Account.Manage
{
    public class MakeAdminModel : PageModel
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly SignInManager<LoginUser> _signInManager;
        private readonly ILogger<MakeAdminModel> _logger;

        public MakeAdminModel(
            UserManager<LoginUser> userManager,
            SignInManager<LoginUser> signInManager,
            ILogger<MakeAdminModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email to make admin")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var tryUser = await _userManager.FindByEmailAsync(Input.Email);

            if (tryUser == null)
            {
                return NotFound($"Unable to load user with email '{Input.Email}'.");
            }

            if (await _userManager.IsInRoleAsync(tryUser, "Administrator"))
            {
                return BadRequest($"'{Input.Email}'Is already admin");
            }
            var roleResult = await _userManager.AddToRoleAsync(tryUser, "Administrator");


            if (!roleResult.Succeeded)
            {
                return BadRequest($"Unable to make '{Input.Email}' admin");
            }

            _logger.LogInformation($"'{Input.Email}' successfully made admin");
            StatusMessage = $"'{Input.Email}' successfully made admin";

            return RedirectToPage();
        }
    }
}
