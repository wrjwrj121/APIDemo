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
    public class MenuInfoController : ControllerBase
    {
        private readonly IMenuInfoService service; 
        public MenuInfoController(IMenuInfoService _service) 
        {
            service = _service;
        }

        [HttpGet]
        public async Task<PageAPIResult<MenuInfoDTO>> Query()
        {
           var   result = await service.Query();
            return result;
        }

        [HttpPost]
        public async Task<DataAPIResult<MenuInfoDTO>> Add(MenuInfo menu)
        {
            var result = new DataAPIResult<MenuInfoDTO>();
            result = await service.Add(menu);
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
        public async Task<DataAPIResult<MenuInfoDTO>> Detail(string guid)
        {
           var  result = await service.Detail(guid);
            return result;
        }


        [HttpPost]
        public async Task<APIResult> Edit(MenuInfo menu)
        {
            var result = await service.Edit(menu);
            return result;
        }
    }
}
