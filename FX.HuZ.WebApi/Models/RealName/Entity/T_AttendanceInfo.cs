using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models.RealName
{
    /// <summary>
    /// 考勤数据入库表
    /// </summary>
    [Serializable]
    public class T_AttendanceInfo
    {
        public string KEYID { get; set; }

        public string PERSONKEYID { get; set; }

        public string ATTENDANCETIME { get; set; }

        public string ATTENDANCEDOORTYPE { get; set; }

        public string IMAGEBUFFER { get; set; }
    }
}