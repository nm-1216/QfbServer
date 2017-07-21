using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QfbServer.Models
{
    public partial class User
    {


        [ScaffoldColumn(false)]
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public virtual UserType userType { get; set; }
    }

    public enum UserType : int
    {
        Application = 0,
        Pad = 1
    }
}
