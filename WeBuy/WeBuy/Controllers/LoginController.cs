using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeBuy.IService.User;
using WeBuyModel.Common;

namespace WeBuy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "User")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService service;
        public LoginController(ILoginService _service) 
        {
            service = _service;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        [HttpGet]
        public APIResult UserLogin(string userName, string passWord) 
        {
            var result = new APIResult();
            result =  service.Login(userName,passWord);
            return result;
        }
        [HttpGet]
        public void Test()
        {
            try
            {
                int i = 0;
                var t = 9 / i;
                Console.WriteLine(t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
