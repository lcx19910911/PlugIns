using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.API
{
    /// <summary>
    /// 店铺
    /// </summary>
    public class ApiDinnerShopModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string UNID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
      
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreatedTime { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 自我介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 营业开始时间
        /// </summary>
        public string StartShoptime { get; set; }
        /// <summary>
        /// 营业结束时间
        /// </summary>
        public string EndShoptime { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Mark { get; set; }
    }
}
