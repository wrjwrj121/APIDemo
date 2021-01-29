using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeBuy.Common;
using WeBuy.IService.User;
using WeBuy.Model.User;
using WeBuyModel.Common;

namespace WeBuy.Service.User
{
    public class RoleInfoService : IRoleInfoService
    {
        private readonly EFCoreContext db;
        private readonly IMapper mapper;
        public RoleInfoService(EFCoreContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        public async Task<DataAPIResult<RoleInfo>> Add(RoleInfo role)
        {
            var result = new DataAPIResult<RoleInfo>
            {
                data = new RoleInfo()
            };
          
            try
            {
                if (role == null)
                {
                   throw new Exception("数据不能为空");
                }
                role.CreateTime = DateTime.Now;
                role.GUID = Guid.NewGuid().ToString();
                await db.RoleInfo.AddAsync(role);
                await db.SaveChangesAsync();
                result.data = await db.RoleInfo.Where(a => a.GUID == role.GUID).FirstAsync();
                result.Success();
            }
            catch (Exception ex)
            {
                result.Fail(ex.Message);
            }
            return result;
        }

        public async Task<APIResult> Delete(string guid)
        {
            var result = new APIResult();
            var role = await db.RoleInfo.Where(a => a.GUID == guid).FirstAsync();
            if (role == null)
            {
                result.Fail("无此记录");
                return result;

            }
            db.RoleInfo.Remove(role);
            db.SaveChanges();
            result.Success();

            return result;
        }

        public async Task<DataAPIResult<RoleInfo>> Detail(string guid)
        {
            var result = new DataAPIResult<RoleInfo>
            {
                data = new RoleInfo()
            };
            try
            {
                result.data = await db.RoleInfo.Where(a => a.GUID == guid).FirstAsync();
                result.Success();
            }
            catch (Exception ex)
            {

                result.Fail(ex.Message);
            }
            return result;
        }

        public async Task<APIResult> Edit(RoleInfo role)
        {
            var result = new APIResult();

            try
            {
                db.RoleInfo.Update(role);
                await db.SaveChangesAsync();
                result.Success();
            }
            catch (Exception ex)
            {
                result.Fail(ex.Message);
            }
            return result;
        }

        public async Task<PageAPIResult<RoleInfo>> Query()
        {
            var users = await db.RoleInfo.ToListAsync();
            var result = new PageAPIResult<RoleInfo>
            {
                data = users
            };
            result.Success();
            return result;
        }
    }
}
