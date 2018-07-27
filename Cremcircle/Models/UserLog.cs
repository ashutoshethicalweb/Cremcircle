using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cremcircle.Models
{
    public class UserLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "User Log ID")]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public long UserID { get; set; }
        public virtual User User { get; set; }

        [Display(Name = "Access Type")]
        public string AccessType { get; set; }

        [Display(Name = "Access Date")]
        public System.DateTime AccessDate { get; set; }

        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Access IP")]
        public string AccessIP { get; set; }
    }
}