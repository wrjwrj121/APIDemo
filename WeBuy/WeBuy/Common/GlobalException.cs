using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeBuy.Common
{
    /// <summary>
    /// 全局未处理异常捕获
    /// </summary>
    public class GlobalException: ExceptionFilterAttribute
    {
        /// <summary>
        /// 当异常发生时会进来
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                Console.WriteLine($"{context.HttpContext.Request.Path}{context.Exception.Message}");

                context.Result = new JsonResult(new
                {
                    Result = false,
                    msg = "发生异常，请联系管理员"
                });

                context.ExceptionHandled = true;
            }
            //base.OnException(context);
        }

        ///async > 上
        //public override Task OnExceptionAsync(ExceptionContext context)
        //{
        //    return base.OnExceptionAsync(context);
        //}
    }
}
