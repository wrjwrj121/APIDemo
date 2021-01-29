using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeBuy.Model.User;
using WeBuyModel.Common;

namespace WeBuy.IService.User
{
    public interface IRoleInfoService
    {
        Task<PageAPIResult<RoleInfo>> Query();
        Task<DataAPIResult<RoleInfo>> Detail(string guid);
        Task<DataAPIResult<RoleInfo>> Add(RoleInfo role);
        Task<APIResult> Edit(RoleInfo role);
        Task<APIResult> Delete(string guid);
    }
}
