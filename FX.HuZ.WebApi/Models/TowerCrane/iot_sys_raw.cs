using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models.TowerCrane
{
    [Serializable]
    public class iot_sys_raw
    {
        public string action { get; set; }

        public data data { get; set; }
        
        public string msgId { get; set; }
    }
}