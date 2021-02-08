using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FX.HuZ.WebApi.Test
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string requestUri = "http://192.168.0.231:800/api/TowerCraneReceive";
            HttpClient httpClient = new HttpClient();
            string param = @"{""iot_sys_devId"":""010914060102"",""iot_sys_pk"":""494038d4779343c0b413700bf33ba72f"",""iot_sys_req"":{""action"":""devSend"",""msgId"":0,""data"":{""cmd"":""upWorkData"",""params"":{""SensorError"":0,""MaxWindSpeed"":0.0,""CollectionTime"":""2019-12-31 13:58:20"",""PreAlarm"":0,""BeginHeight"":0.0,""BeginRadius"":0.0,""MaxLoad"":0.0,""EndAngle"":0.0,""MaxPercent"":0.0,""fall"":1,""Alarm"":0,""BeginAngle"":0.0,""EndRadius"":0.0,""Id"":""010914060102 "",""EndHeight"":0.0}}},""iot_sys_cmd"":""upWorkData"",""iot_sys_resp"":{""action"":""devSendResp"",""msgId"":0,""pk"":""494038 d4779343c0b413700bf33ba72f"",""devId"":""010914060102"",""code"":0,""desc"":""success""},""iot_sys_timestamp"":1577771916142,""iot_sys_params"":{""SensorError"":0,""MaxWindSpeed"":0.0,""CollectionTime"":""2019-12-31 13:58:20"",""PreAlarm"":0,""BeginHeight"":0.0,""BeginRadius"":0.0,""MaxLoad"":0.0,""EndAngle"":0.0,""MaxPercent"":0.0,""fall"":1,""Alarm"":0,""BeginAngle"":0.0,""EndRadius"":0.0,""Id"":""010914060102"",""EndHeight"":0.0},""iot_sys_event"":""devSend"",""iot_sys_raw"":{""action"":""devSend"",""msgId"":0,""data"":{""cmd"":""upWorkData"",""params"":{""SensorError"":0,""MaxWindSpeed"":0.0,""CollectionTime"":""2019-12-31 13:58:20"",""PreAlarm"":0,""BeginHeight"":0.0,""BeginRadius"":0.0,""MaxLoad"":0.0,""EndAngle"":0.0,""MaxPercent"":0.0,""fall"":1,""Alarm"":0,""BeginAngle"":0.0,""EndRadius"":0.0,""Id"":""010914060102"",""EndHeight"":0.0}}}}";
            StringContent content = new StringContent(param, Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> task = httpClient.PostAsync(requestUri, content);
            Task<string> task2 = task.Result.Content.ReadAsStringAsync();
            Label1.Text = task2.Result;
        }
    }
}