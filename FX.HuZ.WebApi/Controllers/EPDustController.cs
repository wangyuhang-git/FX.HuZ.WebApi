using FX.FP.Busines.NewDAL;
using Newtonsoft.Json.Converters;
using System;
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
    /// 扬尘相关数据环保局对接
    /// </summary>
    public class EPDustController : ApiController
    {
        protected Common_Dal comdal = new Common_Dal("SqlServer_FP_DB");
        // GET api/<controller>
        [HttpGet]
        [Route("api/EPDust/ProjectList")]
        public IHttpActionResult GetProjectList()
        {
            string sql = string.Format(@"SELECT d.DeviceSN SBBH,P.SegmentName XXMC,P.SegmentAddress XMDZ,P.ProjectArea SSQX,P.Coordinate JD,'' WD,
            P.ProjectLinkMan SGFLXR,p.ProjectLinkPhone SGFLXRDH,p.BuildUnitPrincipal JSFLXR,P.BuildUnitPrincipalTel JSFLXRDH FROM View_LTDQ_Device d inner JOIN NT_Project p on d.ProjectGUID=p.GUID 
            WHERE p.ProjectStatus ='01'");
            DataTable table = comdal.GetDataTableBySQL(new StringBuilder(sql));//在建项目

            if (null != table && table.Rows.Count > 0)
            {
                string wd;
                foreach (DataRow row in table.Rows)
                {
                    row["JD"] = this.GetJW(Convert.ToString(row["JD"]), out wd);
                    row["WD"] = wd;
                }
            }
            else
            {
                return NotFound();
            }
            return Json(table);
        }
        /// <summary>
        /// 获取设备一小时平均值，无时间段参数默认查询当天数据
        /// </summary>
        /// <param name="type">监测因子，必填</param>
        /// <param name="deviceSN">设备编码，选填</param>
        /// <param name="startTime">开始时间，选填</param>
        /// <param name="endTime">结束时间，选填</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/EPDust/DustAvgHour/{type}")]
        public IHttpActionResult GetDustAvgHour(string type, string deviceSN = "", string startTime = "", string endTime = "")
        {
            StringBuilder sql = new StringBuilder(2000);
            sql.AppendFormat(@"select ld.guid XH, t.DeviceSN SBBH, CreateDate JCSJ, '{0}' JCYZ, {0} JCZ from
            (select d.DeviceName, p.SegmentName, d.ProjectGUID dprojectguid, p.GUID PGUID, p.SegmentAddressArea as ProjectArea, d.DeviceSN, p.ConstructUnitName, BuildUnitName
            from View_LTDQ_Device d  left join NT_Project p on d.ProjectGUID = p.GUID where p.ProjectStatus = '01' and d.ProjectGUID is not null
            ) t inner join LTDQ_ExcessiveStatisticalData ld on ld.DeviceSN = t.DeviceSN where 1=1 ", type);
            if (!string.IsNullOrEmpty(startTime))
            {
                sql.AppendFormat(@" and CreateDate >= '{0}'", Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH" + ":00:00"));
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                sql.AppendFormat(@" and CreateDate <= '{0}'", Convert.ToDateTime(endTime).ToString("yyyy-MM-dd HH" + ":59:59"));
            }
            if (string.IsNullOrEmpty(startTime) && string.IsNullOrEmpty(endTime))
            {
                sql.AppendFormat(@" and CreateDate >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
                sql.AppendFormat(@" and CreateDate <= '{0}'", DateTime.Now.AddDays(1));
            }
            if (!string.IsNullOrEmpty(deviceSN))
            {
                sql.AppendFormat(@" AND ld.DeviceSN='{0}'", deviceSN);
            }
            sql.Append(" ORDER BY ld.DeviceSN,ld.CreateDate");
            DataTable table = comdal.GetDataTableBySQL(sql);

            if (null != table && table.Rows.Count > 0)
            {
                return Json(table,GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
            }
            else
            {
                return NotFound();
            }
        }


        /// <summary>
        /// 获取设备每天平均值，无时间段参数默认查询当天数据
        /// </summary>
        /// <param name="type">监测因子，必填</param>
        /// <param name="deviceSN">设备编码，选填</param>
        /// <param name="startTime">开始时间，选填</param>
        /// <param name="endTime">结束时间，选填</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/EPDust/DustAvgDay/{type}")]
        public IHttpActionResult GetDustAvgDay(string type, string deviceSN = "", string startTime = "", string endTime = "")
        {
            StringBuilder sql = new StringBuilder(2000);
            sql.AppendFormat(@"select ld.guid XH, t.DeviceSN SBBH, CreateDate JCSJ, '{0}' JCYZ, {0} JCZ from
            (select d.DeviceName, p.SegmentName, d.ProjectGUID dprojectguid, p.GUID PGUID, p.SegmentAddressArea as ProjectArea, d.DeviceSN, p.ConstructUnitName, BuildUnitName
            from View_LTDQ_Device d  left join NT_Project p on d.ProjectGUID = p.GUID where p.ProjectStatus = '01' and d.ProjectGUID is not null
            ) t inner join LTDQ_StatisticalByDayData ld on ld.DeviceSN = t.DeviceSN where 1=1 ", type);
            if (!string.IsNullOrEmpty(startTime))
            {
                sql.AppendFormat(@" and CreateDate >= '{0}'", Convert.ToDateTime(startTime));
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                sql.AppendFormat(@" and CreateDate <= '{0}'", Convert.ToDateTime(endTime).AddDays(1));
            }
            if (string.IsNullOrEmpty(startTime) && string.IsNullOrEmpty(endTime))
            {
                sql.AppendFormat(@" and CreateDate >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
                sql.AppendFormat(@" and CreateDate <= '{0}'", DateTime.Now.AddDays(1));
            }
            if (!string.IsNullOrEmpty(deviceSN))
            {
                sql.AppendFormat(@" AND ld.DeviceSN='{0}'", deviceSN);
            }
            sql.Append(" ORDER BY ld.DeviceSN,ld.CreateDate");
            DataTable table = comdal.GetDataTableBySQL(sql);

            if (null != table && table.Rows.Count > 0)
            {
                return Json(table, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
            }
            else
            {
                return NotFound();
            }
        }


        #region 获取经纬度
        private string GetJW(string coordinate, out string wd)
        {
            string jd = "";
            wd = "";
            if (!string.IsNullOrWhiteSpace(coordinate))
            {
                if (coordinate.IndexOf(",") > -1)
                {
                    jd = coordinate.Split(',')[0];
                    wd = coordinate.Split(',')[1];
                }
            }
            return jd;
        }
        #endregion


        public bool CheckToken()
        {
            bool flag = false;
            IEnumerable<string> val;
            Request.Headers.TryGetValues("token", out val);
            if (null != val.FirstOrDefault())
            {
                if (val.FirstOrDefault() == "XXXXXXXX")
                {
                    flag = true;
                }
            }
            return flag;
        }
    }
}