using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QfbServer.Models
{
    public class MeasureData
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public String ProjectName {get; set;}
        [Required]
        public int ProductId { get; set; }
        [Required]
        public String ProductName { get; set; }
        [Required]
        public int TargetId { get; set; }
        [Required]
        public String TargetName { get; set; }
        public String TargetType { get; set; }
        [Required]
        public int PageId { get; set; }
        public String MeasurePoint { get; set; }
        public String Direction { get; set; }
        public String Value1 { get; set; }
        public String Value2 { get; set; }
        public String Value3 { get; set; }
        public String Value4 { get; set; }
        [Required]
        public String Username { get; set; }
        [Required]
        public long Timestamp { get; set; }


        //[ScaffoldColumn(false)]
        //public int Id { get; set; }

        //public Page Page { get; set; }

        ///// <summary>
        ///// MeasurePoint 的 Name 和 Id 用 , 隔开，放在一个字符串里
        ///// </summary>
        //public String MeasurePoint { get; set; }

        //public String Value1 { get; set; }
        //public String Value2 { get; set; }
        //public String Value3 { get; set; }
        //public String Value4 { get; set; }

        //public String Username { get; set; }
        //public long Timestamp { get; set; }
    }
}