using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cremcircle.Models
{
    public class AppConfiguration
    {
        public long ID { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Index(IsUnique = true)]
        [Remote("IsAppKeyUnique", "AppConfigurations", AdditionalFields = "ID", ErrorMessage = "Application Key already in use")]
        public string AppKey { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        [Index(IsUnique = true)]
        [Remote("IsTitleUnique", "AppConfigurations", AdditionalFields = "ID", ErrorMessage = "Application Title already in use")]
        public string Title { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string AppGroup { get; set; }

        //0 = TextBox, 1 = TextArea, 2 = Editor
        [Required]
        public int FieldType { get; set; }

        [Required]
        [AllowHtml]
        public string AppConfigValue { get; set; }
    }
}