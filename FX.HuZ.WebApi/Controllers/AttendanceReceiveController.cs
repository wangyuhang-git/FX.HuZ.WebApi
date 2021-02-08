using FX.FP.Busines.NewDAL;
using FX.FP.Common.DotNetCode;
using FX.HuZ.WebApi.Business;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FX.HuZ.WebApi.Controllers
{
    /// <summary>
    /// 接收DMP平台推送的考勤数据接口
    /// </summary>
    public class AttendanceReceiveController : ApiController
    {
        //protected Common_Dal comdal = new Common_Dal("AttCloudInstance");
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]JObject value)
        {
            //Helper.SaveLog("AttendanceReceive接收:" + JsonConvert.SerializeObject(value), "AttendanceReceive");
            /*
            Helper.SaveLog("iot_sys_raw:" + value.iot_sys_raw, "AttendanceReceive");
            try
            {
                FX.HuZ.WebApi.Models.Attendance.iot_sys_raw iot_sys_raw1 = JsonConvert.DeserializeObject<FX.HuZ.WebApi.Models.Attendance.iot_sys_raw>(Convert.ToString(value.iot_sys_raw));
                Helper.SaveLog("data:" + JsonConvert.SerializeObject(iot_sys_raw1.data), "AttendanceReceive");

            }
            catch (Exception ex)
            {
                Helper.SaveLog("接收考勤异常:" + ex.Message, "AttendanceReceive");
            }
            return;
            */
            try
            {
                dynamic entity = value;
                if (null != entity)
                {
                    FX.HuZ.WebApi.Models.Attendance.iot_sys_raw iot_sys_raw = JsonConvert.DeserializeObject<FX.HuZ.WebApi.Models.Attendance.iot_sys_raw>(Convert.ToString(entity.iot_sys_raw));

                    FX.HuZ.WebApi.Models.Attendance.data data = iot_sys_raw.data;
                    FX.HuZ.WebApi.Models.Attendance.@params paramsStr = data.paramsValue;

                    string device_id = iot_sys_raw.devId;//设备编号
                    string user_id = paramsStr.user_id;//人员ID
                    byte[] log_image = paramsStr.log_image;//考勤图片（Base64数据）
                    string io_mode = "IN";
                    int receivestatus = 0;
                    FX.HuZ.WebApi.Models.Attendance.Entity.tbl_realtime_glog tbl_realtime_glog = new FX.HuZ.WebApi.Models.Attendance.Entity.tbl_realtime_glog()
                    {
                        device_id = device_id,
                        user_id = user_id,
                        log_image = log_image,
                        io_mode = io_mode,
                        receivestatus = receivestatus
                    };
                    SetData(tbl_realtime_glog);
                }
            }
            catch (Exception ex)
            {
                Helper.SaveLog("接收考勤异常:" + ex.Message, "AttendanceReceive");
            }
        }


        #region 保存数据
        /// <summary>
        /// 保存DMP平台推送过来的考勤数据
        /// </summary>
        /// <param name="tbl_realtime_glog"></param>
        private void SetData(FX.HuZ.WebApi.Models.Attendance.Entity.tbl_realtime_glog tbl_realtime_glog)
        {
            try
            {
                string msDbConn = ConfigurationManager.AppSettings["AttCloudInstance"].ToString();
                string strSql;
                strSql = "INSERT INTO TBL_REALTIME_GLOG";
                strSql = strSql + "(UPDATE_TIME, DEVICE_ID, USER_ID, VERIFY_MODE, IO_MODE, IO_TIME,LOG_IMAGE)";
                strSql = strSql + "VALUES(@update_time,@dev_id, @user_id, @verify_mode, @io_mode, @io_time,@log_image )";

                using (SqlConnection sqlConn = new SqlConnection(msDbConn))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand(strSql, sqlConn);

                    sqlCmd.Parameters.Add("@update_time", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sqlCmd.Parameters.Add("@dev_id", SqlDbType.VarChar).Value = tbl_realtime_glog.device_id;
                    sqlCmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = tbl_realtime_glog.user_id;
                    sqlCmd.Parameters.Add("@verify_mode", SqlDbType.VarChar).Value = "[\"FACE\"]";
                    sqlCmd.Parameters.Add("@io_mode", SqlDbType.VarChar).Value = tbl_realtime_glog.io_mode;
                    sqlCmd.Parameters.Add("@io_time", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    SqlParameter sqlParamLogImg = new SqlParameter("@log_image", SqlDbType.VarBinary);
                    sqlParamLogImg.Direction = ParameterDirection.Input;
                    sqlParamLogImg.Size = tbl_realtime_glog.log_image.Length;
                    sqlParamLogImg.Value = tbl_realtime_glog.log_image;
                    sqlCmd.Parameters.Add(sqlParamLogImg);

                    int flag = -1;
                    //启动考勤入库
                    if (ConfigurationManager.AppSettings["IsUseAttendance"].ToString() == "1")
                    {
                        flag = sqlCmd.ExecuteNonQuery();
                    }
                    Helper.SaveLog("保存考勤数据结果:" + flag, "AttendanceReceive");
                }
            }
            catch (Exception ex)
            {
                Helper.SaveLog("保存考勤数据异常！" + ex.Message, "AttendanceReceive");
            }
        }
        #endregion       
    }
}