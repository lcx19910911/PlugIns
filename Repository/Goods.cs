//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Goods
    {
        public string UNID { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public int StockNum { get; set; }
        public int SurplusNum { get; set; }
        public System.DateTime OverTime { get; set; }
        public System.DateTime OngoingTime { get; set; }
        public Nullable<decimal> OriginalPrice { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
        public Nullable<int> ScoreNum { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public System.DateTime UpdatedTime { get; set; }
        public Nullable<long> Flag { get; set; }
        public string OpenId { get; set; }
    }
}
