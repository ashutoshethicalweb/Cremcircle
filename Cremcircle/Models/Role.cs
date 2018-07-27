using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cremcircle.Models
{
    public class Role
    {
        [Display(Name = "Role ID")]
        public long ID { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Role Name")]
        [Index(IsUnique = true)]
        [Remote("IsTitleUnique", "Roles", AdditionalFields = "ID", ErrorMessage = "Role is already in use.")]
        public string RoleName { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}