using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QfbServer.Models
{
    public class MeasurementPage
    {
        [Key]
        public int MeasPageID { get; set; }
        public int MeasItemID { get; set; }
        public string MeasPageNo { get; set; }
        public string MeasPageImage { get; set; }
        public int? Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
    }
}