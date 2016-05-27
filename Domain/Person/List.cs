using Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Person
{
    public class List
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string UNID { get; set; }


        public string Account { get; set; }

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
        /// 角色位域值
        /// </summary>
        public long? RoleFlag { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EnumAutoMapper("Flag", typeof(Enum.GlobalFlag))]
        public string FlagState { get; set; }        
    }
}
