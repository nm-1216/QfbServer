using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QfbServer.Models
{

    /// <summary>
    /// 预警
    /// </summary>
    public class Warning
    {
        public int id { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 信息1
        /// </summary>
        public string Filed1 { get; set; }

        /// <summary>
        /// 信息2
        /// </summary>
        public string Filed2 { get; set; }

        /// <summary>
        /// 预警类型
        /// </summary>
        public string WarningType { get; set; }

        /// <summary>
        /// 预警时间
        /// </summary>
        public DateTime PreWwarningTime { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? DoTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }


    }
}