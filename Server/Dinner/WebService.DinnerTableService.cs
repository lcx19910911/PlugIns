using Core.Extensions;
using Core.Helper;
using Core.Model;
using  EnumPro;
using Extension;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class WebService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">标题 - 搜索项</param>
        /// <returns></returns>
        public PageList<DinnerTable> Get_DinnerTablePageList(int pageIndex, int pageSize, string name)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerTable.AsQueryable();
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }

                var list = new List<DinnerTable>();
                var count = query.Count();
                query.OrderBy(x => x.Sort).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        list.Add(new DinnerTable()
                        {
                            UNID = x.UNID,
                            CreatedTime = x.CreatedTime,
                            Name = x.Name,
                            MinNum=x.MinNum,
                            MaxNum=x.MaxNum,
                            Sort = x.Sort                  
                        });
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);
            }
        }


        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_DinnerTable(DinnerTable model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                ||model.MinNum>model.MaxNum
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerTable.AsQueryable();
                if (query.Where(x => x.Name.Equals(model.Name) &&x.ShopId.Equals(Client.LoginUser.TargetID) ).Count() != 0)
                    return "餐台标识名称已存在";

                var addEntity =new DinnerTable();
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.Sort = model.Sort;
                addEntity.Name = model.Name;
                addEntity.State = model.State;
                addEntity.CreatedTime = DateTime.Now;
                addEntity.UpdatedTime = DateTime.Now;
                addEntity.ShopId = Client.LoginUser.TargetID;
                addEntity.MinNum = model.MinNum;
                addEntity.MaxNum = model.MaxNum;

                entities.DinnerTable.Add(addEntity);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }


        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update_DinnerTable(DinnerTable model, string unid)
        {
            if (model == null
                 || !model.Name.IsNotNullOrEmpty()
                 || model.MinNum > model.MaxNum
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.DinnerTable.Find(unid);
                if (oldEntity != null)
                {
                    var query = entities.DinnerTable.AsQueryable();
                    if (query.Where(x => x.Name.Equals(model.Name)&&!x.UNID.Equals(unid)&& x.ShopId.Equals(Client.LoginUser.TargetID)).Count() != 0)
                        return "餐台标识名称已存在";

                    oldEntity.Sort = model.Sort;
                    oldEntity.Name = model.Name;
                    oldEntity.MinNum = model.MinNum;
                    oldEntity.MaxNum = model.MaxNum;
                    oldEntity.UpdatedTime = DateTime.Now;
                }
                else
                    return "数据为空";

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }
        /// <summary>
        /// 真删
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public bool Delete_DinnerTable(string unids)
        {
            if (!unids.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.DinnerTable.Where(x => unids.Contains(x.UNID)).ToList().ForEach(x => {
                    entities.DinnerTable.Remove(x);
                });
                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public DinnerTable Find_DinnerTable(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.DinnerTable.Find(unid);
                return entity;
            }
        }
    }
}
