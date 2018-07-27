using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cremcircle.Models
{
    public class User
    {
        [Display(Name = "User ID")]
        public long ID { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Login Name")]
        [Index(IsUnique = true)]
        public string LoginName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, EmailAddress]
        [StringLength(150, MinimumLength = 2)]
        [Index(IsUnique = true)]
        [Remote("IsLoginNameUnique", "Users", AdditionalFields = "ID", ErrorMessage = "Login Name is already in use.")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [StringLength(500)]
        [Display(Name = "User Image")]
        public string UserImage { get; set; }

        //[StringLength(500)]
        //[Display(Name = "User Signature")]
        //public string UserSignature { get; set; }

      

        [StringLength(10, MinimumLength = 10)]
        [Display(Name = "Mobile No")]
        public string Phonenumber { get; set; }

        //[StringLength(10, MinimumLength = 10)]
        //[Display(Name = "Fax")]
        //public string Fax { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }

        [Required(ErrorMessage = "Please select Your Age")]
        [Display(Name = "User Age Description")]
        public long UserAgeDescriptionID { get; set; }
        public virtual UserAgeDescription UserAgeDescription { get; set; }

        [Required]
        [Display(Name = "Role")]
        public long RoleID { get; set; }
        public virtual Role Role { get; set; }

        [Required]
        [Display(Name = "SecurityTemplate")]
        public long SecurityTemplateID { get; set; }
        public virtual SecurityTemplate SecurityTemplate { get; set; }

        [Required]
        [Display(Name = "Country")]
        public long CountryID { get; set; }
        public virtual Country Country { get; set; }


        [Required]
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }


    }
}