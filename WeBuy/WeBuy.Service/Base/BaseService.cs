using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WeBuy.IService.Base;

namespace WeBuy.Service.Base
{
    public class BaseService : IBaseService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public  ClaimsPrincipal user => httpContextAccessor.HttpContext?.User;
        public BaseService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> IsManager()
        {
            var uses = this.user;
            var user1 = httpContextAccessor.HttpContext.User;
            var isM =  true;
            return isM;
        }
    }
}
