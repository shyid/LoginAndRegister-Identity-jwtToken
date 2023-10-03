using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Auth.DTO
{
     public class EditUserDTO
    {
          public Guid Id { get; set; }
        
          [Required(ErrorMessage = "Person Name can't be blank")]
          public string PersonName { get; set; } = string.Empty;


          [Required(ErrorMessage = "Email can't be blank")]
          [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
          //[Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already is use")]
          public string Email { get; set; } = string.Empty;


          [Required(ErrorMessage = "Phone number can't be blank")]
          [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain digits only")]
          public string PhoneNumber { get; set; } = string.Empty;


        //[Required(ErrorMessage = "Password can't be blank")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,15}$", ErrorMessage = "The password is incorrect")]
        public string Password { get; set; } = string.Empty;


          //[Required(ErrorMessage = "Confirm Password can't be blank")]
          [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
          public string ConfirmPassword { get; set; } = string.Empty;
     }
}
