using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FX.FP.Busines.NewDAL;
using FX.FP.Common.DotNetExt;
using System.Data;
using FX.FP.Common.DotNetCode;
using System.Collections;
using FX.FP.Common.DotNetEncrypt;

namespace FX.HuZ.WebApi.Business
{
    /// <summary>
    /// 用户信息处理类
    /// </summary>
    public class UserInfoBusiness
    {
        protected Common_Dal comdal = new Common_Dal("SqlServer_FP_DB");
        /// <summary>
        /// 标点系统-登录验证码查询用户
        /// </summary>
        /// <param name="verCode">动态验证码</param>
        /// <returns>用户信息</returns>
        public DataTable GetUserInfoTable(string verCode)
        {
            DataTable dt = null;
            string sql = string.Format(@"SELECT  [User_ID], [User_Name] AS username,User_TelPhone AS userphone 
                FROM  dbo.Base_UserInfo WHERE DeleteMark=1 and PunctuationVerCode=@verCode ");
            
            dt = comdal.GetDataTableBySQL(new System.Text.StringBuilder(sql)
                ,new SqlParam[] { new SqlParam("@verCode", Md5Helper.MD5(verCode,32))});
            //查询成功且有数据时,仅存在一条数据
            if (null != dt && dt.Rows.Count == 1)
            {
                Hashtable ht = new Hashtable();
                ht["PUNCTUATIONVERCODE"] = null;
                comdal.SubmitData("Base_UserInfo", "User_ID", dt.Rows[0]["User_ID"].ToString2(),ht);
                dt.Columns.Remove("User_ID");
            }
            return dt;
        }

    }
}