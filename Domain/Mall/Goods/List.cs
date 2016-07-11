using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mall.Goods
{
    public class List
    {

        /// <summary>
        /// 主键
        /// </summary>
        public string UNID { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string Name { get; set; }

        public string CategoryName { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public int StockNum { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int SurplusNum { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public System.DateTime OverTime { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public System.DateTime OngoingTime { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SellingPrice { get; set; }
        /// <summary>
        /// 需要积分
        /// </summary>
        public int ScoreNum { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public long Flag { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// 菜品状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreatedTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public System.DateTime UpdatedTime { get; set; }
    }
}
