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
    public class MenuInfoService : IMenuInfoService
    {
        private readonly EFCoreContext db;
        private readonly IMapper mapper;
        public MenuInfoService(EFCoreContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        public async Task<DataAPIResult<MenuInfoDTO>> Add(MenuInfo menu)
        {
            var result = new DataAPIResult<MenuInfoDTO>
            {
                data = new MenuInfoDTO()
            };
          
            try
            {
                if (menu == null)
                {
                   throw new Exception("数据不能为空");
                }
                menu.CreateTime = DateTime.Now;
                menu.GUID = Guid.NewGuid().ToString();
                await db.MenuInfo.AddAsync(menu);
                await db.SaveChangesAsync();
                var parentMenu = await db.MenuInfo.Where(a => a.GUID == menu.GUID).FirstAsync();
                result.data = mapper.Map<MenuInfoDTO>(parentMenu);
           
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
            var menu = await db.MenuInfo.Where(a => a.GUID == guid).FirstAsync();
            if (menu == null)
            {
                result.Fail("无此记录");
                return result;

            }
            db.MenuInfo.Remove(menu);
            db.SaveChanges();
            result.Success();

            return result;
        }

        public async Task<DataAPIResult<MenuInfoDTO>> Detail(string guid)
        {
            var result = new DataAPIResult<MenuInfoDTO>
            {
                data = new MenuInfoDTO()
            };
            try
            {
                var menu = await db.MenuInfo.Where(a => a.GUID == guid).FirstAsync();
                result.data = mapper.Map<MenuInfoDTO>(menu);
                result.Success();
            }
            catch (Exception ex)
            {

                result.Fail(ex.Message);
            }
            return result;
        }

        public async Task<APIResult> Edit(MenuInfo menu)
        {
            var result = new APIResult();

            try
            {
                db.MenuInfo.Update(menu);
                await db.SaveChangesAsync();
                result.Success();
            }
            catch (Exception ex)
            {
                result.Fail(ex.Message);
            }
            return result;
        }

        public async Task<PageAPIResult<MenuInfoDTO>> Query()
        {
            var menus = await db.MenuInfo.Where(a=> string.IsNullOrEmpty(a.ParentID)).ToListAsync();
            var result = new PageAPIResult<MenuInfoDTO>
            {
                data = mapper.Map<List<MenuInfoDTO>>(menus)
            };

            var parentIds = result.data.Select(a => a.GUID).ToList();
            var childMneu = await db.MenuInfo.Where(a => parentIds.Contains(a.ParentID)).ToListAsync();

            result.data.ToList().ForEach(a=> 
            {
                var childdto = childMneu.Where(b => b.ParentID == a.GUID).ToList();
                a.ChildMenu = mapper.Map<List<MenuInfoDTO>>(childdto).ToList();
            });
            result.data = mapper.Map<List<MenuInfoDTO>>(result.data);
            result.Success();
            return result;
        }
    }
}
