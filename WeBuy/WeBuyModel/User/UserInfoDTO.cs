using System;
using System.Collections.Generic;
using System.Text;

namespace WeBuy.Model.User
{
   public class UserInfoDTO
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsEnabled { get; set; }
        public string Remark { get; set; }
    }
}
