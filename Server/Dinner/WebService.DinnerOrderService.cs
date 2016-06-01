using Core.Extensions;
using Core.Helper;
using Core.Model;
using Core.Web;
using EnumPro;
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
        //public PageList<DinnerOrder> Get_DinnerOrderPageList(int pageIndex, int pageSize, string name)
        //{
        //    using (DbRepository entities = new DbRepository())
        //    {
        //        var query = entities.DinnerOrder.AsQueryable();

        //        var list = new List<DinnerOrder>();
        //        var count = query.Count();
        //        query.OrderBy(x => x.Sort).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
        //        {
        //            if (x != null)
        //            {
        //                list.Add(new DinnerOrder()
        //                {
        //                    UNID = x.UNID,
        //                    CreatedTime = x.CreatedTime,
        //                    Name = x.Name,
        //                    MinNum=x.MinNum,
        //                    MaxNum=x.MaxNum,
        //                    Sort = x.Sort                  
        //                });
        //            }
        //        });

        //        return CreatePageList(list, pageIndex, pageSize, count);
        //    }
        //}


        /// <summary>
        /// 增加订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_DinnerOrder(string info)
        {
            if (!info.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var model= info.DeserializeJson<Domain.Dinner.OrderModel>();

                string openId = CacheHelper.Get<string>("dinner-openId");
                string shopId = CacheHelper.Get<string>("dinner-shopId");
                if (!openId.IsNotNullOrEmpty())
                    return "微信授权过期";
                if (!shopId.IsNotNullOrEmpty())
                    return "店铺不存在";
                var addEntity = new DinnerOrder();
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.OpenId = openId;
                addEntity.Details = "";

                decimal totalPrice = 0;
                model.Details.ForEach(x => {
                    entities.OrderDetails.Add(new OrderDetails()
                    {
                        UNID = Guid.NewGuid().ToString("N"),
                        DishId = x.DishId,
                        DishName = x.DishName,
                        Number = x.Number,
                        OrderId = addEntity.UNID,
                        Price = x.Price,
                    }); 
                    totalPrice += (x.Price * x.Number);
                    addEntity.Details = string.Format("{0} {1}X{2}份 ", addEntity.Details, x.DishName, x.Number);
                });

                addEntity.TotalPrice = totalPrice;
                addEntity.CreatedTime = DateTime.Now;
                addEntity.UpdatedTime = DateTime.Now;
                addEntity.State = (int)DinnerOrderState.Audting;
                addEntity.ShopId = shopId;
                addEntity.Remark = model.Remark;

                entities.DinnerOrder.Add(addEntity);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }


        public Domain.Dinner.OrderModel Get_CurrentOrder()
        {
            using (DbRepository entities = new DbRepository())
            {
                string openId = CacheHelper.Get<string>("dinner-openId");
                string shopId = CacheHelper.Get<string>("dinner-shopId");
                if (!openId.IsNotNullOrEmpty())
                    return null; 
                if (!shopId.IsNotNullOrEmpty())
                    return null;

                var model =new Domain.Dinner.OrderModel();
                //15分钟内有效订单
                var limitTime = DateTime.Now.AddHours(-2);
                var order = entities.DinnerOrder.OrderByDescending(x=>x.CreatedTime).FirstOrDefault(x => x.OpenId.Equals(openId) && x.ShopId.Equals(shopId) && x.CreatedTime > limitTime);

                if(order != null)
                {
                    model.Order = order;
                    model.Details = entities.OrderDetails.Where(x => x.OrderId.Equals(order.UNID)).ToList();
                    model.Remark = model.Order.Remark;
                }

                return model;

            }

        }
    }
}
