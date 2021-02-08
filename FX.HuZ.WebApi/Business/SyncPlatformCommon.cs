using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FX.FP.Busines.NewDAL;
using System.Data;
using System.Text;
using FX.FP.Common.DotNetExt;
using FX.FP.Common.DotNetCode;

namespace FX.HuZ.WebApi.Business
{
    public class SyncPlatformCommon
    {
        Common_Dal systemidao = new Common_Dal("SqlServer_FP_DB_SMZ");
        Common_Dal commom = new Common_Dal();
        LogHelper log = new LogHelper("RealHandler");

        #region 获取业务表主键
        /// <summary>
        /// 获取业务表主键
        /// </summary>
        /// <param name="table">业务表</param>
        /// <param name="businessKey">业务表外键</param>
        /// <param name="GUID">业务表外键的值</param>
        /// <param name="isAlreadySync">是否已经同步过</param>
        /// <returns></returns>
        public string getBusinessGUID(string table, string businessKey, string GUID, out int isAlreadySync, out int syncState)
        {
            string businessGUID = string.Empty;
            isAlreadySync = 0;
            syncState = 1;
            DataTable dt = systemidao.GetDataTableBySQL(new StringBuilder(string.Format(@"select GUID,isAlreadySync,syncState from {0} with(nolock) where {1}='{2}'", table, businessKey, GUID)));
            if (dt != null)
            {
                if (dt.Rows.Count == 1)
                {
                    businessGUID = dt.Rows[0]["GUID"].ToString2();
                    isAlreadySync = dt.Rows[0]["ISALREADYSYNC"].ToInt32(0);
                    syncState = dt.Rows[0]["SYNCSTATE"].ToInt32(1);
                }
                else if (dt.Rows.Count > 1)
                {
                    isAlreadySync = 99;
                }
            }
            return businessGUID;
        }
        #endregion


        #region 同步数量统计
        /// <summary>
        /// 同步数量统计
        /// </summary>
        /// <param name="plat">同步平台</param>
        /// <param name="actionType">操作类型 add update delete</param>
        /// <param name="businessType">业务类型 Project Enterprise Company Team Worker</param>
        /// <param name="projectGUID">项目主键</param>
        /// <param name="syncState">原同步状态</param>
        /// <param name="isAlreadySync">是否已同步过</param>
        public void syncStatistics(string plat, string actionType, string businessType, string projectGUID, int syncState, int isAlreadySync)
        {
            string ssTable = string.Empty;
            switch (plat)
            {
                case "WC": ssTable = "WholeCountry_SyncStatistics"; break;
            }
            string c2 = string.Empty;
            switch (syncState)
            {
                //取出业务字段后半部分（原）
                case 1: c2 = "Untreated"; break;
                case 0: c2 = "Processing"; break;
                case -1: c2 = "Fail"; break;
                case 2: c2 = "Success"; break;
            }
            if (actionType == "add")
            {
                //新增的数据则总数累计加1，未处理累计加1
                systemidao.ExecuteBySql(new StringBuilder(string.Format(@"update {0} set {1}Total={1}Total+1,{1}Untreated={1}Untreated+1 where ProjectGUID='{2}'", ssTable, businessType, projectGUID)));
            }
            else if (actionType == "update")
            {
                if (syncState == 1)
                {
                    //未处理状态下不变更统计
                    return;
                }
                systemidao.ExecuteBySql(new StringBuilder(string.Format(@"update {0} set {1}Untreated={1}Untreated+1,{1}{2}={1}{2}-1 where ProjectGUID='{3}'", ssTable, businessType, c2, projectGUID)));
            }
            else if (actionType == "delete")
            {
                string setAlreadyCol = string.Empty;
                if (isAlreadySync == 1)
                {
                    //原数据已同步成功过
                    setAlreadyCol = string.Format(@" {0}AlreadySuccess={0}AlreadySuccess-1,{0}DelAlreadySuccess={0}DelAlreadySuccess+1, ", businessType);
                }
                systemidao.ExecuteBySql(new StringBuilder(string.Format(@"update {0} set {4} {1}Total={1}Total-1,{1}DelTotal={1}DelTotal+1,{1}{2}={1}{2}-1,{1}Del{2}={1}Del{2}+1 where ProjectGUID='{3}'", ssTable, businessType, c2, projectGUID, setAlreadyCol)));
            }
        }
        #endregion

        #region
        /// <summary>
        /// 处理统计表数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="projectGUID"></param>
        public void syncAddStatisticsDate(string table, string projectGUID)
        {
            try
            {
                #region 添加各同步统计表数据
                if (table == "WholeCountry")
                {
                    systemidao.ExecuteScalar(string.Format(@"insert into {0}_SyncStatistics(GUID,ProjectGUID,ProjectTotal,ProjectUntreated,
                    EnterpriseTotal,EnterpriseUntreated,EnterpriseDelTotal,EnterpriseDelUntreated,CompanyTotal,CompanyUntreated,CompanyDelTotal,CompanyDelUntreated
                    ,TeamTotal,TeamUntreated,TeamDelTotal,TeamDelUntreated,WorkerTotal,WorkerUntreated,WorkerDelTotal,WorkerDelUntreated)
                    select NEWID(),p.GUID,1,1,
                    (select count(GUID) from {0}_BuinessEnterprise where ProjectGUID=p.GUID  and isDelete=0),
                    (select count(GUID) from {0}_BuinessEnterprise where ProjectGUID=p.GUID  and isDelete=0),
                    (select count(GUID) from {0}_BuinessEnterprise where ProjectGUID=p.GUID and isDelete=1),     
                    (select count(GUID) from {0}_BuinessEnterprise where ProjectGUID=p.GUID and isDelete=1), 
                    (select count(GUID) from {0}_BuinessCompany where ProjectGUID=p.GUID and isDelete=0),        
                    (select count(GUID) from {0}_BuinessCompany where ProjectGUID=p.GUID and isDelete=0),
                    (select count(GUID) from {0}_BuinessCompany where ProjectGUID=p.GUID and isDelete=1),    
                    (select count(GUID) from {0}_BuinessCompany where ProjectGUID=p.GUID and isDelete=1),  
                    (select count(GUID) from {0}_BuinessProjectTeam where ProjectGUID=p.GUID  and isDelete=0),
                    (select count(GUID) from {0}_BuinessProjectTeam where ProjectGUID=p.GUID  and isDelete=0),
                    (select count(GUID) from {0}_BuinessProjectTeam where ProjectGUID=p.GUID and isDelete=1),      
                    (select count(GUID) from {0}_BuinessProjectTeam where ProjectGUID=p.GUID and isDelete=1),           
                    (select count(GUID) from {0}_BuinessProjectWorker where ProjectGUID=p.GUID  and isDelete=0),
                    (select count(GUID) from {0}_BuinessProjectWorker where ProjectGUID=p.GUID  and isDelete=0),
                    (select count(GUID) from {0}_BuinessProjectWorker where ProjectGUID=p.GUID and isDelete=1),      
                    (select count(GUID) from {0}_BuinessProjectWorker where ProjectGUID=p.GUID and isDelete=1)      
                    from T_Project p where p.IsDelete=0 and p.GUID='{1}'", table, projectGUID));
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper log = new LogHelper("syncBusiness");
                log.WriteLog("加" + table + "数据至统计表--------------" + ex.Message);
            }
        }

        #endregion


        #region 获取上级是否已同步
        /// <summary>
        /// 获取上级是否已同步
        /// </summary>
        /// <param name="table">业务表</param>
        /// <param name="businessKey">业务表外键</param>
        /// <param name="GUID">业务表外键的值</param>
        /// <returns></returns>
        public int getIsPrepositionSync(string table, string businessKey, string GUID)
        {
            int isPrepositionSync = systemidao.ExecuteScalar(string.Format(@"select top 1 isAlreadySync from {0} with(nolock) where {1} = '{2}' ", table, businessKey, GUID)).ToInt32(0);
            if (isPrepositionSync != 1)
            {
                isPrepositionSync = 0;
            }
            return isPrepositionSync;
        }
        #endregion

        #region 获取已同步成功班组编号
        /// <summary>
        /// 获取已同步成功班组编号
        /// </summary>
        /// <param name="table">业务班组表</param>
        /// <param name="businessKey">业务班组表外键</param>
        /// <param name="GUID">业务班组表外键的值</param>
        /// <param name="key">班组编号</param>
        /// <returns></returns>
        public string getTeamNo(string table, string businessKey, string GUID, string key)
        {
            string teamSysNo = systemidao.ExecuteScalar(string.Format(@"select top 1 {0} from {1} with(nolock) where {2} = '{3}' ", key, table, businessKey, GUID)).ToString2();
            return teamSysNo;
        }
        #endregion

    }
}