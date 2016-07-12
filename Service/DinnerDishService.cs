using Core.Model;
using Domain;
using Domain.ScratchCard;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
using EnumPro;
using Core.Helper;
using Core.Web;
using IService;
using Extension;
using System.Web;
using Model;

namespace Service
{
    public  class DinnerDishService : BaseService, IDinnerDishService
    {
        public DinnerDishService()
        {
            base.ContextCurrent = HttpContext.Current;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">菜品名 - 搜索项</param>
        /// <param name="categoryId">分类id - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public PageList<Domain.DinnerDish.List> Get_DinnerDishPageList(int pageIndex, int pageSize, string name, string categoryId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerDish.AsQueryable().Where(x => x.ShopId.Equals(Client.LoginUser.ShopId));
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                if (categoryId.IsNotNullOrEmpty() && !categoryId.Equals("0"))
                {
                    query = query.Where(x => x.CategoryId.Contains(categoryId));
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

                var categoryIdList = list.Select(x => x.CategoryId).ToList();
            
                var categoryList = entities.Category.Where(x => categoryIdList.Contains(x.UNID)).ToList();

                var returnList = new List<Domain.DinnerDish.List>();
                list.ForEach(x =>
                {
                    returnList.Add(new Domain.DinnerDish.List()
                    {
                        UNID = x.UNID,
                        CategoryName = categoryList.FirstOrDefault(y => y.UNID.Equals(x.CategoryId))?.Name,
                        CreatedTime = x.CreatedTime,
                        Label = x.Label,
                        Name = x.Name,
                        Price = x.Price,
                        Sort = x.Sort
                    });
                });

                return CreatePageList(returnList, pageIndex, pageSize, count);
            }
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_DinnerDish(DinnerDish model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || !model.CategoryId.IsNotNullOrEmpty()
                || model.Price == 0
                || !model.Image.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerDish.AsQueryable();
                if (query.Where(x => x.Name.Equals(model.Name) && x.CategoryId.Equals(model.CategoryId)).Count() != 0)
                    return "菜品名称已存在";

                model.UNID = Guid.NewGuid().ToString("N");
                model.CreatedTime = DateTime.Now;
                model.UpdatedTime = DateTime.Now;
                model.ShopId = Client.LoginUser.ShopId;

                entities.DinnerDish.Add(model);
                return entities.SaveChanges() > 0 ? "" : "保存错误";
            }

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update_DinnerDish(DinnerDish model, string unid)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || !model.CategoryId.IsNotNullOrEmpty()
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
                    if (query.Where(x => x.Name.Equals(model.Name) && !x.UNID.Equals(unid) && x.CategoryId.Equals(model.CategoryId)).Count() != 0)
                        return "菜品名称已存在";

                    oldEntity.Name = model.Name;
                    oldEntity.Sort = model.Sort;
                    oldEntity.State = model.State;
                    oldEntity.CategoryId = model.CategoryId;
                    oldEntity.Image = model.Image;
                    oldEntity.Price = model.Price;
                    oldEntity.Label = model.Label;
                    oldEntity.Description = model.Description;
                    oldEntity.UpdatedTime = DateTime.Now;
                }
                else
                    return "数据为空";

                return entities.SaveChanges() > 0 ? "" : "保存错误";
            }

        }

        /// <summary>
        /// 删除
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
                    entities.DinnerDish.Remove(x);
                });
                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 查找实体
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

        /// <summary>
        /// 根据菜品分类获取菜品
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        public List<DinnerDish> Get_DishListByCategoryId(string cId)
        {
            using (DbRepository entities = new DbRepository())
            {
                return entities.DinnerDish.Where(x => x.CategoryId.Equals(cId)&&x.State==1).ToList();
            }
        }
    }
}

