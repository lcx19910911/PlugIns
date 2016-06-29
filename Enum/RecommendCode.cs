﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumPro
{
    public enum RecommendCode
    {

        /// <summary>
        /// 成功
        /// </summary>
        [Description("首页推荐商品")]
        HomeGoods = 1,
        /// <summary>
        /// 服务器错误
        /// </summary>
        [Description("首页推荐分类")]
        HomeCategory = 2,
    }
}
