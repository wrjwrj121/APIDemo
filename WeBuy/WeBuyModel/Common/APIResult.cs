using System;
using System.Collections.Generic;
using System.Text;

namespace WeBuyModel.Common
{
    public class APIResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        public string Message { get; set; }

        public void Success(string message ="请求成功") 
        {
            this.Code = 200;
            Message = message;
        }
        public void Fail(string message = "请求失败")
        {
            this.Code = 500;
            Message = message;
        }
    }
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageAPIResult<T> : APIResult where T:class
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public T data { get; set; }

    }
}
