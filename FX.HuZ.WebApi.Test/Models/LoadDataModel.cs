using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models
{
    public class LoadDataModel
    {
        /// <summary>
        /// 经度
        /// </summary>
        public string lng { get; set; }

        /// <summary>
        /// 回复状态
        /// </summary>
        public string relayStatus { get; set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        public string coordinateType { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public List<NodeListDataModel> nodelist { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string deviceId { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public string lat { get; set; }
    }
}