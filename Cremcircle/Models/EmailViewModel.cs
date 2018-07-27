using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cremcircle.Models
{
    public class EmailViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string ToEmail { get; set; }

        [Display(Name = "Body")]
        [DataType(DataType.MultilineText)]
        public string EMailBody { get; set; }

        [Display(Name = "Subject")]
        public string EmailSubject { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "CC")]
        public string EmailCC { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "BCC")]
        public string EmailBCC { get; set; }
    }
}