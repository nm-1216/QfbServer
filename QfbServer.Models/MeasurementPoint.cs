using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QfbServer.Models
{
    public class MeasurementPoint
    {
        [Key]
        public int MeasPointID { get; set; }
        public int MeasPageID { get; set; }
        public string PointNo { get; set; }
        public string PointType { get; set; }
        public string XAxis { get; set; }
        public string YAxis { get; set; }
        public string ZAxis { get; set; }
        public string UpperTol { get; set; }
        public string LowerTol { get; set; }
        public string Direct { get; set; }
        public string AVG { get; set; }
        public string CorrectDirect { get; set; }

        public int? Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
    }
}