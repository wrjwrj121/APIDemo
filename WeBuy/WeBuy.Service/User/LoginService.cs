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
    public class LoginService : ILoginService
    {

        private readonly EFCoreContext db;
        private readonly IMapper mapper;

        public LoginService(EFCoreContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        /// <summary>
        /// list
        /// </summary>
        /// <returns></returns>
        public async Task<PageAPIResult<UserInfoDTO>> Query()
        {
            var users = await db.UserInfo.Where(a => a.IsEnabled == true).ToListAsync();
            var dto = mapper.Map<List<UserInfoDTO>>(users);
            var result = new PageAPIResult<UserInfoDTO>
            {
                data = dto
            };
            result.Success();
            return result;
        }
       /// <summary>
       /// 登录
       /// </summary>
       /// <param name="userName"></param>
       /// <param name="passWord"></param>
       /// <returns></returns>
        public async Task<DataAPIResult<LoginDTO>> Login(string userName, string passWord)
        {
            var result = new DataAPIResult<LoginDTO>();
            var isSuccess = await db.UserInfo.Where(a => a.UserName == userName && a.PassWord == passWord).AnyAsync();

            if (isSuccess)
            {
                result.data = new LoginDTO()
                {
                    Token = "sucess"
                };
                result.Success("登录成功");
            }
            else
            {
                result.Fail("登录失败");
            }
               
            return result;
        }
    }
}
