using FX.FP.Busines.NewDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Business
{
    public class DictionaryBusiness
    {
        protected Dictionary_Dal comdal = new Dictionary_Dal("SqlServer_FP_DB");
        /// <summary>
        /// 根据字典编码获取字典
        /// </summary>
        /// <param name="dictionaryCode">编码</param>
        public DataTable GetDictionaryList(string dictionaryCode)
        {
            DataTable dt = comdal.GetDictionaryList(dictionaryCode);
            return dt;
        }
    }
}