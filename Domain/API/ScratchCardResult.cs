using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Domain.API
{
    /// <summary>
    /// 刮刮卡接口返回结果
    /// </summary>
    public class ScratchCardResult
    {
        public ApiScratchCardModel ScratchCard { get; set; }

        public ApiPrizeModel Prize { get; set; }
    }
}
