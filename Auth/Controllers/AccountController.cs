using Auth.DTO;
using Auth.Identity;
using Auth.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Auth.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private IConfiguration _configuration;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager
            , IJwtService jwtService , IConfiguration configuration, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _configuration = configuration;
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> Login(LoginDTO loginDTO)
        {
            //Validation
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }


            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user == null)
                {
                    //return NoContent();
                    ViewBag.ErrorMessage = "User is null";
                    return View();
                }

                //sign-in
                await _signInManager.SignInAsync(user, isPersistent: false);

                var authenticationResponse = _jwtService.CreateJwtToken(user);

                user.RefreshToken = authenticationResponse.RefreshToken;

                user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index", "Home");
                //return Ok(authenticationResponse);
            }

            else
            {
                //return Problem("Invalid email or password");
                ViewBag.ErrorMessage ="Invalid email or password";
                return View();
            }
            
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> Register(RegisterDTO model)
        {
            //Validation
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                //return Problem(errorMessage);
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }
            //Create user
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                PersonName = model.PersonName
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                string url = $"{_configuration["AppUrl"]}/Account/confirmemail?userid={user.Id}&token={validEmailToken}";

                 await _userService.SendEmailAsync(user.Email, "Confirm your email", $"<h1>Welcome to Auth Demo</h1>" +
                    $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");

                //sign-in
                await _signInManager.SignInAsync(user, isPersistent: false);

                var authenticationResponse = _jwtService.CreateJwtToken(user);
                user.RefreshToken = authenticationResponse.RefreshToken;

                user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index", "Home");
                //return Ok(authenticationResponse);
            }
            else
            {
                string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description)); //error1 | error2
                //return Problem(errorMessage);.
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }
        }
            
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
            //return NoContent();
        }

    }
}
