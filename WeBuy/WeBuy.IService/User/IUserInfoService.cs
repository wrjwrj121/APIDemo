using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeBuy.Model.User;
using WeBuyModel.Common;

namespace WeBuy.IService.User
{
    public interface IUserInfoService
    {
        Task<PageAPIResult<UserInfoDTO>> Query();
        Task<DataAPIResult<UserInfoDTO>> Detail(string guid);
        Task<DataAPIResult<UserInfoDTO>> Add(UserInfo user);
        Task<APIResult> Edit(UserInfo user);
        Task<APIResult> Delete(string guid);
        Task<APIResult> UserRole(UserRole userRole);
        Task<APIResult> DeleteUserRole(string guid);

    }
}
