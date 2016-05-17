using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                Core.Util.LogHelper.WriteException(ex);
                return 0;
            }
           
        }
    }
}
