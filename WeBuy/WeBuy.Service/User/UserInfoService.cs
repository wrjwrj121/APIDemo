using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeBuy.Common;
using WeBuy.Common.DB;
using WeBuy.Common.EncryptHelper;
using WeBuy.Common.MD5;
using WeBuy.IService.Base;
using WeBuy.IService.User;
using WeBuy.Model.User;
using WeBuyModel.Common;

namespace WeBuy.Service.User
{
    public class UserInfoService : IUserInfoService
    {
        private readonly EFCoreContext db;
        private readonly IMapper mapper;
        private readonly ILogger log;
        private readonly IBaseService baseService;
        public UserInfoService(EFCoreContext _db,IMapper _mapper, ILogger<UserInfoService> _log, IBaseService _baseService) 
        {
            db = _db;
            mapper = _mapper;
            log = _log;
            baseService = _baseService;
        }

        public async Task<DataAPIResult<UserInfoDTO>> Add(UserInfo user)
        {
            var result = new DataAPIResult<UserInfoDTO> 
            {
             data = new UserInfoDTO()
            };
            if (user == null)
            {
                result.Fail("数据不能为空");
                return result;

            }
            try
            {
                user.CreateTime = DateTime.Now;
                user.GUID = Guid.NewGuid().ToString();

                user.PassWord = DesEncrypt.Encrypt(user.PassWord);
                //user.PassWord = MD5Encrypt.Encrypt(user.PassWord);
                await db.UserInfo.AddAsync(user);
                await db.SaveChangesAsync();
                var userInfo = await db.UserInfo.Where(a=> a.GUID == user.GUID).FirstAsync();
                result.data = mapper.Map<UserInfoDTO>(userInfo);
                result.Success();
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                result.Fail(ex.Message);
            }
            return result;
        }

        public async Task<APIResult> Delete(string guid)
        {
            var result = new APIResult();
            var user = await db.UserInfo.Where(a => a.GUID == guid).FirstAsync();
            if (user == null)
            {
                result.Fail("无此记录");
                return result;

            }
            db.UserInfo.Remove(user);
            db.SaveChanges();
            result.Success();

            return result;
        }
        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<APIResult> DeleteUserRole(string guid)
        {
            var result = new APIResult();
            var userRole = await db.UserRole.Where(a => a.GUID == guid).FirstAsync();
            if (userRole == null)
            {
                result.Fail("无此记录");
                return result;

            }
            db.UserRole.Remove(userRole);
            db.SaveChanges();
            result.Success();

            return result;
        }

        public async Task<DataAPIResult<UserInfoDTO>> Detail(string guid)
        {
            var result = new DataAPIResult<UserInfoDTO>
            {
                data = new UserInfoDTO()
            };
            try
            {
                var user = await db.UserInfo.Where(a => a.GUID == guid).FirstAsync();
                user.PassWord = DesEncrypt.Decrypt(user.PassWord);
                result.data = mapper.Map<UserInfoDTO>(user);
                result.Success();
            }
            catch (Exception ex)
            {

                result.Fail(ex.Message);
            }
            return result;
        }

        public async Task<APIResult> Edit(UserInfo user)
        {
            var result = new APIResult();

            try
            {
                user.PassWord = MD5Encrypt.Encrypt(user.PassWord);

                db.UserInfo.Update(user);
                await  db.SaveChangesAsync();
                result.Success();
            }
            catch (Exception ex)
            {
                result.Fail(ex.Message);
            }
            return result;


        }

        public async Task<PageAPIResult<UserInfoDTO>> Query(UserQuery query)
        {
            var isSys = baseService.IsManager();
            var users = await db.UserInfo.Where(a => a.IsEnabled == true).ToListAsync();
            var count = users.Count;
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                users = users.Where(a=>a.Name.Contains(query.Name)).Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToList();
            }
            else
            {
                users = users.Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToList();
            }
            
            var dto = mapper.Map<List<UserInfoDTO>>(users);
            var result = new PageAPIResult<UserInfoDTO>
            {
                data = dto,
                Count = count
            };
            result.Success();
            return result;
        }

        public async Task<APIResult> UserRole(UserRole userRole)
        {
            userRole.GUID = Guid.NewGuid().ToString();
            await db.UserRole.AddAsync(userRole);
            var result = new APIResult();
            result.Success();
            return result;
        }
    }
}
