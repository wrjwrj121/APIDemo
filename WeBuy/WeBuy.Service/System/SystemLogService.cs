using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeBuy.Common;
using WeBuy.IService.System;
using WeBuy.Model.System;
using WeBuyModel.Common;

namespace WeBuy.Service.System
{
    public class SystemLogService : ISystemLogService
    {
        private readonly EFCoreContext db;
        private readonly IMapper mapper;

        public SystemLogService(EFCoreContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public async Task<APIResult> Add(SystemLog add)
        {
            add.ID = Guid.NewGuid().ToString();
            add.CreateTime = DateTime.Now;
            await db.SystemLog.AddAsync(add);
            await db.SaveChangesAsync();
            var result = new APIResult();
            result.Success();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<PageAPIResult<SystemLog>> Query()
        {
            var result = new PageAPIResult<SystemLog>();
            result.data = new List<SystemLog>();
            result.data = await db.SystemLog.ToListAsync();
            result.Success();
            return result;
        }
    }
}
