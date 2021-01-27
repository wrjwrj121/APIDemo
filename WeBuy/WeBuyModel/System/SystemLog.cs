using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WeBuy.Model.System
{
    [Table("S_SystemLog")]
    public class SystemLog
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 所属企业
        /// </summary>
        public string EnterpriseCode { get; set; }
        /// <summary>
        /// 所属企业
        /// </summary>
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// 方法
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 功能描述
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 法法描述
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 关键参数
        /// </summary>
        public string KeyText { get; set; }
        /// <summary>
        /// 执行结果
        /// </summary>
        public string ActionResult { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        public string IP { get; set; }

    }
}
