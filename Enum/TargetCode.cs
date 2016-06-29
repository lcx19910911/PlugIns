using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumPro
{
    /// <summary>
    /// 活动类型
    /// </summary>
    public enum TargetCode
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin =0,

        /// <summary>
        /// 刮刮卡
        /// </summary>
        [Description("刮刮卡")]
        ScratchCard = 1,

        /// <summary>
        /// 微点餐
        /// </summary>
        [Description("微点餐")]
        Dinner = 2,

        /// <summary>
        /// 商品
        /// </summary>
        [Description("商品")]
        Goods = 3,


        /// <summary>
        /// 分类
        /// </summary>
        [Description("商品分类")]
        Category = 4,

    }
}
