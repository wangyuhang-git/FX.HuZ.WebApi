using FX.HuZ.WebApi.Converter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
namespace FX.HuZ.WebApi.Models.Dust
{
    public class DustInfoNewModel
    {
        /// <summary>
        /// 坐标类型
        /// </summary>
        public string CoordinateType { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Lat { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Lng { get; set; }

        /// <summary>
        /// 上报日期
        /// </summary>
        public string ReportDate { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        [JsonConverter(typeof(DustNodeConverter))]
        public List<NodeNewModel> Nodelist { get; set; }

        /// <summary>
        /// 回复状态
        /// </summary>
        public string RelayStatus { get; set; }
    }
}