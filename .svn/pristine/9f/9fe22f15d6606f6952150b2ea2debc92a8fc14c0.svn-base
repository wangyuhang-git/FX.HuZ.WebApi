using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models.Attendance.Entity
{
    /// <summary>
    /// 考勤数据入库表
    /// </summary>
    public class tbl_realtime_glog
    {
        /// <summary>
        /// 入库时间
        /// </summary>
        public string update_time { get; set; }
        /// <summary>
        /// 设备编号，对应devId
        /// </summary>
        public string device_id { get; set; }
        /// <summary>
        /// 考勤刷脸时间，需要解析
        /// </summary>
        public string io_time { get; set; }
        /// <summary>
        /// 人员编号
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 考勤图片，base64
        /// </summary>
        public string log_image { get; set; }
        /// <summary>
        /// ["FACE"]
        /// </summary>
        public string verify_mode { get; set; }
        /// <summary>
        /// IN
        /// </summary>
        public string io_mode { get; set; }
        /// <summary>
        /// 0
        /// </summary>
        public string receivestatus { get; set; }
    }
}