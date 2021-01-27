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
        Task<PageAPIResult<UserInfoDTO>> Query();
        Task<DataAPIResult<LoginDTO>> Login(string userName ,string passWord);
    }
}
