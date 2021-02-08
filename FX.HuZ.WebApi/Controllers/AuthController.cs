using FX.HuZ.WebApi.Business;
using FX.HuZ.WebApi.Common;
using FX.HuZ.WebApi.Models;
using FX.HuZ.WebApi.Models.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FX.HuZ.WebApi.Controllers
{
    public class AuthController : ApiController
    {
        /// <summary>
        /// 根据用户名获取token
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetToken(string staffId, string areaName)
        {
            ResultMsg resultMsg = null;
            ProjectBusiness projectBusiness = new ProjectBusiness();
            //判断参数是否合法
            if (!projectBusiness.IsStaffid(staffId, areaName))
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = "";
                return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
            }

            //插入缓存
            Token token = (Token)HttpRuntime.Cache.Get(staffId);
            if (HttpRuntime.Cache.Get(staffId) == null)
            {
                token = new Token();
                token.StaffId = staffId;
                token.SignToken = Guid.NewGuid();
                token.ExpireTime = DateTime.Now.AddMinutes(120);
                HttpRuntime.Cache.Insert(token.StaffId, token, null, token.ExpireTime, TimeSpan.Zero);
            }

            //返回token信息
            resultMsg = new ResultMsg();
            resultMsg.StatusCode = (int)StatusCodeEnum.Success;
            resultMsg.Info = "";
            resultMsg.Data = token;

            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }
    }
}