using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QfbServer.Models
{
    public class MeasureData
    {
        //[ScaffoldColumn(false)]
        //public int dataId { get; set; }
        //[Required]
        //public int projectId { get; set; }
        //[Required]
        //public String projectName { get; set;
        //    [Required]}
        //public int productId { get; set; }
        //[Required]
        //public String productName { get; set; }
        //[Required]
        //public int targetId { get; set; }
        //[Required]
        //public String targetName { get; set; }
        //public String targetType { get; set; }
        //[Required]
        //public int pageId { get; set; }
        //[Required]
        //public String measure_point { get; set; }
        //public String direction { get; set; }
        //public String value1 { get; set; }
        //public String value2 { get; set; }
        //public String value3 { get; set; }
        //public String value4 { get; set; }
        //[Required]
        //public String username { get; set; }
        //[Required]
        //public long timestamp { get; set; }
        //public int uploaded { get; set; }


        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public Page Page { get; set; }

        /// <summary>
        /// MeasurePoint 的 Name 和 Id 用 , 隔开，放在一个字符串里
        /// </summary>
        public String MeasurePoint { get; set; }

        public String Value1 { get; set; }
        public String Value2 { get; set; }
        public String Value3 { get; set; }
        public String Value4 { get; set; }

        public String Username { get; set; }
        public long Timestamp { get; set; }
    }
}