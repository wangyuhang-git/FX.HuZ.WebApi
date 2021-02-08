using FX.HuZ.WebApi.Business;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FX.HuZ.WebApi.Controllers
{
    public class DustReceiveByFromController : ApiController
    {
        // POST api/<controller>
        public void Post([FromBody]JObject value)
        {
            Helper.SaveLog("DustReceiveByFrom_post接收:" + value);
        }
    }
}