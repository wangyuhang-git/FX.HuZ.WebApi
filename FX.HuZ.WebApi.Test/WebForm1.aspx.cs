using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FX.HuZ.WebApi.Test
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();

            param.Add(new KeyValuePair<string, string>("sign", "000000"));
            param.Add(new KeyValuePair<string, string>("payload", "d6932849575a4976b63aa4235b9b6e6f"));
            string str = JsonConvert.SerializeObject(param);
            //string url = "http://60.12.107.219:800/api/DustReceiveByFrom";
            string url = "http://192.168.0.231:800/api/DustReceive";//调用方式一
            Task<HttpResponseMessage> responseMessage = httpClient.PostAsync(url, new FormUrlEncodedContent(param));

            responseMessage.Wait();
            Task<string> reString = responseMessage.Result.Content.ReadAsStringAsync();
            reString.Wait();
            Label1.Text = "返回结果：" + reString.Result;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //调用方式二
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://192.168.0.231:800/api/DustReceive");
            request.Method = "POST";
            request.ContentType = "application/json";
            string content = "{ 'sign': 'sign_000000', 'payload':'payload_aaaaaaaaa'}";
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            request.GetRequestStream().Write(buffer, 0, buffer.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();//发送
            Stream myResponseStream = response.GetResponseStream();
            //获取返回值
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();//及时关闭
            myResponseStream.Close();//及时关闭
            response.Close();//及时关闭

            //最后要把HttpWebRequest及时关闭释放
            if (request != null)
            {
                request.Abort();
            }
            Label1.Text = retString;
        }


        /// <summary>
        /// 通过请求体向指定URL传输数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="objBody">需传输的数据</param>
        /// <returns></returns>
        private string SendObjectAsJsonInBody(string url, object objBody)
        {
            string result;
            HttpClient client;
            StringContent content;

            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            content = new StringContent(JsonConvert.SerializeObject(objBody), Encoding.UTF8, "application/json");
            result = client.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
            return result;
        }
    }
}