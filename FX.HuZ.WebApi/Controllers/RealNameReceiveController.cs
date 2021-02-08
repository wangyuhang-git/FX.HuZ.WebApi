using FX.FP.Busines.NewDAL;
using FX.FP.Common.DotNetCode;
using FX.FP.Common.DotNetExt;
using FX.HuZ.WebApi.Business;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace FX.HuZ.WebApi.Controllers
{
    /// <summary>
    /// 接收DMP平台推送的实名制数据接口
    /// </summary>
    public class RealNameReceiveController : ApiController
    {
        protected Common_Dal comdal = new Common_Dal("SqlServer_FP_DB_SMZ");
        [HttpPost]
        public IHttpActionResult Post([FromBody] JObject value)
        {
            //Helper.SaveLog("RealNameReceive:" + JsonConvert.SerializeObject(value), "RealNameReceive");
            Hashtable ht = new Hashtable();
            try
            {
                dynamic entity = value;
                if (null != entity)
                {
                    /*
                    JObject raw = entity.iot_sys_raw;
                    var rawModel = raw.ToObject<FX.HuZ.WebApi.Models.RealName.iot_sys_raw>();
                    if (null != rawModel)
                    {
                        FX.HuZ.WebApi.Models.RealName.data data = rawModel.data;
                        FX.HuZ.WebApi.Models.RealName.@params paramsStr = data.paramsValue;
                        //SetData(paramsStr);
                    }
                    */

                    FX.HuZ.WebApi.Models.RealName.iot_sys_raw iot_sys_raw = JsonConvert.DeserializeObject<FX.HuZ.WebApi.Models.RealName.iot_sys_raw>(Convert.ToString(entity.iot_sys_raw));
                    if (null != iot_sys_raw)
                    {
                        FX.HuZ.WebApi.Models.RealName.data data = iot_sys_raw.data;
                        FX.HuZ.WebApi.Models.RealName.@params paramsStr = data.paramsValue;
                        SetData(paramsStr);
                    }
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
                Helper.SaveLog("接收实名制考勤数据异常:" + ex.Message, "RealNameReceive");
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
        /// 保存DMP平台推送实名制考勤数据
        /// </summary>
        /// <param name="params"></param>
        private void SetData(FX.HuZ.WebApi.Models.RealName.@params @params)
        {
            string syncData = @params.syncData;
            string guid = Guid.NewGuid().ToString2();
            //if (@params.ProjectGUID == "a29c8ca2-aec0-44b3-820e-e44da159aca3")
            //{
            //    Helper.SaveLog("@params.syncData:" + syncData, "RealNameReceive");
            //}
            //Helper.SaveLog("@请求id:" + guid + "开始时间：" + DateTime.Now, "RealNameReceiveTime");
            var list = JsonConvert.DeserializeObject<List<FX.HuZ.WebApi.Models.RealName.T_AttendanceInfo>>(Convert.ToString(syncData));
            var dtAtt = comdal.GetDataTablePrimaryNameBySQL(new StringBuilder(" SELECT * FROM dbo.T_AttendanceData WHERE 1=2 "));
            if (null != list && list.Count > 0)
            {
                try
                {
                    //Helper.SaveLog("@请求id:" + guid + "list.Count：" + list.Count, "RealNameReceiveTime");
                    //Helper.SaveLog("@项目ID:" + @params.ProjectGUID + "，list.Count：" + list.Count, "RealNameReceiveTime");
                    DataRow dr = null;
                    string attendancetime = string.Empty;
                    DateTime _attendanceTime;//考勤时间
                    foreach (Models.RealName.T_AttendanceInfo item in list)
                    {
                        #region
                        /* Hashtable ht = new Hashtable();
                         ht["GUID"] = Guid.NewGuid().ToString();
                         ht["KeyID"] = item.KEYID;
                         string personGuid = this.GetPersonGUID(item.PERSONKEYID);
                         ht["PersonGUID"] = string.IsNullOrEmpty(personGuid) ? item.PERSONKEYID : personGuid;
                         ht["ProjectGUID"] = @params.ProjectGUID;
                         ht["AttendanceTime"] = item.ATTENDANCETIME;
                         ht["AttendanceDoorType"] = item.ATTENDANCEDOORTYPE;
                         ht["ImageBuffer"] = item.IMAGEBUFFER;
                         ht["CreateTime"] = System.DateTime.Now;
                         bool returnValue = comdal.SubmitData("T_AttendanceInfo", "GUID", "", ht);
                         if (returnValue)
                         {
                             //Helper.SaveLog("写入实名制考勤数据成功，人员GUID：" + item.PERSONKEYID + "项目GUID：" + @params.ProjectGUID, "RealNameReceive");
                             OpAttInfo(ht["GUID"].ToString(), @params.ProjectGUID);
                         }
                         else
                         {
                             Helper.SaveLog("写入实名制考勤数据失败，人员GUID：" + item.PERSONKEYID + "项目GUID：" + @params.ProjectGUID, "RealNameReceive");
                         }*/
                        #endregion
                        dr = dtAtt.NewRow();
                        dr["GUID"] = Guid.NewGuid().ToString();
                        dr["KeyID"] = item.KEYID;
                        string personGuid = this.GetPersonGUID(item.PERSONKEYID, @params.ProjectGUID);
                        personGuid = string.IsNullOrEmpty(personGuid) ? item.PERSONKEYID : personGuid;
                        if (personGuid.Length != 36)//非法人员
                        {
                            continue;
                        }
                        dr["PersonGUID"] = personGuid;
                        dr["ProjectGUID"] = @params.ProjectGUID;
                        attendancetime = item.ATTENDANCETIME;
                        if (DateTime.TryParse(attendancetime, out _attendanceTime))
                        {
                            if (_attendanceTime.AddMonths(1) < DateTime.Now)//考勤时间超过一个月的数据不在接收
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        dr["AttendanceTime"] = item.ATTENDANCETIME;
                        dr["AttendanceDoorType"] = item.ATTENDANCEDOORTYPE;
                        dr["ImageBuffer"] = item.IMAGEBUFFER;
                        dr["CreateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dr["ProcessStatus"] = 0;
                        dtAtt.Rows.Add(dr);
                    }
                    dtAtt.TableName = "T_AttendanceData";//新的记录数据表，考勤数据全部存储
                    bool flag = comdal.SqlBulkCopyImport(dtAtt);

                    if (!flag)
                    {
                        //OpAttInfo(dtAtt, @params.ProjectGUID);
                        Helper.SaveLog("批量写入实名制考勤数据失败，项目ID：" + @params.ProjectGUID, "RealNameReceive");
                    }
                }
                catch (Exception ex)
                {
                    Helper.SaveLog("写入实名制考勤数据失败，原因：" + ex.Message + ",syncData：" + syncData, "RealNameReceive");
                    throw new ApplicationException(ex.Message);
                }
            }
            else
            {
                Helper.SaveLog("写入实名制考勤数据失败，原因：未获取到详细的考勤信息，请检查传输的参数", "RealNameReceive");
                throw new ApplicationException("未获取到详细的考勤信息参数，请检查传输的参数");
            }
            //Helper.SaveLog("@请求id:" + guid + "结束时间：" + DateTime.Now, "RealNameReceiveTime");
        }
        #endregion

        private string GetPersonGUID(string keyID, string projectGuid)
        {
            string personGuid = string.Empty;
            DataTable dt = comdal.GetDataTableBySQL(new StringBuilder(string.Format(@"SELECT * FROM T_PERSONINFO WHERE KEYID='{0}' AND PROJECTGUID='{1}'", keyID, projectGuid)));
            if (dt != null && dt.Rows.Count > 0)
            {
                personGuid = Convert.ToString(dt.Rows[0]["GUID"]);
            }
            return personGuid;
        }


    }
}