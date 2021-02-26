using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeBuyModel.Common;

namespace WeBuy.IService.Base
{
    public interface IBaseService
    {
        Task<bool> IsManager();

    }
}
