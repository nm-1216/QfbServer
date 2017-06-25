using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QfbServer.Models
{
    public class MeasurementItem
    {        
        [Key]
        public int MeasItemID { get; set; }
        public int MeasReportID { get; set; }
        public string MeasItemNO { get; set; }
        public string MeasItemName { get; set; }
        public int? Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
    }
}