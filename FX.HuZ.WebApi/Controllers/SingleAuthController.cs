using FX.FP.Busines.NewDAL;
using FX.HuZ.WebApi.Business;
using FX.HuZ.WebApi.Common;
using FX.HuZ.WebApi.Models;
using FX.HuZ.WebApi.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
namespace FX.HuZ.WebApi.Controllers
{
    /// <summary>
    /// 标点信息系统-单点登录验证码获取接口
    /// </summary>
    public class SingleAuthController : ApiController
    {
        UserInfoBusiness business = new UserInfoBusiness();
        /// <summary>
        /// 标点系统-登录验证码查询用户
        /// </summary>
        /// <param name="verCode">动态验证码</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(string verCode)
        {

            ResultMsg resultMsg = new ResultMsg();
            //判断参数是否合法
            if (string.IsNullOrEmpty(verCode))
            {
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = null;
                return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
            }

            var dt = business.GetUserInfoTable(verCode);
            //查询成功且有数据时
            if (null != dt && dt.Rows.Count == 1)
            {
                resultMsg.StatusCode = (int)StatusCodeEnum.Success;
                resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
                resultMsg.Data = dt;
            }
            //查询无数据时
            else if (null != dt && dt.Rows.Count == 0)
            {
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = null;
            }
            //其他系统异常时
            else
            {
                resultMsg.StatusCode = (int)StatusCodeEnum.Error;
                resultMsg.Info = StatusCodeEnum.Error.GetEnumText();
                resultMsg.Data = null;
            }
            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }

    }
}