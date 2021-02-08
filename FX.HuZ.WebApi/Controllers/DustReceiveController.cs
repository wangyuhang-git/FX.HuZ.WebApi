﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using FX.FP.Busines.NewDAL;
using FX.FP.Common.DotNetExt;
using FX.HuZ.WebApi.Business;
using FX.HuZ.WebApi.Models.Dust;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FX.HuZ.WebApi.Controllers
{
    /// <summary>
    /// 接收DMP（老）平台推送扬尘数据接口
    /// </summary>
    public class DustReceiveController : ApiController
    {
        protected Common_Dal comdal = new Common_Dal();
        #region 方式一
        /// <summary>
        /// HttpClient方式掉用
        /// </summary>
        /// <param name="jobject"></param>
        [HttpPost]
        public IHttpActionResult Post([FromBody] JObject jobject)
        {
            string message = "接收数据成功";
            dynamic entity = jobject;
            string paramstr = string.Empty;
            try
            {
                //Helper.SaveLog("cmp接收:" + JsonConvert.SerializeObject(entity), "DustReceive全部");
                if (entity != null)
                {
                    /*
                    string sign = entity.sign;
                    string address = entity.address;
                    string longitude = entity.longitude;
                    string latitude = entity.latitude;
                    string devName = entity.devName;
                    string deviceId = entity.deviceId;
                    string prodcutId = entity.prodcutId;
                    string appId = entity.appId;
                    string deveui = entity.deveui;
                    string uploadTime = entity.uploadTime;
                    string payload = entity.payload;
                    */
                    paramstr = JsonConvert.SerializeObject(entity);
                    //Stopwatch stopwatch = new Stopwatch();
                    //stopwatch.Start();
                    SetData(paramstr);
                    //stopwatch.Stop();
                    //Helper.SaveLog("老dmp数据保存时长:" + stopwatch.ElapsedMilliseconds + "毫秒", "DustReceive");
                }
            }
            catch (Exception ex)
            {
                message = "接收数据失败：" + ex.Message;
                Helper.SaveLog("dmp参数：" + paramstr + ",cmp接收异常:" + ex.Message, "DustReceive");
            }
            if (!string.IsNullOrEmpty(message))
            {
                var result = new
                {
                    result = "02",
                    resultmsg = message
                };
                return Json(result);
            }
            else
            {
                var result = new
                {
                    result = "01",
                    resultmsg = message
                };
                return Json(result);
            }
        }
        #endregion


        #region 方式二
        /*
        /// <summary>
        /// HttpWebRequest方式调用
        /// </summary>
        /// <param name="entity"></param>
        [HttpPost]
        public IHttpActionResult Post(dynamic entity)
        {
            string message = "接收数据成功";
            try
            {
                Helper.SaveLog("cmp接收:" + JsonConvert.SerializeObject(entity),"DustReceive");
                if (entity != null)
                {
                    string sign = entity.sign;
                    string address = entity.address;
                    string longitude = entity.longitude;
                    string latitude = entity.latitude;
                    string devName = entity.devName;
                    string deviceId = entity.deviceId;
                    string prodcutId = entity.prodcutId;
                    string appId = entity.appId;
                    string deveui = entity.deveui;
                    string uploadTime = entity.uploadTime;
                    string payload = entity.payload;
                }
            }
            catch (Exception ex)
            {
                message = "接收数据失败：" + ex.Message;
                Helper.SaveLog("cmp接收异常:" + ex.Message,"DustReceive");
            }
            if (!string.IsNullOrEmpty(message))
            {
                var result = new
                {
                    result = "02",
                    resultmsg = message
                };
                return Json(result);
            }
            else
            {
                var result = new
                {
                    result = "01",
                    resultmsg = message
                };
                return Json(result);
            }
        }
        */
        #endregion

        #region 保存数据
        private void SetData(string strData)
        {
            string deviceId = string.Empty;//设备编码
            try
            {
                strData = "[" + strData + "]";
                Hashtable ht = new Hashtable();
                List<DataBaseModel> baseModel = JsonConvert.DeserializeObject<List<DataBaseModel>>(strData);
                string payload = "[" + baseModel[0].payload + "]";
                List<LoadDataModel> dataModel = JsonConvert.DeserializeObject<List<LoadDataModel>>(payload);
                deviceId = string.IsNullOrEmpty(baseModel[0].deveui) ? baseModel[0].deviceId : baseModel[0].deveui;
                if (string.IsNullOrEmpty(deviceId))
                {
                    //deviceId = baseModel[0].deviceId;
                    return;//设备编号为空直接返回
                }
                List<NodeListDataModel> listData = dataModel[0].nodelist;
                if (null != listData && listData.Count > 0)
                {
                    //mondify on 20191225 如该设备十分钟内有数据的就不在接收
                    object hasData = comdal.ExecuteScalar(string.Format(@" select guid from LTDQ_LiveData where DeviceSN='{0}' and CreateDate > dateadd(minute,-10,GETDATE())", deviceId));
                    if (null == hasData || Convert.IsDBNull(hasData))
                    {
                        //mondify on 20200107 对神威科技,杭州豪测测绘仪器有限公司, 安吉先锋三家厂商的pm25和pm10做*10
                        double pm25, pm10;
                        double.TryParse(listData[0].hum, out pm25);
                        double.TryParse(listData[0].tem, out pm10);
                        hasData = comdal.ExecuteScalar(string.Format(@"select  Manufacturer from LTDQ_LianTongBook where DeviceSN='{0}'", deviceId));
                        string[] ManufacturerList = new string[] { "神威科技", "杭州豪测测绘仪器有限公司", "安吉先锋" };
                        if (null != hasData)
                        {
                            if (ManufacturerList.Contains(hasData.ToString2()))
                            {
                                pm25 = pm25 * 10;
                                pm10 = pm10 * 10;
                            }
                        }
                        //20200827增加判断上传时间和当前时间相比相差30分钟以上
                        if (DateTime.Now.AddMinutes(-30) > DateTime.Parse(baseModel[0].uploadTime))
                        {
                            //查询设备是否已记录
                            DataTable dtde = comdal.GetDataTableBySQL(new StringBuilder(string.Format(@"SELECT * FROM LTDQ_AbnormalDeviceData WHERE DeviceSN='{0}'", deviceId)));
                            if (dtde == null || dtde.Rows.Count < 1)
                            {
                                Hashtable htde = new Hashtable();
                                htde.Add("GUID", Guid.NewGuid().ToString());
                                htde.Add("DeviceSN", deviceId);
                                htde.Add("CreateDate", baseModel[0].uploadTime);
                                htde.Add("pm25", pm25);
                                htde.Add("pm10", pm10);
                                htde.Add("pd04", listData[2].tem);//温度
                                htde.Add("pd05", listData[2].hum);//湿度
                                htde.Add("pd09", listData[1].hum);//噪声
                                htde.Add("LTcontent", "chocking");
                                htde.Add("ProjectGUID", "--");
                                bool returnValue = comdal.SubmitData("LTDQ_AbnormalDeviceData", "GUID", string.Empty, htde);
                                if (!returnValue)
                                {
                                    Helper.SaveLog(deviceId + "设备插入异常设备数据表失败！插入原因：上传时间和当前时间相差大于30分钟！", "DustReceive");
                                }
                            }
                            else
                            {
                                Hashtable htde = new Hashtable();
                                htde.Add("GUID", dtde.Rows[0]["GUID"].ToString());
                                htde.Add("DeviceSN", deviceId);
                                htde.Add("CreateDate", baseModel[0].uploadTime);
                                htde.Add("pm25", pm25);
                                htde.Add("pm10", pm10);
                                htde.Add("pd04", listData[2].tem);//温度
                                htde.Add("pd05", listData[2].hum);//湿度
                                htde.Add("pd09", listData[1].hum);//噪声
                                htde.Add("LTcontent", "chocking");
                                htde.Add("ProjectGUID", "--");
                                bool returnValue = comdal.SubmitData("LTDQ_AbnormalDeviceData", "GUID", dtde.Rows[0]["GUID"].ToString(), htde);
                                if (!returnValue)
                                {
                                    Helper.SaveLog(deviceId + "设备更新异常设备数据表失败！更新原因：上传时间和当前时间相差大于30分钟！", "DustReceive");
                                }
                            }
                        }
                        //市本级
                        DataTable dtSNSBJ = comdal.GetDataTableBySQL(new StringBuilder(string.Format(@"SELECT * FROM LTDQ_Device WHERE DeviceSN='{0}'", deviceId)));
                        if (dtSNSBJ != null && dtSNSBJ.Rows.Count > 0)
                        {
                            ht.Add("GUID", Guid.NewGuid().ToString());
                            ht.Add("DeviceSN", deviceId);
                            ht.Add("CreateDate", baseModel[0].uploadTime);
                            ht.Add("pm25", pm25);
                            ht.Add("pm10", pm10);
                            ht.Add("pd04", listData[2].tem);//温度
                            ht.Add("pd05", listData[2].hum);//湿度
                            ht.Add("pd09", listData[1].hum);//噪声
                            ht.Add("LTcontent", "chocking");
                            ht.Add("ProjectGUID", dtSNSBJ.Rows[0]["ProjectGUID"]);
                            bool returnValue = comdal.SubmitData("LTDQ_HistoryData", "GUID", string.Empty, ht);
                            if (returnValue)
                            {
                                DataTable dt = comdal.GetDataTableBySQL(new StringBuilder(string.Format(@" select * from LTDQ_LiveData where DeviceSN='{0}' ", deviceId)));
                                string guid = string.Empty;
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    ht["GUID"] = dt.Rows[0]["GUID"].ToString2();
                                    guid = dt.Rows[0]["GUID"].ToString2();
                                }
                                else
                                {
                                    ht["GUID"] = Guid.NewGuid().ToString2();
                                }
                                comdal.SubmitData("LTDQ_LiveData", "GUID", guid, ht);
                                if (ht["pm10"].ToDouble() > Double.Parse(ConfigurationManager.AppSettings["MaxPmValue"].ToString().Trim()) || ht["pm25"].ToDouble() >= Double.Parse(ConfigurationManager.AppSettings["MaxPm25Value"].ToString().Trim()))
                                {
                                    ht["GUID"] = Guid.NewGuid().ToString2();
                                    bool flag = comdal.SubmitData("LTDQ_Warning", "GUID", "", ht);
                                }

                                //更改设备为已接入
                                if (dtSNSBJ.Rows[0]["ISBIND"] == DBNull.Value || dtSNSBJ.Rows[0]["ISBIND"].ToString2() == "0")
                                {
                                    comdal.ExecuteBySql(new StringBuilder(string.Format("update LTDQ_Device set IsBind=1 where DeviceSN='{0}'", deviceId)));
                                }
                            }
                            else
                            {
                                Helper.SaveLog(deviceId + "设备未绑定项目！", "DustReceive");
                            }
                        }
                        else
                        {

                            DataTable dtSN = comdal.GetDataTableBySQL(new StringBuilder(string.Format(@"SELECT * FROM LTDQ_DeviceZT WHERE DeviceSN='{0}'", deviceId)));
                            if (dtSN == null || dtSN.Rows.Count == 0)
                            {
                                ht.Add("GUID", Guid.NewGuid().ToString());
                                ht.Add("DeviceSN", deviceId);
                                ht.Add("CreateDate", baseModel[0].uploadTime);
                                ht.Add("pm25", pm25);
                                ht.Add("pm10", pm10);
                                ht.Add("pd04", listData[2].tem);//温度
                                ht.Add("pd05", listData[2].hum);//湿度
                                ht.Add("pd09", listData[1].hum);//噪声
                                ht.Add("LTcontent", "chocking");
                                ht.Add("ProjectGUID", "-");
                                bool returnValue = comdal.SubmitData("LTDQ_HistoryData", "GUID", string.Empty, ht);
                                if (returnValue)
                                {
                                    DataTable dt = comdal.GetDataTableBySQL(new StringBuilder(string.Format(@" select * from LTDQ_LiveData where DeviceSN='{0}' ", deviceId)));
                                    string guid = string.Empty;
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        ht["GUID"] = dt.Rows[0]["GUID"].ToString2();
                                        guid = dt.Rows[0]["GUID"].ToString2();
                                    }
                                    else
                                    {
                                        ht["GUID"] = Guid.NewGuid().ToString2();
                                    }
                                    comdal.SubmitData("LTDQ_LiveData", "GUID", guid, ht);
                                    if (ht["pm10"].ToDouble() >= Double.Parse(ConfigurationManager.AppSettings["MaxPmValue"].ToString().Trim()) || ht["pm25"].ToDouble() >= Double.Parse(ConfigurationManager.AppSettings["MaxPm25Value"].ToString().Trim()))
                                    {
                                        ht["GUID"] = Guid.NewGuid().ToString2();
                                        bool flag = comdal.SubmitData("LTDQ_Warning", "GUID", "", ht);
                                    }
                                    #region 保存区县项目数据
                                    Hashtable htTz = new Hashtable();
                                    htTz.Add("GUID", Guid.NewGuid().ToString());
                                    htTz.Add("DeviceName", baseModel[0].devName);
                                    htTz.Add("DeviceSN", deviceId);
                                    htTz.Add("DeviceAddress", baseModel[0].address);
                                    htTz.Add("ProjectName", baseModel[0].devName);
                                    htTz.Add("Coordinate", dataModel[0].lng + "," + dataModel[0].lat);
                                    htTz.Add("RegisterPerson", "cmp平台");
                                    htTz.Add("ProjectDate", baseModel[0].uploadTime);
                                    htTz.Add("ProjectArea", "---");
                                    bool result = comdal.SubmitData("LTDQ_DeviceZT", "GUID", string.Empty, htTz);
                                    if (result)
                                    {
                                        //Helper.SaveLog("保存区县项目数据成功","DustReceive");
                                    }
                                    else
                                    {
                                        Helper.SaveLog(deviceId + "保存区县项目数据失败", "DustReceive");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    Helper.SaveLog(deviceId + "设备未绑定项目！", "DustReceive");
                                }
                            }
                            else
                            {
                                ht.Add("GUID", Guid.NewGuid().ToString());
                                ht.Add("DeviceSN", deviceId);
                                ht.Add("CreateDate", baseModel[0].uploadTime);
                                ht.Add("pm25", pm25);
                                ht.Add("pm10", pm10);
                                ht.Add("pd04", listData[2].tem);
                                ht.Add("pd05", listData[2].hum);
                                ht.Add("pd09", listData[1].hum);
                                ht.Add("LTcontent", "chocking");
                                ht.Add("ProjectGUID", dtSN.Rows[0]["GUID"].ToString());
                                bool returnValue = comdal.SubmitData("LTDQ_HistoryData", "GUID", string.Empty, ht);
                                if (returnValue)
                                {
                                    DataTable dt = comdal.GetDataTableBySQL(new StringBuilder(string.Format(@" select * from LTDQ_LiveData where DeviceSN='{0}' ", deviceId)));
                                    string guid = string.Empty;
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        ht["GUID"] = dt.Rows[0]["GUID"].ToString2();
                                        guid = dt.Rows[0]["GUID"].ToString2();
                                    }
                                    else
                                    {
                                        ht["GUID"] = Guid.NewGuid().ToString2();
                                    }
                                    comdal.SubmitData("LTDQ_LiveData", "GUID", guid, ht);
                                    if (ht["pm10"].ToDouble() > Double.Parse(ConfigurationManager.AppSettings["MaxPmValue"].ToString().Trim()) || ht["pm25"].ToDouble() >= Double.Parse(ConfigurationManager.AppSettings["MaxPm25Value"].ToString().Trim()))
                                    {
                                        ht["GUID"] = Guid.NewGuid().ToString2();
                                        bool flag = comdal.SubmitData("LTDQ_Warning", "GUID", "", ht);
                                    }
                                    #region 保存区县项目数据
                                    Hashtable htTz = new Hashtable();
                                    htTz.Add("GUID", dtSN.Rows[0]["GUID"].ToString());
                                    htTz.Add("DeviceName", baseModel[0].devName);
                                    //htTz.Add("DeviceSN", deviceId);
                                    htTz.Add("DeviceAddress", baseModel[0].address);
                                    htTz.Add("ProjectName", baseModel[0].devName);
                                    //htTz.Add("Coordinate", dataModel[0].lng + "," + dataModel[0].lat);
                                    htTz.Add("RegisterPerson", "cmp平台");
                                    htTz.Add("ProjectDate", baseModel[0].uploadTime);
                                    htTz.Add("IsBind", 1);
                                    //htTz.Add("ProjectArea", "---");
                                    bool result = comdal.SubmitData("LTDQ_DeviceZT", "GUID", htTz["GUID"].ToString(), htTz);
                                    if (result)
                                    {
                                        //Helper.SaveLog("更新区县项目数据成功","DustReceive");
                                    }
                                    else
                                    {
                                        Helper.SaveLog(deviceId + "更新区县项目数据失败", "DustReceive");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    Helper.SaveLog(deviceId + "设备未绑定项目！", "DustReceive");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Helper.SaveLog("Dmp参数：" + strData + ",保存数据异常！" + ex.Message, "DustReceive异常");
                Helper.SaveLog("保存数据异常：" + ex.Message, "DustReceive异常");
            }
        }
        #endregion
    }
}