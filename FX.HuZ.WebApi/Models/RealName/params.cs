using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models.RealName
{
    [Serializable]
    public class @params
    {
        public string PostType { get; set; }

        public string key { get; set; }

        public string ProjectGUID { get; set; }

        //public List<T_AttendanceInfo> syncData { get; set; }
        public string syncData { get; set; }
    }
}