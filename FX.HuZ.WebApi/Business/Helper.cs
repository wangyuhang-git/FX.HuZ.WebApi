﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace FX.HuZ.WebApi.Business
{
    public static class Helper
    {
        /// <summary>
        /// 保存日志信息
        /// </summary>
        /// <param name="msg"></param>
        public static void SaveLog(string msg, string type = "")
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["LOGPath"];
            //判断文件夹是否存在
            if (!Directory.Exists(path))//判断目录是否存在{}
            { Directory.CreateDirectory(path); }
            string filepath = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + type + ".txt";
            FileStream fs = new FileStream(filepath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine("\r" + msg + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sw.Close();
            fs.Close();
        }
    }
}