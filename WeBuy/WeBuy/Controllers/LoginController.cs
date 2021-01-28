using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WeBuy.IService.User;
using WeBuy.Model.User;
using WeBuyModel.Common;

namespace WeBuy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Authorize")]
    [ApiController]
    [Description("登录")]
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
        public async Task<DataAPIResult<LoginDTO>> UserLogin(string userName, string passWord) 
        {
            var result = new DataAPIResult<LoginDTO>();
            result = await service.Login(userName,passWord);
            return result;
        }
    }
}
