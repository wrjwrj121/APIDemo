using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeBuy.Model.User;
using WeBuyModel.Common;

namespace WeBuy.IService.User
{
    public interface IMenuInfoService
    {
        Task<PageAPIResult<MenuInfoDTO>> Query();
        Task<DataAPIResult<MenuInfoDTO>> Detail(string guid);
        Task<DataAPIResult<MenuInfoDTO>> Add(MenuInfo menu);
        Task<APIResult> Edit(MenuInfo menu);
        Task<APIResult> Delete(string guid);
    }
}
