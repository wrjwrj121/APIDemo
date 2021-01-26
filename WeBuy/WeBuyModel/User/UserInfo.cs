using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WeBuy.Model.User
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        public string GUID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsEnable { get; set; }
        public string Remark { get; set; }
    }
}
