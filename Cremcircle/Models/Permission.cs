using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cremcircle.Models
{
    public class Permission
    {
        [Display(Name = "Permission ID")]
        public long ID { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Controller Name")]
        public string ControllerName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Action Name")]
        public string ActionName { get; set; }

        [Required]
        [Display(Name = "Only Admin And Hidden")]
        public Boolean OnlyAdminHidden { get; set; }

        public virtual ICollection<SecurityTemplatePermission> SecurityTemplatePermissions { get; set; }
    }
}