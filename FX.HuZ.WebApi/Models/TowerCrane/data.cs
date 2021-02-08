using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models.TowerCrane
{
    [Serializable]
    public class data
    {
        /// <summary>
        /// 数据类型：upWorkData，upLiveData
        /// </summary>
        public string cmd { get; set; }

        //public @params @params { get; set; }
        [JsonProperty("params")]
        public @params paramsValue { get; set; }
    }
}