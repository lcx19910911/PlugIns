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
        /// <param name="title">标题 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public PageList<Domain.DinnerDish.List> Get_DinnerDishPageList(int pageIndex, int pageSize, string name,string categoryId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerDish.AsQueryable().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                if (categoryId.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.DinnerCategoryId.Contains(categoryId));
                }
                if (createdTimeStart != null)
                {
                    query = query.Where(x => x.CreatedTime >= createdTimeStart);
                }
                if (createdTimeEnd != null)
                {
                    createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                    query = query.Where(x => x.CreatedTime < createdTimeEnd);
                }

                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var returnList = list.AutoMap<DinnerDish, Domain.DinnerDish.List>();

                return CreatePageList(returnList, pageIndex, pageSize, count);
            }
        }


        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_DinnerDish(DinnerDish model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || !model.DinnerCategoryId.IsNotNullOrEmpty()
                || model.Price==0
                || !model.Image.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerDish.AsQueryable();
                if (query.Where(x => x.Name.Equals(model.Name) &&x.DinnerCategoryId.Equals(model.DinnerCategoryId)).Count() != 0)
                    return "菜品名称已存在";

                model.UNID = Guid.NewGuid().ToString("N");
                model.CreatedTime = DateTime.Now;
                model.UpdatedTime = DateTime.Now;
                model.Flag = (long)GlobalFlag.Normal;
                model.ShopId = Client.LoginUser.TargetID;

                entities.DinnerDish.Add(model);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }


        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update_DinnerDish(DinnerDish model, string unid)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || !model.DinnerCategoryId.IsNotNullOrEmpty()
                || model.Price == 0
                || !model.Image.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.DinnerDish.Find(unid);
                if (oldEntity != null)
                {
                    var query = entities.DinnerDish.AsQueryable();
                    if (query.Where(x => x.Name.Equals(model.Name)&&!x.UNID.Equals(unid)&& x.DinnerCategoryId.Equals(model.DinnerCategoryId)).Count() != 0)
                        return "菜品名称已存在";

                    oldEntity.Name = model.Name;
                    oldEntity.Sort = model.Sort;
                    oldEntity.DinnerCategoryId = model.DinnerCategoryId;
                    oldEntity.Image = model.Image;
                    oldEntity.Price = model.Price;
                    oldEntity.Label = model.Label;
                    oldEntity.Description = model.Description;
                    oldEntity.UpdatedTime = DateTime.Now;
                }
                else
                    return "数据为空";

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public bool Delete_DinnerDish(string unids)
        {
            if (!unids.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.DinnerDish.Where(x => unids.Contains(x.UNID)).ToList().ForEach(x => {
                    x.Flag = (x.Flag | (long)GlobalFlag.Removed);
                });
                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public DinnerDish Find_DinnerDish(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.DinnerDish.Find(unid);
                return entity;
            }
        }
    }
}
