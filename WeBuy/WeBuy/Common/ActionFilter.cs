using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WeBuy.IService.System;
using WeBuy.Model.System;
using WeBuyModel.Common;

namespace WeBuy.Common
{
    /// <summary>
    /// 方法过滤器(拦截方法执行前后数据)
    /// </summary>
    public class ActionFilter : IActionFilter, IAsyncActionFilter
    {
        private readonly ILogger logger;
        private readonly ISystemLogService system;

        public ActionFilter(ILogger<ActionFilter> logger, ISystemLogService _system)
        {
            this.logger = logger;
            system = _system;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
           
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            if (result.Result is ObjectResult)
            {
                try
                {
                    string input_json = string.Empty;
                    if ("post".Equals(context.HttpContext.Request.Method.ToLower()) && context.HttpContext.Request.ContentType.Contains("json"))
                    {
                        var httpContext = context.HttpContext;
                        var request = httpContext.Request;
                        //request.Body.Position = 0;
                        using StreamReader sr = new StreamReader(request.Body);
                        input_json = await sr.ReadToEndAsync();
                    }
                    else
                        input_json = context.HttpContext.Request.QueryString.ToString();
                    var api = (APIResult)((result.Result as ObjectResult).Value);
                    await WriteLog(context, input_json, api);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "日志拦截异常");
                }
            }
        }

        private async Task WriteLog(ActionExecutingContext context, string input, APIResult result)
        {
            try
            {
                //if (string.IsNullOrEmpty(context.HttpContext.User.Identity.Name))
                //    return;
                var action = (context.ActionDescriptor as ControllerActionDescriptor);
                var desc = action.ControllerTypeInfo.GetCustomAttributes(typeof(DescriptionAttribute), true).Select(f => (DescriptionAttribute)f).FirstOrDefault();
                if (desc == null)
                    return;
                if (!ActionInfo.Exist(action.ActionName))
                    return;
                var model = new SystemLog
                {
                    UserId = context.HttpContext.User.Identity.Name,
                    UserName = context.HttpContext.User.FindFirstValue("UserName"),
                    Controller = action.ControllerName,
                    Action = action.ActionName,
                    ActionName = ActionInfo.GetKeyName(action.ActionName),
                    ActionResult = string.Format("{0}|状态码:{1}", result.Message, result.Code),
                    KeyText = input,
                    ControllerName = desc.Description,
                    IP = context.HttpContext.Connection.RemoteIpAddress.ToString()
                };
       
                await system.Add(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "系统日志记录异常");
            }
        }
    }


    /// <summary>
    /// 功能列表
    /// </summary>
    public class ActionInfo
        {
            private readonly static Dictionary<string, string> actions = new Dictionary<string, string>();
            static ActionInfo()
            {
                actions.Add("Query", "查询");
                actions.Add("Detail", "详情");
                actions.Add("Add", "新增");
                actions.Add("Edit", "编辑");
                actions.Add("Delete", "删除");
                actions.Add("Import", "导入Excel数据");
                actions.Add("UserLogin", "登录");
                actions.Add("Logout", "登出");
            }
            /// <summary>
            /// 是否存在记录功能
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static bool Exist(string key)
            {
                return actions.Any(a => a.Key == key);
            }
            /// <summary>
            /// 获取具体中文描述
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public static string GetKeyName(string key)
            {
                return actions.First(a => a.Key.Equals(key)).Value;
            }
        }
    
}
