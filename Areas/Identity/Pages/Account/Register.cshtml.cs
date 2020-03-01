using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HotelReservationsManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using HotelReservationsManager.Data;
using System.Linq;

namespace HotelReservationsManager.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly MyHRManagerDBContext hRManagerDBContext;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            MyHRManagerDBContext hRManagerDBContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.hRManagerDBContext = hRManagerDBContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Second Name")]
            public string SecondName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(10, ErrorMessage=" The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            [Display(Name ="UCN")]
            public string UCN { get; set; }
            [Required]
            [StringLength(10, ErrorMessage = " The {0} must be {2}")]
            [Display(Name = "Number")]
            public string Number { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")] 
            [DataType(DataType.Date)]
            [Display(Name = "AppointmentDate")]
            public DateTime AppointmentDate { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            
            if (ModelState.IsValid)
            {
                var user = new User { 
                    Id = Guid.NewGuid().ToString(), 
                    UserName = Input.Username,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    SecondName = Input.SecondName,
                    LastName = Input.LastName,
                    UCN = Input.UCN, PhoneNumber = Input.Number, AppointmentDate = Input.AppointmentDate, IsActive = true};

                var result = await _userManager.CreateAsync(user, Input.Password);
                
                if (result.Succeeded)
                {
                    if(this.hRManagerDBContext.Users.Count()==1)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin"); 
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);   
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
