using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cremcircle.Models
{
    public class SecurityTemplatePermission
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Security Template Permissions ID")]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Security Template")]
        public long SecurityTemplateID { get; set; }
        public virtual SecurityTemplate SecurityTemplate { get; set; }

        [Required]
        [Display(Name = "Permission")]
        public long PermissionID { get; set; }
        public virtual Permission Permission { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }
    }
}