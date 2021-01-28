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
    [ApiExplorerSettings(GroupName = "User")]
    [ApiController]
    [Description("用户")]
    public class UserController : ControllerBase
    {
        private readonly ILoginService service;
        public UserController(ILoginService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<PageAPIResult<UserInfoDTO>> Query()
        {
            var result = new PageAPIResult<UserInfoDTO>();
            result = await service.Query();
            return result;
        }
    }
}
