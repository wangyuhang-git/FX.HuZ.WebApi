using FX.HuZ.WebApi.Business.EnterpriseApp;
using FX.HuZ.WebApi.Common;
using FX.HuZ.WebApi.Models;
using FX.HuZ.WebApi.Models.EnterpriseApp;
using FX.HuZ.WebApi.Models.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FX.HuZ.WebApi.Controllers.EnterpriseApp
{
    public class ProjectController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult Post([FromBody] dynamic value)
        {
            ResultModel resultMsg = new ResultModel();
            if (null != value)
            {
                JObject @object = JObject.Parse(value.ToString());
                ProjectEpBusiness projectBusiness = new ProjectEpBusiness();
                ProjectPageSearch projectPageSearch = @object.ToObject<ProjectPageSearch>();
                resultMsg = projectBusiness.GetProjectList(projectPageSearch);
            }
            else
            {
                resultMsg.success = false;
                resultMsg.msg = StatusCodeEnum.ParameterError.GetEnumText();
            }
            return Json(resultMsg);
        }

    }
}