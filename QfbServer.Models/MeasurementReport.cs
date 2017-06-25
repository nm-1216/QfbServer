using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QfbServer.Models
{
    public class MeasurementReport
    {
        [Key]
        [ScaffoldColumn(false)]
        public int MeasReportID { get; set; }
        public string ProjectNo { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public DateTime CreateTime { get; set; }
        public string Creator { get; set; }
        public int? Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
    }
}