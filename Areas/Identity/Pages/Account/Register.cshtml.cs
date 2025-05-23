// // Licensed to the .NET Foundation under one or more agreements.
// // The .NET Foundation licenses this file to you under the MIT license.
// #nullable disable

// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.Linq;
// using System.Text;
// using System.Text.Encodings.Web;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.UI.Services;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.AspNetCore.WebUtilities;
// using Microsoft.Extensions.Logging;

// namespace AdilBooks.Areas.Identity.Pages.Account
// {
//     public class RegisterModel : PageModel
//     {
//         private readonly SignInManager<IdentityUser> _signInManager;
//         private readonly UserManager<IdentityUser> _userManager;
//         private readonly IUserStore<IdentityUser> _userStore;
//         private readonly IUserEmailStore<IdentityUser> _emailStore;
//         private readonly ILogger<RegisterModel> _logger;
//         private readonly IEmailSender _emailSender;

//         public RegisterModel(
//             UserManager<IdentityUser> userManager,
//             IUserStore<IdentityUser> userStore,
//             SignInManager<IdentityUser> signInManager,
//             ILogger<RegisterModel> logger,
//             IEmailSender emailSender)
//         {
//             _userManager = userManager;
//             _userStore = userStore;
//             _emailStore = GetEmailStore();
//             _signInManager = signInManager;
//             _logger = logger;
//             _emailSender = emailSender;
//         }

//         /// <summary>
//         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//         ///     directly from your code. This API may change or be removed in future releases.
//         /// </summary>
//         [BindProperty]
//         public InputModel Input { get; set; }

//         /// <summary>
//         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//         ///     directly from your code. This API may change or be removed in future releases.
//         /// </summary>
//         public string ReturnUrl { get; set; }

//         /// <summary>
//         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//         ///     directly from your code. This API may change or be removed in future releases.
//         /// </summary>
//         public IList<AuthenticationScheme> ExternalLogins { get; set; }

//         /// <summary>
//         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//         ///     directly from your code. This API may change or be removed in future releases.
//         /// </summary>
//         public class InputModel
//         {
//             /// <summary>
//             ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//             ///     directly from your code. This API may change or be removed in future releases.
//             /// </summary>
//             [Required]
//             [EmailAddress]
//             [Display(Name = "Email")]
//             public string Email { get; set; }

//             /// <summary>
//             ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//             ///     directly from your code. This API may change or be removed in future releases.
//             /// </summary>
//             [Required]
//             [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
//             [DataType(DataType.Password)]
//             [Display(Name = "Password")]
//             public string Password { get; set; }

//             /// <summary>
//             ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//             ///     directly from your code. This API may change or be removed in future releases.
//             /// </summary>
//             [DataType(DataType.Password)]
//             [Display(Name = "Confirm password")]
//             [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
//             public string ConfirmPassword { get; set; }
//         }


//         public async Task OnGetAsync(string returnUrl = null)
//         {
//             ReturnUrl = returnUrl;
//             ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
//         }

//         public async Task<IActionResult> OnPostAsync(string returnUrl = null)
//         {
//             returnUrl ??= Url.Content("~/");
//             ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
//             if (ModelState.IsValid)
//             {
//                 var user = CreateUser();

//                 await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
//                 await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
//                 var result = await _userManager.CreateAsync(user, Input.Password);

//                 if (result.Succeeded)
//                 {
//                     _logger.LogInformation("User created a new account with password.");

//                     var userId = await _userManager.GetUserIdAsync(user);
//                     var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                     code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
//                     var callbackUrl = Url.Page(
//                         "/Account/ConfirmEmail",
//                         pageHandler: null,
//                         values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
//                         protocol: Request.Scheme);

//                     await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
//                         $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

//                     if (_userManager.Options.SignIn.RequireConfirmedAccount)
//                     {
//                         return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
//                     }
//                     else
//                     {
//                         await _signInManager.SignInAsync(user, isPersistent: false);
//                         return LocalRedirect(returnUrl);
//                     }
//                 }
//                 foreach (var error in result.Errors)
//                 {
//                     ModelState.AddModelError(string.Empty, error.Description);
//                 }
//             }

//             // If we got this far, something failed, redisplay form
//             return Page();
//         }

//         private IdentityUser CreateUser()
//         {
//             try
//             {
//                 return Activator.CreateInstance<IdentityUser>();
//             }
//             catch
//             {
//                 throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
//                     $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
//                     $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
//             }
//         }

//         private IUserEmailStore<IdentityUser> GetEmailStore()
//         {
//             if (!_userManager.SupportsUserEmail)
//             {
//                 throw new NotSupportedException("The default UI requires a user store with email support.");
//             }
//             return (IUserEmailStore<IdentityUser>)_userStore;
//         }
//     }
// }



using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using AdilBooks.Data;
using AdilBooks.Models;

namespace AdilBooks.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IUserStore<IdentityUser> userStore,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/"); 
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(Input.Email);
                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "You are already registered! Please log in.";
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Determine role based on ReturnUrl
                    string role = returnUrl.Contains("Shows/Register", StringComparison.OrdinalIgnoreCase)
                                    ? "Participant"
                                    : "Admin";

                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }

                    await _userManager.AddToRoleAsync(user, role);
                    _logger.LogInformation($"Assigned role '{role}' to user {Input.Email}");

                    // Save to Participants table only if Participant
                    if (role == "Participant")
                    {
                        var participant = new Participant
                        {
                            Email = Input.Email,
                            Name = Input.Email.Split('@')[0],
                            RegisteredAt = DateTime.UtcNow
                        };

                        _context.Participants.Add(participant);
                        await _context.SaveChangesAsync();
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        
                        // Redirect based on role
                        return role == "Participant"
                            ? LocalRedirect("~/Shows/MyShows")
                            : LocalRedirect("~/");
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }


        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
