using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumPro
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum DinnerOrderState
    {

        [Description("等审核")]
        Audting = 0,

        [Description("完成")]
        Complate = 1,

        [Description("无效")]
        Invalid = 2,
    }
}
