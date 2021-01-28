using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WeBuy.Controllers.Base;
using WeBuy.IService.System;
using WeBuy.Model.System;
using WeBuyModel.Common;

namespace WeBuy.Controllers
{
    [ApiExplorerSettings(GroupName = "Base")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemlogController : MyBaseController
    {
        private readonly ISystemLogService service;
        public SystemlogController(ISystemLogService _service) 
        {
            service = _service;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageAPIResult<SystemLog>> Query()
        {
            return await service.Query();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
       [HttpPost]
        public async Task<APIResult> Add(SystemLog add) 
        {
            return await service.Add(add);
        }
    }
}
