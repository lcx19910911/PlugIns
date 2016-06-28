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

namespace Service
{
    public  class MallGoodsService : BaseService, IMallGoodsService
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
        /// <returns></returns>
        public PageList<Goods> Get_MallGoodsPageList(int pageIndex, int pageSize, string name, string categoryId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
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

                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return CreatePageList(list, pageIndex, pageSize, count);
            }
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_MallGoods(Goods model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || !model.CategoryId.IsNotNullOrEmpty()
                || model.OverTime == null
                || model.OngoingTime==null
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
                model.PersonId = Client.LoginUser.ShopId;
                model.Flag = (long)GlobalFlag.Normal;

                entities.Goods.Add(model);
                return entities.SaveChanges() > 0 ? "" : "保存错误";
            }

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update_MallGoods(Goods model, string unid)
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
                var oldEntity = entities.Goods.Find(unid);
                if (oldEntity != null)
                {
                    var query = entities.Goods.AsQueryable();
                    if (query.Where(x => x.Name.Equals(model.Name) && !x.UNID.Equals(unid) && x.CategoryId.Equals(model.CategoryId)).Count() != 0)
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
                entities.Goods.Where(x => unids.Contains(x.UNID)).ToList().ForEach(y => {
                    y.Flag = (y.Flag | (long)GlobalFlag.Removed);
                });
                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public Goods Find_MallGoods(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.Goods.Find(unid);
                return entity;
            }
        }

        /// <summary>
        /// 根据分类获取商品
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        public List<Goods> Get_GoodsListByCategoryId(string cId)
        {
            using (DbRepository entities = new DbRepository())
            {
                return entities.Goods.Where(x => x.CategoryId.Equals(cId)&&(x.Flag & (long)GlobalFlag.Removed) == 0).ToList();
            }
        }
    }
}

