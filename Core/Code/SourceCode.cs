using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Code
{
    public enum SourceCode
    {

        None =1,

        /// <summary>
        /// PC端网页
        /// </summary>
        Web = 2,

        /// <summary>
        /// 应用
        /// </summary>
        App = 3,

        /// <summary>
        /// 微站
        /// </summary>
        WeChat = 4,

        /// <summary>
        /// 移动端网页
        /// </summary>
        Wap = 5
    }
}
