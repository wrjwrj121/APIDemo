using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeBuy.Model.System;
using WeBuyModel.Common;

namespace WeBuy.IService.System
{
     public interface ISystemLogService
    {
        Task<PageAPIResult<SystemLog>> Query();
        Task<APIResult> Add(SystemLog add);
    }
}
