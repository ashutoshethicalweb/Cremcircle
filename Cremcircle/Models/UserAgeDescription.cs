using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cremcircle.Models
{
    public class UserAgeDescription
    {
        [Display(Name = "User Age Description ID")]
        public long ID { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Role Name")]
        [Index(IsUnique = true)]
        //[Remote("IsTitleUnique", "Roles", AdditionalFields = "ID", ErrorMessage = "Role is already in use.")]
        public string AgeDescription { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }



        [Required]
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }

    }
}