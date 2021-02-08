using FX.HuZ.WebApi.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models.Dust
{
    public class NodeNewModel
    {
        public string Hum { get; set; }
        public string NodeId { get; set; }
        public string Tem { get; set; }
    }
}