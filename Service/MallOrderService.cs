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
using Domain.API;
using System.Threading;

namespace Service
{
    /// <summary>
    /// 商品订单
    /// </summary>
    public class MallOrderService : BaseService, IMallOrderService
    {
        public MallOrderService()
        {
            base.ContextCurrent = HttpContext.Current;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">商品名 - 搜索项</param>
        /// <param name="nickName">用户昵称 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public PageList<Domain.Mall.Order.OrderModel> Get_OrderPageList(int pageIndex, int pageSize, string unid, string name, string nickName, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.GoodsOrder.AsQueryable().Where(x => x.PersonId.Equals(Client.LoginUser.UNID));

                var goodsList = entities.Goods.Where(x => x.Name.Contains(name)).ToList();
                var openList = entities.User.Where(x => x.NickName.Contains(nickName)).ToList();

                if (name.IsNotNullOrEmpty())
                {
                    var goodsIdList = goodsList.Select(x => x.UNID).ToList();
                    query = query.Where(x => goodsIdList.Contains(x.GoodsId));
                }
                if (nickName.IsNotNullOrEmpty())
                {
                    var openIdList = openList.Select(x => x.OpenId).ToList();
                    query = query.Where(x => openIdList.Contains(x.OpenId));
                }

                if (unid.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.UNID.Contains(unid));
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

                var list = new List<Domain.Mall.Order.OrderModel>();
                var count = query.Count();
                query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        list.Add(new Domain.Mall.Order.OrderModel()
                        {
                            UNID = x.UNID,
                            GoodsName = goodsList.FirstOrDefault(y => y.UNID.Equals(x.GoodsId))?.Name,
                            CreatedTime = x.CreatedTime,
                            AllPrice = x.AllPrice,
                            Count = x.Count,
                            GoodsId = x.GoodsId,
                            NickName = openList.FirstOrDefault(y => y.OpenId.Equals(x.OpenId))?.NickName,
                            PersonId = x.PersonId,
                            OpenId = x.OpenId,
                            ScoreNum = x.ScoreNum
                        });
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);
            }
        }

        /// <summary>
        /// 获取用户所有的刮刮卡
        /// </summary>
        /// <returns></returns>
        public List<Tuple<GoodsOrder, Goods>> Get_AllOrderList(string openId, string personId)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<Tuple<GoodsOrder, Goods>> list = new List<Tuple<GoodsOrder, Goods>>();
                var orderList=entities.GoodsOrder.Where(x => x.PersonId.Equals(personId)&&x.OpenId.Equals(openId)).ToList();
                var goodsIdList = orderList.Select(x=>x.GoodsId).ToList();
                var goodsList = entities.Goods.Where(x => goodsIdList.Contains(x.UNID)).ToList();
                orderList.ForEach(x =>
                {
                    list.Add(new Tuple<GoodsOrder, Goods>(
                            x,
                            goodsList.FirstOrDefault(y => y.UNID.Equals(x.GoodsId))
                        ));
                });

                return list;
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_Order(GoodsOrder model)
        {
            if (model == null
                || !model.GoodsId.IsNotNullOrEmpty()
                || model.Count == 0
                )
                return "数据错误";
            var user = CacheHelper.Get<Repository.User>("user");
            var person = CacheHelper.Get<Person>("person");
            if (user == null || person == null)
                return "身份验证过期";
            using (DbRepository entities = new DbRepository())
            {
                var userEntity = entities.User.Find(user.OpenId);
                if (userEntity == null)
                    return "用户不存在";

                var goods = entities.Goods.Find(model.GoodsId);
                if (goods == null)
                    return "数据错误";

                if (goods.SurplusNum < model.Count)
                    return "商品库存不足";

                //商品库存减少
                goods.SurplusNum -= (int)model.Count;
                //积分总计
                model.ScoreNum = model.Count * goods.ScoreNum;

                var userScore = entities.UserScore.FirstOrDefault(x => x.OpenId.Equals(user.OpenId) && x.PersonId.Equals(person.UNID));
                if (userScore == null || userScore.Score < model.ScoreNum)
                    return "用户积分不足";

                //扣减用户积分
                userScore.Score -= (int)model.ScoreNum;

                //消费积分
                var scoreDetials = new ScoreDetails()
                {
                    UNID = Guid.NewGuid().ToString("N"),
                    OpenId = user.OpenId,
                    CreatedTime = DateTime.Now,
                    Description = string.Format("购买商品：{0}，数量{1}，消费积分{2}", goods.Name, model.Count, model.Count * goods.ScoreNum),
                    IsAdd = (int)YesOrNoCode.No,
                    Value = (int)(model.Count * goods.ScoreNum),
                    Type = (int)ScoreType.Mall,
                    PersonId = person.UNID
                };
                entities.ScoreDetails.Add(scoreDetials);


                model.UNID = Guid.NewGuid().ToString("N");
                model.OpenId = user.OpenId;
                model.PersonId = person.UNID;
                model.AllPrice = model.Count * goods.SellingPrice;
                model.CreatedTime = DateTime.Now;

                entities.GoodsOrder.Add(model);

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }
        }
    }
}

