using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeBuy.Controllers.Base;
using WeBuy.IService.User;
using WeBuy.Model.User;
using WeBuyModel.Common;

namespace WeBuy.Controllers
{
    [ApiExplorerSettings(GroupName = "User")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleInfoController : MyBaseController
    {
        private readonly IRoleInfoService service; 
        public RoleInfoController(IRoleInfoService _service) 
        {
            service = _service;
        }

        [HttpGet]
        public async Task<PageAPIResult<RoleInfo>> Query()
        {
           var   result = await service.Query();
            return result;
        }

        [HttpPost]
        public async Task<DataAPIResult<RoleInfo>> Add(RoleInfo role)
        {
            var result = new DataAPIResult<RoleInfo>();
            result = await service.Add(role);
            return result;
        }

        [HttpDelete("{guid}")]
        public async Task<APIResult> Delete(string guid)
        {
             var  result = await service.Delete(guid);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DataAPIResult<RoleInfo>> Detail(string guid)
        {
           var  result = await service.Detail(guid);
            return result;
        }


        [HttpPost]
        public async Task<APIResult> Edit(RoleInfo role)
        {
            var result = await service.Edit(role);
            return result;
        }
    }
}
