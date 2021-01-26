using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WeBuy.Model.User;

namespace WeBuy.Common.AutoProfile
{
   public   class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserInfo, UserInfoDTO>();//ForMember 自定义
        }
    }
}
