using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cremcircle.Models
{
    public class Country
    {

        [Display(Name = "Country ID")]
        public long ID { get; set; }

        [Required]
        [StringLength(400, MinimumLength = 2)]
        [Display(Name = "Country  Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        [DefaultValue(true)]
        public Boolean IsActive { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Modified Date")]
        public System.DateTime ModifiedDate { get; set; }

    }
}