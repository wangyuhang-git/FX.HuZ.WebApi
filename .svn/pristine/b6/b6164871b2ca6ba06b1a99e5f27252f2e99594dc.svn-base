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
    /// <summary>
    /// 接收DMP平台推送的塔吊黑匣子数据接口
    /// </summary>
    public class TowerCraneReceiveController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post([FromBody]JObject value)
        {
            Helper.SaveLog("TowerCraneReceive:" + JsonConvert.SerializeObject(value), "TowerCraneReceive");
            try
            {
                dynamic entity = value;
                if (null != entity)
                {
                    //反序列化有以下两种方式
                    #region 方式一
                    JObject raw = entity.iot_sys_raw;
                    var rawModel = raw.ToObject<FX.HuZ.WebApi.Models.TowerCrane.iot_sys_raw>();
                    if (null != rawModel)
                    {
                        FX.HuZ.WebApi.Models.TowerCrane.data data = rawModel.data;
                        FX.HuZ.WebApi.Models.TowerCrane.@params paramsStr = data.paramsValue;
                    }
                    #endregion

                    #region 方式二
                    FX.HuZ.WebApi.Models.TowerCrane.iot_sys_raw iot_sys_raw = JsonConvert.DeserializeObject<FX.HuZ.WebApi.Models.TowerCrane.iot_sys_raw>(Convert.ToString(entity.iot_sys_raw));
                    if (null != iot_sys_raw)
                    {
                        FX.HuZ.WebApi.Models.TowerCrane.data data = iot_sys_raw.data;
                        FX.HuZ.WebApi.Models.TowerCrane.@params paramsStr = data.paramsValue;
                    }
                    #endregion


                    //SetData();

                    var result = new
                    {
                        code = "01",
                        msg = "调用接口成功"
                    };
                    return Json(result);
                }
                else
                {
                    var result = new
                    {
                        code = "03",
                        msg = "调用接口失败，原因：获取参数为空！"
                    };
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                Helper.SaveLog("接收塔吊黑匣子数据异常:" + ex.Message, "TowerCraneReceive");
                var result = new
                {
                    code = "02",
                    msg = "调用接口失败，原因：" + ex.Message + ex.StackTrace
                };
                return Json(result);
            }
        }

        #region 保存数据
        /// <summary>
        /// 保存DMP平台推送过来的黑匣子数据
        /// </summary>
        /// <param name="entity"></param>
        private void SetData(dynamic entity)
        {

        }
        #endregion
    }
}
