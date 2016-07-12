using Core.Extensions;
using Core.Helper;
using Core.Model;
using EnumPro;
using Extension;
using IService;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service
{
    /// <summary>
    /// 菜品分类
    /// </summary>
    public class CategoryService: BaseService, ICategoryService
    {
        public CategoryService()
        {
            base.ContextCurrent = HttpContext.Current;
        }


        #region 菜品分类


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">分类名 - 搜索项</param>
        /// <returns></returns>
        public PageList<Category> Get_DinnerCategoryPageList(int pageIndex, int pageSize, string name)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Category.AsQueryable().Where(x=>x.ShopId.Equals(Client.LoginUser.ShopId));
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }

                var list = new List<Category>();
                var count = query.Count();
                query.OrderByDescending(x => x.Sort).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        list.Add(new Category()
                        {
                            UNID = x.UNID,
                            CreatedTime = x.CreatedTime,
                            Name = x.Name,
                            UpdatedTime = x.UpdatedTime,
                            Sort = x.Sort                  
                        });
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);
            }
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_DinnerCategory(Category model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Category.AsQueryable();
                if (query.Where(x => x.Name.Equals(model.Name) &&x.ShopId.Equals(Client.LoginUser.ShopId)).Count() != 0)
                    return "分类名称已存在";

                var addEntity =new Category();
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.Sort = model.Sort;
                addEntity.Name = model.Name;
                addEntity.CreatedTime = DateTime.Now;
                addEntity.UpdatedTime = DateTime.Now;
                addEntity.ShopId = Client.LoginUser.ShopId;

                entities.Category.Add(addEntity);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update_DinnerCategory(Category model, string unid)
        {
            if (model == null
                 || !model.Name.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.Category.Find(unid);
                if (oldEntity != null)
                {
                    var query = entities.Category.AsQueryable();
                    if (query.Where(x => x.Name.Equals(model.Name)&&!x.UNID.Equals(unid)&& x.ShopId.Equals(Client.LoginUser.ShopId)).Count() != 0)
                        return "分类名称已存在";

                    oldEntity.Sort = model.Sort;
                    oldEntity.Name = model.Name;
                    oldEntity.UpdatedTime = DateTime.Now;
                }
                else
                    return "数据为空";

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public bool Delete_DinnerCategory(string unids)
        {
            if (!unids.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.Category.Where(x => unids.Contains(x.UNID)).ToList().ForEach(x =>
                {
                    entities.DinnerDish.Where(y => y.CategoryId.Equals(x.UNID)).ToList().ForEach(y =>
                    {
                        entities.DinnerDish.Remove(y);
                    });
                    entities.Category.Remove(x);
                });
                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public Category Find_DinnerCategory(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.Category.Find(unid);
                return entity;
            }
        }


        /// <summary>
        /// 获取分类下拉框集合
        /// </summary>
        /// <param name="dinnerCategoryId">分类id</param>
        /// <returns></returns>
        public List<SelectItem> Get_DinnerCategorySelectItem(string dinnerCategoryId)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<SelectItem> list = new List<SelectItem>();
                entities.Category.Where(x=>x.ShopId.Equals(Client.LoginUser.ShopId)).ToList().ForEach(x =>
                {
                    list.Add(new SelectItem()
                    {
                        Selected = x.UNID.Equals(dinnerCategoryId),
                        Text = x.Name,
                        Value = x.UNID
                    });
                });
                return list;

            }
        }

        /// <summary>
        /// 获取店家的分类
        /// </summary>
        /// <param name="dinnerCategoryId"></param>
        /// <returns></returns>
        public List<SelectItem> Get_ItemByShopId(string shopId)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<SelectItem> list = new List<SelectItem>();
                entities.Category.Where(x =>x.ShopId.Equals(shopId)).ToList().ForEach(x =>
                {
                    list.Add(new SelectItem()
                    {
                        Text = x.Name,
                        Value = x.UNID
                    });
                });
                return list;

            }
        }


        #endregion

        #region 商品分类


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">分类名 - 搜索项</param>
        /// <returns></returns>
        public PageList<Category> Get_MallCategoryPageList(int pageIndex, int pageSize, string name)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Category.AsQueryable().Where(x => x.PersonId.Equals(Client.LoginUser.UNID));
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }

                var list = new List<Category>();
                var count = query.Count();
                query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        list.Add(new Category()
                        {
                            UNID = x.UNID,
                            CreatedTime = x.CreatedTime,
                            Name = x.Name,
                            UpdatedTime = x.UpdatedTime,
                            Sort = x.Sort,
                            Image=x.Image
                        });
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);
            }
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_MallCategory(Category model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Category.AsQueryable();
                if (query.Where(x => x.Name.Equals(model.Name) && x.PersonId.Equals(Client.LoginUser.UNID)).Count() != 0)
                    return "分类名称已存在";

                var addEntity = new Category();
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.Sort = model.Sort;
                addEntity.Name = model.Name;
                addEntity.CreatedTime = DateTime.Now;
                addEntity.UpdatedTime = DateTime.Now;
                addEntity.Image = model.Image;
                addEntity.PersonId = Client.LoginUser.UNID;

                entities.Category.Add(addEntity);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update_MallCategory(Category model, string unid)
        {
            if (model == null
                 || !model.Name.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.Category.Find(unid);
                if (oldEntity != null)
                {
                    var query = entities.Category.AsQueryable();
                    if (query.Where(x => x.Name.Equals(model.Name) && !x.UNID.Equals(unid) && x.PersonId.Equals(Client.LoginUser.UNID)).Count() != 0)
                        return "分类名称已存在";

                    oldEntity.Sort = model.Sort;
                    oldEntity.Name = model.Name;
                    oldEntity.Image = model.Image;
                    oldEntity.UpdatedTime = DateTime.Now;
                }
                else
                    return "数据为空";

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public bool Delete_MallCategory(string unids)
        {
            if (!unids.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.Category.Where(x => unids.Contains(x.UNID)).ToList().ForEach(x =>
                {

                    entities.Category.Remove(x);
                    //删除推荐
                    var reconmend = entities.Recommend.FirstOrDefault(z => z.TargetID.Equals(x.UNID));
                    if (reconmend != null)
                        entities.Recommend.Remove(reconmend);
                });
                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public Category Find_MallCategory(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.Category.Find(unid);
                return entity;
            }
        }


        /// <summary>
        /// 获取分类下拉框集合
        /// </summary>
        /// <param name="categoryId">分类id</param>
        /// <returns></returns>
        public List<SelectItem> Get_MallCategorySelectItem(string categoryId)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<SelectItem> list = new List<SelectItem>();
                entities.Category.Where(x => x.PersonId.Equals(Client.LoginUser.UNID)).ToList().ForEach(x =>
                {
                    list.Add(new SelectItem()
                    {
                        Selected = x.UNID.Equals(categoryId),
                        Text = x.Name,
                        Value = x.UNID
                    });
                });
                return list;

            }
        }

        /// <summary>
        /// 获取店家的分类
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public List<SelectItem> Get_ItemByPersonId(string personId)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<SelectItem> list = new List<SelectItem>();
                entities.Category.Where(x => x.PersonId.Equals(personId)).ToList().ForEach(x =>
                {
                    list.Add(new SelectItem()
                    {
                        Text = x.Name,
                        Value = x.UNID
                    });
                });
                return list;

            }
        }

        /// <summary>
        /// 获取店家的分类
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public List<Category> Get_ListByPersonId(string personId)
        {
            using (DbRepository entities = new DbRepository())
            {
                return entities.Category.Where(x => x.PersonId.Equals(personId)).ToList();

            }
        }

        #endregion
    }
}
