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
    /// <summary>
    /// 点餐订单
    /// </summary>
    public class DinnerOrderService : BaseService, IDinnerOrderService
    {
        public DinnerOrderService()
        {
            base.ContextCurrent = HttpContext.Current;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="state">状态 - 搜索项</param>
        /// <param name="orderNum">订单号 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public PageList<Domain.Dinner.Order.List> Get_DinnerOrderPageList(int pageIndex, int pageSize, int state, string orderNum, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerOrder.AsQueryable().Where(x => x.ShopId.Equals(Client.LoginUser.ShopId));
                if (state != -1)
                {
                    query = query.Where(x => x.State == state);
                }
                if (orderNum.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.OrderNum.Contains(orderNum));
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

                var openIdList = list.Select(x => x.OpenId).ToList();

                var userList = entities.User.Where(x => openIdList.Contains(x.OpenId)).ToList();

                var returnList = new List<Domain.Dinner.Order.List>();
                list.ForEach(x =>
                {
                    returnList.Add(new Domain.Dinner.Order.List()
                    {
                        UNID = x.UNID,
                        NickName = userList.FirstOrDefault(y => y.OpenId.Equals(x.OpenId))?.NickName,
                        CreatedTime = x.CreatedTime,
                        Details=x.Details,
                        OrderNum=x.OrderNum,
                        Remark=x.Remark,
                        State=x.State,
                        TotalPrice=x.TotalPrice
                    });
                });
                

                return CreatePageList(returnList, pageIndex, pageSize, count);
            }
        }



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
                var model = info.DeserializeJson<Domain.Dinner.OrderModel>();

                var user = CacheHelper.Get<Repository.User>("user");
                string shopId = CacheHelper.Get<string>("dinner-shopId");
                if (user==null)
                    return "微信授权过期";
                if (!shopId.IsNotNullOrEmpty())
                    return "店铺不存在";
                var addEntity = new DinnerOrder();
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.OpenId = user.OpenId;
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
                addEntity.OrderNum = string.Format("DC{0}-{1}", DateTime.Now.ToString("yyMMddhhmmss"), addEntity.UNID.SubString(4));
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

        /// <summary>
        /// 获取2两小时内的订单
        /// </summary>
        /// <returns></returns>
        public Domain.Dinner.OrderModel Get_CurrentOrder()
        {
            using (DbRepository entities = new DbRepository())
            {
                var user = CacheHelper.Get<Repository.User>("user");
                string shopId = CacheHelper.Get<string>("dinner-shopId");
                if (user == null)
                    return null;
                if (!shopId.IsNotNullOrEmpty())
                    return null;

                var model = new Domain.Dinner.OrderModel();
                //2小时有效订单
                var limitTime = DateTime.Now.AddHours(-2);
                var order = entities.DinnerOrder.OrderByDescending(x => x.CreatedTime).FirstOrDefault(x => x.OpenId.Equals(user.OpenId) && x.ShopId.Equals(shopId) && x.CreatedTime > limitTime);

                if (order != null)
                {
                    model.Order = order;
                    model.Details = entities.OrderDetails.Where(x => x.OrderId.Equals(order.UNID)).ToList();
                    model.Remark = model.Order.Remark;
                }

                return model;

            }

        }


        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public bool Confirm_DinnerOrder(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                var order = entities.DinnerOrder.Find(unid);
                if (order == null || order.State != (int)DinnerOrderState.Audting)
                    return false;
                order.State = (int)DinnerOrderState.Complate;
                return entities.SaveChanges() > 0 ? true : false;
            }
        }

        /// <summary>
        /// 订单无效
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public bool Invalid_DinnerOrder(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                var order = entities.DinnerOrder.Find(unid);
                if (order == null || order.State != (int)DinnerOrderState.Audting)
                    return false;
                order.State = (int)DinnerOrderState.Invalid;
                return entities.SaveChanges() > 0 ? true : false;
            }
        }
    }
}

