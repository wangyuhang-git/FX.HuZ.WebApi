﻿using FX.HuZ.WebApi.Business.EnterpriseApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FX.HuZ.WebApi.Models;
using FX.HuZ.WebApi.Models.Enum;
using FX.HuZ.WebApi.Common;
using Newtonsoft.Json.Linq;

namespace FX.HuZ.WebApi.Controllers.EnterpriseApp
{
    /// <summary>
    /// 用户登录控制器
    /// </summary>
    public class UserLoginController : ApiController
    {
        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult Post([FromBody] JObject value)
        {
            ResultMsg resultMsg = new ResultMsg();
            if (null != value)
            {
                UserInfoEpBusiness userInfoEpBusiness = new UserInfoEpBusiness();
                dynamic @object = value;
                string userName = @object.userName;
                string userPwd = @object.userPwd;
                resultMsg = userInfoEpBusiness.Login(userName, userPwd);
            }
            else
            {
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
            }
            return Json(resultMsg);
        }
    }
}