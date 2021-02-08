﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models.TowerCrane
{
    /// <summary>
    /// 塔吊黑匣子工作数据(upWorkData)
    /// </summary>
    [Serializable]
    public class @params
    {
        public int SensorError { get; set; }
        public double MaxWindSpeed { get; set; }
        public string CollectionTime { get; set; }
        public int PreAlarm { get; set; }
        public double BeginHeight { get; set; }
        public double BeginRadius { get; set; }
        public double MaxLoad { get; set; }
        public double EndAngle { get; set; }
        public double MaxPercent { get; set; }
        public int fall { get; set; }
        public double Alarm { get; set; }
        public double BeginAngle { get; set; }
        public double EndRadius { get; set; }
        public string Id { get; set; }
        public double EndHeight { get; set; }
    }
}