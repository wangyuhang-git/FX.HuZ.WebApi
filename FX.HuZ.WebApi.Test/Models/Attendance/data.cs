using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Test.Models.Attendance
{
    [Serializable]
    public class data
    {
        public string cmd { get; set; }
        [JsonProperty("params")]
        public FX.HuZ.WebApi.Test.Models.Attendance.@params paramsValue { get; set; }
    }
}