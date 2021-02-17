using FX.FP.Busines.NewDAL;
using FX.FP.Common.DotNetCode;
using FX.HuZ.WebApi.Common;
using FX.HuZ.WebApi.Models;
using FX.HuZ.WebApi.Models.EnterpriseApp;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace FX.HuZ.WebApi.Business.EnterpriseApp
{
    public class ProjectEpBusiness
    {
        private Common_Dal common = new Common_Dal("SqlServer_FP_DB");

        /// <summary>
        /// 得到项目列表
        /// </summary>
        /// <param name="accountType">企业类型</param>
        /// <param name="organizationCode">统一社会信用代码</param>
        /// <returns></returns>
        public DataTable GetProjectTable(string accountType, string organizationCode)
        {
            DataTable dt = null;
            StringBuilder sql = new StringBuilder(@"SELECT * FROM DBO.NT_PROJECT P WHERE (ISDELETE IS NULL OR ISDELETE=0) ");
            if (accountType == "03")//监理企业
            {
                sql.Append(" AND P.APPLYSTATE='2' AND P.SUPERVISEUNITCODE=@OrganizationCode");
            }
            else if (accountType == "02")//施工企业
            {
                sql.Append(" AND P.CONSTRUCTUNITCODE=@OrganizationCode");
            }
            else
            {
                sql.Append(" AND P.APPLYSTATE='2' AND (P.SUPERVISEUNITCODE=@OrganizationCode OR P.CONSTRUCTUNITCODE=@OrganizationCode)");
            }
            dt = common.GetDataTableBySQL(sql
               , new SqlParam[] { new SqlParam("@OrganizationCode", organizationCode) });
            return dt;
        }
        /*
        /// <summary>
        /// 根据条件获取项目列表
        /// </summary>
        /// <param name="projectParam"></param>
        /// <returns></returns>
        public ResultModel GetProjectList(ProjectPageSearch projectParam)
        {
            ResultModel resultModel = new ResultModel();
            int count = 0;
            DataTable dt = null;
            //StringBuilder sql = new StringBuilder(@"SELECT * FROM DBO.NT_PROJECT P WHERE (ISDELETE IS NULL OR ISDELETE=0) ");

            string tabName = "DBO.NT_PROJECT P";
            string cols = @"P.GUID,P.PROJECTNUM,P.CONSTRUCTPERMITNUM,P.CREATETIME,P.SEGMENTNAME,P.SEGMENTADDRESS,SEGMENTADDRESSAREA,
            P.BUILDAREA,P.SEGMENTPRICE,P.PROJECTSTYLE,P.PLANSTARTDATE,P.PLANENDDATE,P.COORDINATE,
            P.BUILDUNITNAME,P.BUILDUNITCODE,P.BUILDUNITPRINCIPAL,P.BUILDUNITPRINCIPALTEL,
            P.RECONNAISSANCEUNITNAME,P.RECONNAISSANCEUNITCODE,P.RECONNAISSANCEUNITPRINCIPAL,P.RECONNAISSANCEUNITPRINCIPALTEL,
            P.DESIGNUNITNAME,P.DESIGNUNITCODE,P.DESIGNUNITPRINCIPAL,P.DESIGNUNITPRINCIPALTEL,
            P.SUPERVISEUNITGUID,P.SUPERVISEUNITNAME,P.SUPERVISEUNITCODE,P.MAJORDOMOGUID,P.MAJORDOMO,P.MAJORDOMOTEL,
            P.CONSTRUCTUNITGUID,P.CONSTRUCTUNITNAME,P.CONSTRUCTUNITCODE,P.PROJECTMANAGERGUID,P.PROJECTMANAGER,P.PROJECTMANAGERTEL,P.SAFETYPERSON,
            P.APPLYSTATE,P.APPLYDATE,P.AUDITDATE,P.AUDITREPLY,
            P.SUPERVISIONDEPARTMENTGUID,P.SUPERVISIONDEPARTMENT,P.SUPERVISIONPERSONGUID,P.SUPERVISIONPERSON,P.SUPERVISORGUID,P.SUPERVISOR";
            StringBuilder sqlWhere = new StringBuilder(" WHERE (ISDELETE IS NULL OR ISDELETE=0)");

            if (projectParam.AccountType == "03")//监理企业
            {
                sqlWhere.Append(" AND P.APPLYSTATE='2' AND P.SUPERVISEUNITCODE=@OrganizationCode");
            }
            else if (projectParam.AccountType == "02")//施工企业
            {
                sqlWhere.Append(" AND P.CONSTRUCTUNITCODE=@OrganizationCode");
            }
            else
            {
                sqlWhere.Append(" AND P.APPLYSTATE='2' AND (P.SUPERVISEUNITCODE=@OrganizationCode OR P.CONSTRUCTUNITCODE=@OrganizationCode)");
            }

            string order = " CREATETIME DESC";
            dt = common.GetListByPage(tabName, cols, sqlWhere, order,
                new SqlParam[] { new SqlParam("@OrganizationCode", projectParam.OrganizationCode) },
                projectParam.PageIndex, projectParam.PageSize, ref count);
            resultModel.success = true;
            resultModel.total = count;
            resultModel.rows = dt;
            return resultModel;
        }
        */

        /// <summary>
        /// 根据条件获取项目列表
        /// </summary>
        /// <param name="projectParam"></param>
        /// <returns></returns>
        public ResultModel GetProjectList(ProjectPageSearch projectParam)
        {
            ResultModel resultModel = new ResultModel();
            int count = 0;
            DataTable dt = null;
            string tabName = "DBO.NT_PROJECT P";
            string cols = @"P.GUID,P.PROJECTNUM,P.CONSTRUCTPERMITNUM,P.CREATETIME,P.SEGMENTNAME,P.SEGMENTADDRESS,SEGMENTADDRESSAREA,
            P.BUILDAREA,P.SEGMENTPRICE,P.PROJECTSTYLE,P.PLANSTARTDATE,P.PLANENDDATE,P.COORDINATE,
            P.BUILDUNITNAME,P.BUILDUNITCODE,P.BUILDUNITPRINCIPAL,P.BUILDUNITPRINCIPALTEL,
            P.RECONNAISSANCEUNITNAME,P.RECONNAISSANCEUNITCODE,P.RECONNAISSANCEUNITPRINCIPAL,P.RECONNAISSANCEUNITPRINCIPALTEL,
            P.DESIGNUNITNAME,P.DESIGNUNITCODE,P.DESIGNUNITPRINCIPAL,P.DESIGNUNITPRINCIPALTEL,
            P.SUPERVISEUNITGUID,P.SUPERVISEUNITNAME,P.SUPERVISEUNITCODE,P.MAJORDOMOGUID,P.MAJORDOMO,P.MAJORDOMOTEL,
            P.CONSTRUCTUNITGUID,P.CONSTRUCTUNITNAME,P.CONSTRUCTUNITCODE,P.PROJECTMANAGERGUID,P.PROJECTMANAGER,P.PROJECTMANAGERTEL,P.SAFETYPERSON,
            P.APPLYSTATE,P.APPLYDATE,P.AUDITDATE,P.AUDITREPLY,
            P.SUPERVISIONDEPARTMENTGUID,P.SUPERVISIONDEPARTMENT,P.SUPERVISIONPERSONGUID,P.SUPERVISIONPERSON,P.SUPERVISORGUID,P.SUPERVISOR";

            Dictionary<string, string> searchDic= JsonConvert.DeserializeObject<Dictionary<string, string>>(projectParam.SearchDic.ToString());
            StringBuilder sqlWhere = new StringBuilder(WhereHelper.GetCondition(searchDic));

            if (projectParam.AccountType == "03")//监理企业
            {
                sqlWhere.Append(" AND P.APPLYSTATE='2' AND P.SUPERVISEUNITCODE=@OrganizationCode");
            }
            else if (projectParam.AccountType == "02")//施工企业
            {
                sqlWhere.Append(" AND P.CONSTRUCTUNITCODE=@OrganizationCode");
            }
            else
            {
                sqlWhere.Append(" AND P.APPLYSTATE='2' AND (P.SUPERVISEUNITCODE=@OrganizationCode OR P.CONSTRUCTUNITCODE=@OrganizationCode)");
            }

            string order = " CREATETIME DESC";

            dt = common.GetListByPage(tabName, cols, sqlWhere, order,
                new SqlParam[] { new SqlParam("@OrganizationCode", projectParam.OrganizationCode) },
                projectParam.PageIndex, projectParam.PageSize, ref count);
            resultModel.success = true;
            resultModel.total = count;
            resultModel.rows = dt;
            return resultModel;
        }

    }
}