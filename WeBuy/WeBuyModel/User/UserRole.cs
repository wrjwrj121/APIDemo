using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeBuy.Model.User
{
    public  class UserRole
    {
        [Key]
        public string GUID { get; set; }
        public string RoleID { get; set; }
        public string UserID { get; set; }
    }
}
