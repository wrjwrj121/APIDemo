using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeBuy.Model.User;
using WeBuyModel.Common;

namespace WeBuy.IService.User
{
   public   interface ILoginService
    {
        PageAPIResult<UserInfo> GetUsers();
        APIResult Login(string userName ,string passWord);
    }
}
