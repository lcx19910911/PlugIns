using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DbRepository:PlugInsEntities
    {
        public override int SaveChanges()
        {
            try
            {
                var entries = from e in this.ChangeTracker.Entries()
                              where e.State != EntityState.Unchanged
                              select e;   //过滤所有修改了的实体，包括：增加 / 修改 / 删除

                //foreach (var entry in entries)
                //{
                //    //根据不同的操作，编辑不同的logDetails
                //    switch (entry.State)
                //    {
                //        case EntityState.Added:
                //            logDetail.AfterJson = entry.CurrentValues?.ToObject().ToJson();
                //            break;
                //        case EntityState.Deleted:
                //            logDetail.BeforeJson = entry.OriginalValues?.ToObject().ToJson();
                //            break;
                //        case EntityState.Modified:
                //            logDetail.BeforeJson = entry.OriginalValues?.ToObject().ToJson();
                //            logDetail.AfterJson = entry.CurrentValues?.ToObject().ToJson();
                //            break;
                //    }
                //}

                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                //并发冲突数据
                if (ex.GetType() == typeof(DbUpdateConcurrencyException))
                {
                    return -1;
                }               
                return 0;
            }
           
        }
    }

    public class PlugInsEntities : DbContext
    {
        public PlugInsEntities()
           : base("name=PlugInsEntities")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PlugInsEntities>());
        }


        public  DbSet<Category> Category { get; set; }

        public  DbSet<DinnerDish> DinnerDish { get; set; }
        public  DbSet<DinnerOrder> DinnerOrder { get; set; }

        public  DbSet<DinnerShop> DinnerShop { get; set; }

        public  DbSet<Goods> Goods { get; set; }
        public  DbSet<GoodsOrder> GoodsOrder { get; set; }
        public  DbSet<GoodsDetails> GoodsDetails { get; set; }

        public  DbSet<OrderDetails> OrderDetails { get; set; }


        public  DbSet<Person> Person { get; set; }
        public DbSet<Prize> Prize { get; set; }

        public  DbSet<Puzzle> Puzzle { get; set; }

        public  DbSet<Recommend> Recommend { get; set; }
        public  DbSet<ScoreDetails> ScoreDetails { get; set; }


        public  DbSet<ScratchCard> ScratchCard { get; set; }


        public  DbSet<User> User { get; set; }
        public  DbSet<UserJoinCounter> UserJoinCounter { get; set; }
        public  DbSet<UserPuzzle> UserPuzzle { get; set; }
        public  DbSet<UserScore> UserScore { get; set; }
        public  DbSet<UserSign> UserSign { get; set; }
    }

}
