using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FX.HuZ.WebApi.Common
{
    /// <summary>
    /// sql语句where条件帮助类
    /// </summary>
    public class WhereHelper
    {
        /// <summary>
        /// 根据字典值拼接sql查询语句
        /// </summary>
        /// <param name="searchDic"></param>
        /// <returns></returns>
        public static string GetCondition(Dictionary<string, string> searchDic)
        {
            StringBuilder condition = new StringBuilder(" where 1=1 ");
            if (null != searchDic && searchDic.Count > 0)
            {
                foreach (var item in searchDic)
                {
                    string parmeName = string.Empty; //要查询的字段
                    string parmeValue = item.Value;//要查询的值
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        if (item.Key.StartsWith("s_0_"))//模糊查询
                        {
                            parmeName = item.Key.Replace("s_0_", "");//获取字段名字
                            string[] parmeArr = parmeName.Split(new char[] { '|' });//多个字段模糊查询 用 or
                            if (parmeArr.Length > 0)
                            {
                                condition.Append(" and (");
                                foreach (string newitem in parmeArr)
                                {
                                    if (!string.IsNullOrEmpty(newitem))
                                    {
                                        if (newitem != parmeArr[0]) //如果不是第一个，则sql语句加上 or
                                            condition.Append(" or ");

                                        condition.AppendFormat("  {0} like '%{1}%' ", newitem, parmeValue);
                                    }

                                }
                                condition.Append(" ) ");
                            }
                        }
                        else if (item.Key.StartsWith("s_1_"))//精确查询
                        {
                            parmeName = item.Key.Replace("s_1_", "");//获取字段名字                        
                            condition.AppendFormat(" AND {0}='{1}'", parmeName, item.Value);
                        }
                        else if (item.Key.StartsWith("s_3_"))//时间段查询
                        {
                            parmeName = item.Key.Replace("s_3_", "");//获取字段名字
                            string[] valueArr = item.Value.Split(new char[] { ',' });
                            if (valueArr.Length == 2)
                            {
                                condition.Append(GetDateSql(valueArr[0], valueArr[1], parmeName));
                            }
                        }
                        else
                        {
                            throw new ArgumentNullException("根据时间段查询条件的格式不正确！");
                        }
                    }
                    else if (item.Key.StartsWith("s_5_"))//数字段查询
                    {
                        parmeName = item.Key.Replace("s_5_", "");//获取字段名字
                        condition.AppendFormat(" AND {0} in ({1})", parmeName, item.Value);
                    }
                }
            }
            return condition.ToString();
        }


        public static string GetDateSql(string strStart, string strEnd, string fieldName)
        {
            string returnValue = string.Empty;
            if (!string.IsNullOrEmpty(strStart))
            {
                try
                {
                    DateTime strDate = Convert.ToDateTime(strStart + " 0:00:00");
                    returnValue = string.Format(" AND {0}>='{1}' ", fieldName, strDate.ToString());
                }
                catch (System.Exception ex)
                {
                    throw new ApplicationException(ex.Message);
                }
            }
            if (!string.IsNullOrEmpty(strEnd))
            {
                try
                {
                    DateTime strDate = Convert.ToDateTime(strEnd + " 23:59:59");
                    returnValue += string.Format(" AND {0}<='{1}' ", fieldName, strDate.ToString());
                }
                catch (System.Exception ex)
                {
                    throw new ApplicationException(ex.Message);
                }
            }
            return returnValue;
        }
    }
}