using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Role
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
        /// 权限位域值
        /// </summary>
        public long? LimitFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
