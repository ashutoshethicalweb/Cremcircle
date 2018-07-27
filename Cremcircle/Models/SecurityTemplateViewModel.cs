using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cremcircle.Models
{
    public class SecurityTemplateViewModel
    {
        [Display(Name = "Security Template ID")]
        public long ID { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Security Template Name")]
        [Index(IsUnique = true)]
        [Remote("IsSecurityTemplateNameUnique", "SecurityTemplates", AdditionalFields = "ID", ErrorMessage = "Security Template Name is already in use.")]
        public string SecurityTemplateName { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }

        public List<CheckBoxViewModel> Permissions { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<SecurityTemplatePermission> SecurityTemplatePermissions { get; set; }
    }
}