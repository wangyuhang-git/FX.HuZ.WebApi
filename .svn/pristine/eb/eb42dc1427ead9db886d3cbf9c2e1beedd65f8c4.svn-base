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
    /// 或者项目基本信息列表
    /// </summary>
    public class ProjectInfoController : ApiController
    {
        ProjectBusiness business = new ProjectBusiness();

        //public HttpResponseMessage Get(string areaName)
        //{
        //    var dt = business.GetProjectTable(areaName);
        //    if (null != dt && dt.Rows.Count > 0)
        //    {
        //        //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式
        //        IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
        //        timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        //        string strJson = JsonConvert.SerializeObject(dt, Formatting.Indented, timeConverter);

        //        HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(strJson, Encoding.GetEncoding("UTF-8"), "application/json") };
        //        return result;
        //    }
        //    else
        //    {
        //        HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent("{\"Msg\":\"没有找到对应区域的项目信息！\"}", Encoding.GetEncoding("UTF-8"), "application/json") };
        //        return result;
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage GetProjectInfoList(string staffId, string areaName)
        {
            ResultMsg resultMsg = null;
            resultMsg = new ResultMsg();
            if (!business.IsStaffid(staffId, areaName))
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = "";
                return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
            }
            var dt = business.GetProjectTable(areaName);
            if (null != dt && dt.Rows.Count > 0)
            {
                resultMsg.StatusCode = (int)StatusCodeEnum.Success;
                resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
                resultMsg.Data = dt;
            }
            else
            {
                resultMsg.StatusCode = (int)StatusCodeEnum.Error;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = dt;
            }
            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }


        [HttpGet]
        public HttpResponseMessage GetProjectInfo(string staffId, string areaName, string constructPermitnum)
        {
            ResultMsg resultMsg = null;
            resultMsg = new ResultMsg();
            if (!business.IsStaffid(staffId, areaName))
            {
                resultMsg = new ResultMsg();
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = "";
                return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
            }
            var dt = business.GetProjectInfo(areaName, constructPermitnum);
            if (null != dt && dt.Rows.Count > 0)
            {
                resultMsg.StatusCode = (int)StatusCodeEnum.Success;
                resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
                resultMsg.Data = dt;
            }
            else
            {
                resultMsg.StatusCode = (int)StatusCodeEnum.Error;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                resultMsg.Data = dt;
            }
            return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
        }
    }
}
