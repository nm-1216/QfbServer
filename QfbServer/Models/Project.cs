using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QfbServer.Models
{
    public class Project
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string TargetName { get; set; }

        [Required]
        public List<Page> Pages { get; set; }
    }
}