using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumPro
{
    /// <summary>
    /// 积分活动类型
    /// </summary>
    public enum ScoreType
    {
        [Description("商城消费")]
        Mall = 0,

        [Description("签到")]
        Sign = 1,

        [Description("拼图")]
        Puzzle = 2

    }
}
