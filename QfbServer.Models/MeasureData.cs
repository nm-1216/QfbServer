using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QfbServer.Models
{
    public class MeasureData
    {
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public static readonly string CONN = "conn";


        [ScaffoldColumn(false)]
        public int Id { get; set; }

        /// <summary>
        /// 检测点编号
        /// </summary>
        public int PointId { get; set; }

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
        public String UpperTolerance { get; set; }
        public String LowerTolerance { get; set; }
        public String Value1 { get; set; }
        public String Value2 { get; set; }
        public String Value3 { get; set; }
        public String Value4 { get; set; }
        public String Value5 { get; set; }
        public String Value6 { get; set; }
        public String Value7 { get; set; }
        public String Value8 { get; set; }
        public String Value9 { get; set; }
        public String Value10 { get; set; }
        [Required]
        public String Username { get; set; }
        [Required]
        public long Timestamp { get; set; }

        [NotMapped]
        public DateTime checktime { get {
                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime date = start.AddMilliseconds(Timestamp).ToLocalTime();

                return date;
            } }

        

    }
}