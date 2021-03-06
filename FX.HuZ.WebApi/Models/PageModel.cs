﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FX.HuZ.WebApi.Models
{
    public class PageModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页数据数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 排序字典值
        /// </summary>
        public object SortDic { get; set; }
        /// <summary>
        /// 查询条件字典值
        /// </summary>
        public object SearchDic { get; set; }        
    }
}