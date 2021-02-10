using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models.EnterpriseApp
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class ProjectPageSearch : PageModel
    {
        /// <summary>
        /// 企业类型
        /// </summary>
        public string AccountType { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string OrganizationCode { get; set; }
    }
}