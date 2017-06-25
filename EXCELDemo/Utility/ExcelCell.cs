using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class ExcelCell
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string CellName { set; get; }

        /// <summary>
        /// X轴
        /// </summary>
        public int CellX { set; get; }

        /// <summary>
        /// Y轴
        /// </summary>
        public int CellY { set; get; }
    }
}
