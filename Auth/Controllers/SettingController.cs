using Auth.DTO;
using Auth.Identity;
using Auth.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System;
using System.Text;
//using System.Web.Helpers.Crypto;

namespace Auth.Controllers
{
    public class SettingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SettingController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager
          )
        {
            _userManager = userManager;
            _roleManager = roleManager;
    
        }
        [Authorize]
        public async Task<IActionResult> EditUser()
        {
            var Userin = await _userManager.FindByEmailAsync(User.Identity.Name);
            EditUserDTO user = new EditUserDTO();

            user.Id = (Guid)Userin.Id;
            user.PersonName = Userin.PersonName;
            user.PhoneNumber = Userin.PhoneNumber;

            //if(user.Password is not null)
            //{
            //    user.Password = "pass";
            //    user.ConfirmPassword ="pass";
            //}


            return View(user);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserDTO editUser)
        {
            //var userfindId = await _userManager.FindByEmailAsync(User.Identity.Name);

            var user = await _userManager.FindByIdAsync(editUser.Id.ToString());

            
            // Update it with the values from the view model
            user.PersonName = editUser.PersonName;
            user.PhoneNumber = editUser.PhoneNumber;
            //if (editUser.Email != user.Email)
            //{
            //    //user.Email = editUser.Email;
            //    var token = await _userManager.GenerateChangeEmailTokenAsync(user, editUser.Email);
            //    await _userManager.ChangeEmailAsync(user, editUser.Email, token) ;
            //    user.EmailConfirmed = false;
            //}
            //if (editUser.Password != "pass")
            //{
            //    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, editUser.Password);
                
            //}
           
            // Apply the changes if any to the db
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "Home");
        }
    }
}
