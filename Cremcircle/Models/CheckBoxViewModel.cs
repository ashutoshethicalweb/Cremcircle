using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cremcircle.Models
{
    public class CheckBoxViewModel
    {
        public long ID { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}