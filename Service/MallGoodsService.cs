﻿using Core.Model;
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
using Domain.Mall.Goods;
using Model;

namespace Service
{
    public class MallGoodsService : BaseService, IMallGoodsService
    {
        public MallGoodsService()
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
        /// <returns></returns> /// <returns></returns>
        public PageList<Domain.Mall.Goods.List> Get_MallGoodsPageList(int pageIndex, int pageSize, string name, string categoryId, DateTime? createdTimeStart, DateTime? createdTimeEnd, bool isRecommand)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Goods.AsQueryable().Where(x => x.PersonId.Equals(Client.LoginUser.UNID)).Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0);
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


                if (isRecommand)
                {
                    var goodsIdList = entities.Recommend.Where(x => x.TargetCode == (int)TargetCode.Goods && x.PersonId.Equals(Client.LoginUser.UNID)).Select(x => x.TargetID).ToList();
                    query = query.Where(x => !goodsIdList.Contains(x.UNID));
                }

                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var categoryIdList = list.Select(x => x.CategoryId).ToList();

                var categoryList = entities.Category.Where(x => categoryIdList.Contains(x.UNID)).ToList();

                var returnList = new List<Domain.Mall.Goods.List>();
                list.ForEach(x =>
                {
                    returnList.Add(new Domain.Mall.Goods.List()
                    {
                        UNID = x.UNID,
                        CategoryName = categoryList.FirstOrDefault(y => y.UNID.Equals(x.CategoryId))?.Name,
                        CreatedTime = x.CreatedTime,
                        Name = x.Name,
                        SellingPrice=x.SellingPrice,
                        StockNum=x.StockNum,
                        OngoingTime=x.OngoingTime,
                        OverTime=x.OverTime
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
        public string Add_MallGoods(Goods model, string DetailsImage, string DetailsSort)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || !model.CategoryId.IsNotNullOrEmpty()
                || model.OverTime == null
                || model.OngoingTime == null
                )
                return "数据为空";
            if (model.OngoingTime < DateTime.Now)
                return "开始时间需比晚于当前时间";
            if (model.OverTime < model.OngoingTime)
                return "结束时间必须大于当前时间和开始时间";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Goods.AsQueryable();
                if (query.Where(x => x.Name.Equals(model.Name) && x.CategoryId.Equals(model.CategoryId)).Count() != 0)
                    return "商品名称已存在";

                model.UNID = Guid.NewGuid().ToString("N");
                model.CreatedTime = DateTime.Now;
                model.UpdatedTime = DateTime.Now;
                model.PersonId = Client.LoginUser.UNID;
                model.Flag = (long)GlobalFlag.Normal;
                model.SurplusNum = model.StockNum;
                if (DetailsImage.IsNotNullOrEmpty())
                {
                    var imageList = DetailsImage.Split(',');
                    var sortList = DetailsSort.Split(',');
                    if (imageList.Length != sortList.Length)
                    {
                        return "数据异常";
                    }
                    for (int i = 0; i < imageList.Length; i++)
                    {
                        entities.GoodsDetails.Add(new GoodsDetails()
                        {
                            UNID = Guid.NewGuid().ToString("N"),
                            GoodsId = model.UNID,
                            Image = imageList[i],
                            Sort = sortList[i].GetInt()
                        });
                    }
                }

                entities.Goods.Add(model);
                return entities.SaveChanges() > 0 ? "" : "保存错误";
            }

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update_MallGoods(Goods model, string DetailsImage, string DetailsSort)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || !model.CategoryId.IsNotNullOrEmpty()
                || model.OverTime == null
                || model.OngoingTime == null
                )
                return "数据为空";
            if (model.OverTime < model.OngoingTime)
                return "结束时间必须大于当前时间和开始时间";
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.Goods.Find(model.UNID);
                if (oldEntity != null)
                {
                    var query = entities.Goods.AsQueryable();
                    if (query.Where(x => x.Name.Equals(model.Name) && !x.UNID.Equals(model.UNID) && x.CategoryId.Equals(model.CategoryId)).Count() != 0)
                        return "商品名称已存在";

                    oldEntity.Name = model.Name;
                    oldEntity.CategoryId = model.CategoryId;
                    oldEntity.StockNum = model.StockNum;
                    oldEntity.SurplusNum = model.SurplusNum;
                    oldEntity.OverTime = model.OverTime;
                    oldEntity.OngoingTime = model.OngoingTime;
                    oldEntity.OriginalPrice = model.OriginalPrice;
                    oldEntity.SellingPrice = model.SellingPrice;
                    oldEntity.ScoreNum = model.ScoreNum;
                    oldEntity.UpdatedTime = DateTime.Now;

                    var detailsImageList = entities.GoodsDetails.Where(x => x.GoodsId.Equals(model.UNID)).ToList();

                    //判断是否有明细页
                    if (DetailsImage.IsNotNullOrEmpty())
                    {
                        var imageList = DetailsImage.Split(',');
                        var sortList = DetailsSort.Split(',');

                        if (imageList.Length != sortList.Length)
                        {
                            return "数据异常";
                        }
                        //判断原来是否有
                        if (detailsImageList != null && detailsImageList.Count != 0)
                        {
                            //删除的图片
                            var deleteList = detailsImageList.Where(x => !imageList.Contains(x.Image)).ToList();
                            deleteList.ForEach(x => entities.GoodsDetails.Remove(x));

                            //修改得分图片
                            var updateList = detailsImageList.Where(x => imageList.Contains(x.Image)).ToList();
                            updateList.ForEach(x =>
                            {
                                var index = imageList.IndexOf(y => y.Equals(x.Image));
                                x.Sort = sortList[index].GetInt();
                            }
                            );

                            //新增的图片
                            var oldImageList = detailsImageList.Select(x => x.Image).ToList();
                            var addList = imageList.Where(x => !oldImageList.Contains(x)).ToList();
                            addList.ForEach(x => {
                                var index = imageList.IndexOf(y => y.Equals(x));

                                entities.GoodsDetails.Add(new GoodsDetails()
                                {
                                    UNID = Guid.NewGuid().ToString("N"),
                                    GoodsId = model.UNID,
                                    Image = imageList[index],
                                    Sort = sortList[index].GetInt()
                                });
                            });
                        }
                        else
                        {

                            for (int i = 0; i < imageList.Length; i++)
                            {
                                entities.GoodsDetails.Add(new GoodsDetails()
                                {
                                    UNID = Guid.NewGuid().ToString("N"),
                                    GoodsId = model.UNID,
                                    Image = imageList[i],
                                    Sort = sortList[i].GetInt()
                                });
                            }
                        }
                    }

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
        public bool Delete_MallGoods(string unids)
        {
            if (!unids.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.Goods.Where(x => unids.Contains(x.UNID)).ToList().ForEach(y =>
                {
                    y.Flag = (y.Flag | (long)GlobalFlag.Removed);

                    //删除推荐
                    var reconmend = entities.Recommend.FirstOrDefault(x => x.TargetID.Equals(y.UNID));
                    if(reconmend!=null)
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
        public GoodsModel Find_MallGoods(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                GoodsModel model = new GoodsModel();
                var entity = entities.Goods.Find(unid);
                if (entity != null)
                {
                    model.Goods = entity;
                    model.GoodsDetails = entities.GoodsDetails.Where(x => x.GoodsId.Equals(unid)).OrderBy(x=>x.Sort).ToList();
                }
                return model;
            }
        }

        /// <summary>
        /// 根据分类获取商品
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        public List<Goods> Get_GoodsListByCategoryId(string cId,string personId)
        {
            using (DbRepository entities = new DbRepository())
            {
                if(cId.IsNotNullOrEmpty())
                    return entities.Goods.Where(x => x.CategoryId.Equals(cId) && (x.Flag & (long)GlobalFlag.Removed) == 0&&x.PersonId.Equals(personId)).ToList();
                else
                    return entities.Goods.Where(x =>(x.Flag & (long)GlobalFlag.Removed) == 0&x.PersonId.Equals(personId)).ToList();
            }
        }
    }
}

