﻿using Core.Extensions;
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
        public PageList<DinnerCategory> Get_DinnerCategoryPageList(int pageIndex, int pageSize, string name, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerCategory.AsQueryable().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
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

                var list = new List<DinnerCategory>();
                var count = query.Count();
                query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        list.Add(new DinnerCategory()
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
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_DinnerCategory(DinnerCategory model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerCategory.AsQueryable();
                if (query.Where(x => x.Name.Equals(model.Name) &&x.ShopId.Equals(Client.LoginUser.TargetID)).Count() != 0)
                    return "分类名称已存在";

                var addEntity =new DinnerCategory();
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.Sort = model.Sort;
                addEntity.Name = model.Name;
                addEntity.CreatedTime = DateTime.Now;
                addEntity.UpdatedTime = DateTime.Now;
                addEntity.Flag = (long)GlobalFlag.Normal;
                addEntity.ShopId = Client.LoginUser.TargetID;

                entities.DinnerCategory.Add(addEntity);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }


        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update_DinnerCategory(DinnerCategory model, string unid)
        {
            if (model == null
                 || !model.Name.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.DinnerCategory.Find(unid);
                if (oldEntity != null)
                {
                    var query = entities.DinnerCategory.AsQueryable();
                    if (query.Where(x => x.Name.Equals(model.Name)&&!x.UNID.Equals(unid)&& x.ShopId.Equals(Client.LoginUser.TargetID)).Count() != 0)
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
        /// 
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
                entities.DinnerCategory.Where(x => unids.Contains(x.UNID)).ToList().ForEach(x => {
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
        public DinnerCategory Find_DinnerCategory(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.DinnerCategory.Find(unid);
                return entity;
            }
        }


        /// <summary>
        /// 获取选择项
        /// </summary>
        /// <param name="dinnerCategoryId"></param>
        /// <returns></returns>
        public List<SelectItem> Get_DinnerCategorySelectItem(string dinnerCategoryId)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<SelectItem> list = new List<SelectItem>();
                entities.DinnerCategory.Where(x => x.Flag == 0).ToList().ForEach(x =>
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
    }
}
