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
    
    public partial class Puzzle
    {
        public string UNID { get; set; }
        public string PersonId { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public string Image { get; set; }
        public System.DateTime UpdatedTime { get; set; }
        public long Flag { get; set; }
        public int DifficultyType { get; set; }
        public string Description { get; set; }
        public int IsBindScore { get; set; }
        public int Score { get; set; }
        public string BindTitle { get; set; }
        public string BindName { get; set; }
        public string BindUrl { get; set; }
        public System.DateTime OverTime { get; set; }
        public System.DateTime OngoingTime { get; set; }
    }
}