using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Test.Models.Attendance
{
    public class iot_sys_raw
    {
        public string action { get; set; }

        public FX.HuZ.WebApi.Test.Models.Attendance.data data { get; set; }

        public string devId { get; set; }

        public string msgId { get; set; }

        public string pk { get; set; }
    }
}