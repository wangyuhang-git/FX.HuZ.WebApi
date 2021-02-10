using FX.FP.Busines.NewDAL;
using FX.FP.Common.DotNetCode;
using FX.FP.Common.DotNetEncrypt;
using FX.FP.Common.DotNetExt;
using FX.HuZ.WebApi.Common;
using FX.HuZ.WebApi.Models;
using FX.HuZ.WebApi.Models.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace FX.HuZ.WebApi.Business.EnterpriseApp
{
    /// <summary>
    /// 用户登录操作类
    /// </summary>
    public class UserInfoEpBusiness
    {
        private Common_Dal common = new Common_Dal("SqlServer_FP_DB");
        /// <summary>
        /// 账号密码方式登录
        /// </summary>
        public ResultMsg Login(string userName, string userPwd)
        {
            ResultMsg resultMsg = new ResultMsg();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPwd))
            {
                resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
            }
            else
            {
                string pass = ConfigurationManager.AppSettings["PassWord"].Trim();
                string account = userName;
                string pwd = account + userPwd + pass;
                StringBuilder strSql = new StringBuilder
                    (@"SELECT CASE e.IsSubAccount WHEN 1 THEN ParentGUID ELSE GUID END AS GUID,
	            CASE e.IsSubAccount WHEN 1 THEN  CASE (SELECT TOP 1 M.IsOpen FROM ET_EnterpriseRegister M WHERE M.GUID = E.ParentGUID) 
	            WHEN 1 THEN E.IsOpen ELSE 0 END ELSE IsOpen END AS IsOpen, 
	            e.User_Account, e.EnterpriseName, e.User_Pwd, e.OrganizationCode ,ISNULL(u.EnterpriseType,E.AccountType) AccountType ,e.IsSubAccount ,e.ProjectGUIDS
	            FROM ET_EnterpriseRegister E LEFT JOIN dbo.ET_Enterprise_UserRole u ON e.GUID = u.ER_User_ID
                WHERE User_Account = @User_Account AND User_Pwd = @User_Pwd AND (ISDELETE IS NULL OR ISDELETE = 0)");
                SqlParam[] para = new SqlParam[]
                {
                new SqlParam("@User_Account", account),
                new SqlParam("@User_Pwd", Md5Helper.MD5(pwd, 32))
                };
                DataTable dtlogin = common.GetDataTableBySQL(strSql, para);
                if (dtlogin != null && dtlogin.Rows.Count > 0)
                {
                    if (dtlogin.Rows[0]["IsOpen"].ToString2() != "1")
                    {
                        resultMsg.StatusCode = (int)StatusCodeEnum.Unauthorized;
                        resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                    }
                    else
                    {
                        resultMsg.StatusCode = (int)StatusCodeEnum.Success;
                        resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
                        resultMsg.Data =
                            new
                            {
                                AccountType = dtlogin.Rows[0]["AccountType"].ToString2(),
                                OrganizationCode = dtlogin.Rows[0]["OrganizationCode"].ToString2()
                            };
                    }
                }
                else
                {
                    resultMsg.StatusCode = (int)StatusCodeEnum.ParameterError;
                    resultMsg.Info = StatusCodeEnum.ParameterError.GetEnumText();
                }
            }
            return resultMsg;
        }
    }
}