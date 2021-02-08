﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FX.HuZ.WebApi.Test
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();

            param.Add(new KeyValuePair<string, string>("iot_sys_raw", "{\"action\":\"devSend\",\"data\":{\"cmd\":\"report\",\"params\":{\"io_mode\":1,\"io_time\":\"20150115203948\",\"log_id\":\"18\",\"log_image\":\"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\",\"user_id\":\"1\",\"verify_mode\":20}},\"devId\":\"DC68C8058F5D5236\",\"msgId\":1,\"pk\":\"2524800215e04014b811d788266927d7\"}"));
            string str = Newtonsoft.Json.JsonConvert.SerializeObject(param);
            string url = "http://192.168.0.231:800/api/AttendanceReceive";
            Task<HttpResponseMessage> responseMessage = httpClient.PostAsync(url, new FormUrlEncodedContent(param));

            responseMessage.Wait();
            Task<string> reString = responseMessage.Result.Content.ReadAsStringAsync();
            reString.Wait();
            Label1.Text = "返回结果：" + reString.Result;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string dd = "{\"action\":\"devSend\",\"data\":{\"cmd\":\"report\",\"params\":{\"io_mode\":1,\"io_time\":\"20150118212940\",\"log_id\":\"1\",\"log_image\":\"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\",\"user_id\":\"1\",\"verify_mode\":20}},\"devId\":\"DC68C8058F5D5236\",\"msgId\":1,\"pk\":\"2524800215e04014b811d788266927d7\"}\n";
            //dd = dd.Replace("params", "paramsStr");
            FX.HuZ.WebApi.Test.Models.Attendance.iot_sys_raw iot_sys_raw = JsonConvert.DeserializeObject<FX.HuZ.WebApi.Test.Models.Attendance.iot_sys_raw>(dd);
            FX.HuZ.WebApi.Test.Models.Attendance.data data = JsonConvert.DeserializeObject<FX.HuZ.WebApi.Test.Models.Attendance.data>(JsonConvert.SerializeObject(iot_sys_raw.data));
            var data1 = iot_sys_raw.data;

        }
    }
}