﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.Menu
{
    public class Update
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string UNID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public long Flag { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public long? Sort { get; set; }        

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public string ClassName { get; set; }
    }
}