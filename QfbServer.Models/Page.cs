using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QfbServer.Models
{
    public class Page
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public string PageName { get; set; }
        
        /// <summary>
        /// 一个 MeasurePoint 里面的 Name 和 CC 用 , 分开
        /// </summary>
        public List<string> MeasurePoints { get; set; }

        public List<string> Pictures { get; set; }

        public Project Project { get; set; }
    }
}