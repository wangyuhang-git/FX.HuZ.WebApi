using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models
{
    public class ResultModel
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public long total { get; set; }
        public object rows { get; set; }

        public ResultModel()
        {
            this.total = 0;
            this.rows = new object();
            success = false;
            msg = string.Empty;
        }
    }
}