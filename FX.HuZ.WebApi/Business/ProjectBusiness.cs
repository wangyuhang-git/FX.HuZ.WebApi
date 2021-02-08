using FX.FP.Busines.NewDAL;
using FX.FP.Common.DotNetExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Business
{
    public class ProjectBusiness
    {
        protected Common_Dal comdal = new Common_Dal("SqlServer_FP_DB");

        public string GetStaffId(string areaName)
        {
            string sql = string.Format(@"SELECT GUID FROM dbo.Base_Dictionary WHERE ParentGUID='33603971-c57a-4cad-b7bd-c26d9b6553d5'
            AND DictionaryName='{0}'", areaName);
            object flag = comdal.ExecuteScalar(sql);
            return flag.ToString2();
        }

        public bool IsStaffid(string staffId, string areaName)
        {
            if (string.IsNullOrEmpty(staffId) || string.IsNullOrEmpty(areaName))
                return false;
            return staffId == this.GetStaffId(areaName);
        }

        public DataTable GetProjectTable(string areaName)
        {
            DataTable dt = null;
            string sql = string.Format(@"SELECT p.PROJECTNUM,p.SEGMENTNAME,p.CONSTRUCTPERMITNUM, p.BUILDUNITNAME,p.COORDINATE,
            dbo.GetConstructByProject(p.GUID) CONSTRUCTIONUNITNAME,--施工企业
            dbo.GetSupervisionByProject(p.GUID) SUPERVISIONUNITNAME,--监理企业
            CASE p.PROJECTSTATUS WHEN '01' THEN '在建' WHEN '02' THEN '停工' WHEN '03' THEN '竣工' ELSE '未知' END PROJECTSTATUS
            FROM dbo.NT_Project p WHERE p.SegmentAddressArea = '{0}' AND (IsDelete is null or IsDelete=0)", areaName);
            dt = comdal.GetDataTableBySQL(new System.Text.StringBuilder(sql));
            return dt;
        }

        public DataTable GetProjectInfo(string areaName, string constructPermitnum)
        {
            DataTable dt = null;
            string sql = string.Format(@"SELECT p.PROJECTNUM,p.SEGMENTNAME,p.CONSTRUCTPERMITNUM, p.BUILDUNITNAME,p.COORDINATE,
            dbo.GetConstructByProject(p.GUID) CONSTRUCTIONUNITNAME,--施工企业
            dbo.GetSupervisionByProject(p.GUID) SUPERVISIONUNITNAME,--监理企业
            dbo.GetNameORTelByProject(p.GUID,'name','B') PROJECTMANAGER,
            dbo.GetNameORTelByProject(p.GUID,'tel','B') PROJECTMANAGERTEL,
            dbo.GetNameORTelByProject(p.GUID,'name','C') PROJECTSAFETYNAME,
            dbo.GetMajordomoByProject(p.GUID) MAJORDOMONAME,p.MAJORDOMOTEL,
            CASE p.PROJECTSTATUS WHEN '01' THEN '在建' WHEN '02' THEN '停工' WHEN '03' THEN '竣工' ELSE '未知' END PROJECTSTATUS
            FROM dbo.NT_Project p WHERE (IsDelete is null or IsDelete=0) AND  p.SegmentAddressArea = '{0}'
            AND p.ConstructPermitNum='{1}'", areaName, constructPermitnum);
            dt = comdal.GetDataTableBySQL(new System.Text.StringBuilder(sql));
            return dt;
        }


    }
}