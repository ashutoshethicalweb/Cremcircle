using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cremcircle.Models
{


    public class RegisterViewModel
    {
        [Display(Name = "User ID")]
        public long ID { get; set; }

        [Required(ErrorMessage = "Please enter valid first name")]
        [Display(Name = "First Name")]
        [StringLength(150, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter valid last name")]

        [Display(Name = "Last Name")]
        [StringLength(150, MinimumLength = 2)]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please enter valid email"), EmailAddress]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Email Address")]
        [Index(IsUnique = true)]
        [Remote("IsLoginNameUnique", "Account", AdditionalFields = "ID", ErrorMessage = "Email is already in use.")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Please enter valid 10 digits mobile no.")]
        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Please select Your Age")]
        [Display(Name = "User Age Description")]
        public long UserAgeDescriptionID { get; set; }
        public virtual UserAgeDescription UserAgeDescription { get; set; }

        [Required(ErrorMessage = "Please select You are")]
        [Display(Name = "Role")]
        public long RoleID { get; set; }
        public virtual Role Role { get; set; }

        [Required(ErrorMessage = "Please select Country")]
        
        [Display(Name = "Country")]
        public long CountryID { get; set; }
        public virtual Country Country { get; set; }


        [Required(ErrorMessage = "Please enter valid captcha")]

        [Display(Name = "Captcha")]
        public string Captcha { get; set; }

        public string capchadata { get; set; }
}



        public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login Name")]
        public string LoginName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

      public string ReturnUrl { get; set; }

        //[Required]

        //[Display(Name = "Captcha")]
        //public string Captcha { get; set; }

      

        [Display(Name = "Remember me")]
        public Boolean ChkRememberMe { get; set; }


    }

    public class ForgotPasswordViewModel
    {
        //[Required]
        //[Display(Name = "LoginName")]
        //public string LoginName { get; set; }

        [Required, EmailAddress]
        [StringLength(150, MinimumLength = 2)]
        public string EmailAddress { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Remote("IsOldPasswordMatching", "Users", ErrorMessage = "Old Password does not match with the account.")]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Remote("IsPasswordSame", "Users", AdditionalFields = "NewPassword", ErrorMessage = "Both New Passwords are different.")]
        [Display(Name = "Repeat New Password")]
        public string RepeatPassword { get; set; }
    }

    public class UserProfileViewModel
    {
        [Display(Name = "Login Name")]
        public string LoginName { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }



        [Required, EmailAddress]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required, Phone]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(500)]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
    }


}