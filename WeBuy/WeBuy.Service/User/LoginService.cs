using AutoMapper;
//using AutoMapper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WeBuy.Common;
using WeBuy.IService.User;
using WeBuy.Model.User;
using WeBuyModel.Common;
using Microsoft.Extensions.Configuration;
using WeBuy.Common.MD5;

namespace WeBuy.Service.User
{
    public class LoginService : ILoginService
    {

        private readonly EFCoreContext db;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public LoginService(EFCoreContext _db, IMapper _mapper, IConfiguration _configuration)
        {
            db = _db;
            mapper = _mapper;
            configuration = _configuration;
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
            var md5 = MD5Encrypt.Encrypt(passWord);
            var user = await db.UserInfo.Where(a => a.UserName == userName && a.PassWord ==md5 ).FirstAsync();

            if (user!=null)
            {
                result.data = new LoginDTO()
                {
                    Token = CreateToken(user)
                };
                result.Success("登录成功");
            }
            else
            {
                result.Fail("登录失败");
            }
               
            return result;
        }

        private string CreateToken(UserInfo user)
        {
            //这里也是从配置文件读取的，和上面读取的一致，否则开启对应验证的话会不通过
            string secret = configuration.GetValue<string>("JWT:Secrete");
            string issuer_z = configuration.GetValue<string>("JWT:Issuer");
            string audience_z = configuration.GetValue<string>("JWT:Audience");
            //指定加密算法
            var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)), SecurityAlgorithms.HmacSha256);
            //可以在Token里增加信息，但这里不要加私密信息，比如密码等这种数据
            var claims = new Claim[] { new Claim("UserName", user.UserName) , new Claim("GUID",user.GUID),  new Claim("IsEnabled" , user.IsEnabled.ToString())};

            //组装数据
            SecurityToken securityToken = new JwtSecurityToken(
                  issuer: issuer_z,//颁发者
                  audience: audience_z,//接收者
                  signingCredentials: securityKey,//组装的秘钥

                  expires: DateTime.Now.AddMinutes(10),//有效时间
                  claims: claims
                );
            //生成Token
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
