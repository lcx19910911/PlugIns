﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PlugInsEntities : DbContext
    {
        public PlugInsEntities()
            : base("name=PlugInsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<UserJoinCounter> UserJoinCounter { get; set; }
        public virtual DbSet<Prize> Prize { get; set; }
        public virtual DbSet<DinnerShop> DinnerShop { get; set; }
        public virtual DbSet<DinnerDish> DinnerDish { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<DinnerOrder> DinnerOrder { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<DinnerCategory> DinnerCategory { get; set; }
        public virtual DbSet<ScratchCard> ScratchCard { get; set; }
    }
}
