using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeBuy.Common;
using WeBuy.IService.User;
using WeBuy.Model.User;
using WeBuyModel.Common;

namespace WeBuy.Service.User
{
    public class LoginService : ILoginService
    {

        private readonly EFCoreContext db;

        public LoginService(EFCoreContext _db)
        {
            db = _db;
        }
        public PageAPIResult<UserInfo> GetUsers()
        {
            throw new NotImplementedException();
        }
       /// <summary>
       /// 登录
       /// </summary>
       /// <param name="userName"></param>
       /// <param name="passWord"></param>
       /// <returns></returns>
        public APIResult Login(string userName, string passWord)
        {
            var result = new APIResult();
            var isSuccess =  db.UserInfo.Where(a => a.UserName == userName && a.PassWord == passWord).Any();

            if (isSuccess)
                result.Success("登录成功");
            else
                result.Fail("登录失败");
            return result;
        }
    }
}
