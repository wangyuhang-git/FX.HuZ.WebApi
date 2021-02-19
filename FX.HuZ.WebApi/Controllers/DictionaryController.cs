using FX.HuZ.WebApi.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FX.HuZ.WebApi.Controllers
{
    public class DictionaryController : ApiController
    {
        DictionaryBusiness dictionaryBusiness = new DictionaryBusiness();

        /// <summary>
        /// 根据字典值获取字典数据列表
        /// </summary>
        /// <param name="dictionaryCode"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody] dynamic value)
        {
            DataTable dt = new DataTable("DictionaryList");
            if (!string.IsNullOrEmpty(value.dictionaryCode.ToString()))
            {
                dt = dictionaryBusiness.GetDictionaryList(value.dictionaryCode.ToString());
            }
            else
            {
                throw new ArgumentNullException("参数不能为空！");
            }
            return Json(dt);
        }
    }
}
