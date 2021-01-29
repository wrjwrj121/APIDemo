using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeBuy.Model.User
{
    public class RoleInfo
    {
        [Key]
        public string GUID { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
