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
        private readonly IUserInfoService userInfoService;
        public UserController(IUserInfoService _userInfoService)
        {
            userInfoService = _userInfoService;
        }

        [HttpGet]
        public async Task<PageAPIResult<UserInfoDTO>> Query([FromQuery]UserQuery query)
        {
            var result = new PageAPIResult<UserInfoDTO>();
            result = await userInfoService.Query(query);
            return result;
        }

        [HttpPost]
        public async Task<DataAPIResult<UserInfoDTO>> Add(UserInfo user)
        {
            var result = new DataAPIResult<UserInfoDTO>();
            result = await userInfoService.Add(user);
            return result;
        }

        [HttpDelete("{guid}")]
        public async Task<APIResult> Delete(string guid)
        {
            var result = new APIResult();
            result = await userInfoService.Delete(guid);
            return result;
        }
        /// <summary>
        /// 移除用户角色
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<APIResult> DeleteUserRole(string guid)
        {
            var result = await userInfoService.DeleteUserRole(guid);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DataAPIResult<UserInfoDTO>> Detail(string guid)
        {
            var result = await userInfoService.Detail(guid);
            return result;
        }


        [HttpPost]
        public async Task<APIResult> Edit(UserInfo user)
        {
            var result = await userInfoService.Edit(user);
            return result;
        }
        /// <summary>
        /// 用户角色
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResult> UserRole(UserRole userRole)
        {
            var result = await userInfoService.UserRole(userRole);
            return result;
        }
    }
}
