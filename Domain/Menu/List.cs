using Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Domain.Menu
{
    public class List
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public long? Sort { get; set; }

        /// <summary>
        /// 权限位域值
        /// </summary>
        public long? LimitFlag { get; set; }

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

        /// <summary>
        /// 主键
        /// </summary>
        public string UNID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public long Flag { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EnumAutoMapper("Flag", typeof(Enum.GlobalFlag))]
        public string FlagState { get; set; }


        /// <summary>
        /// 父级名称
        /// </summary>
        [DataAutoMapper("ParentID", typeof(Repository.Menu), "UNID", "Name")]
        public string ParentName { get; set; }
    }
}