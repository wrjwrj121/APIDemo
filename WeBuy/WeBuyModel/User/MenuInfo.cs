using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeBuy.Model.User
{
    /// <summary>
    /// 菜单
    /// </summary>
     public class MenuInfo
    {
        [Key]
        public string GUID { get; set; }
        public string Icon { get; set; }
        public int Level { get; set; }
        public string ParentID { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
