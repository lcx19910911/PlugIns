using Core.Extensions;
using Core.Helper;
using Core.Model;
using Domain.UserJoinCounter;
using Enum;
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
        /// <param name="name">活动名</param>
        /// <param name="openId">微信openid</param>
        /// <param name="targetCode">活动类型</param>
        /// <param name="SN">中奖码</param>
        /// <param name="createdTimeStart">中奖时间开始</param>
        /// <param name="createdTimeEnd">中奖时间结束</param>
        /// <returns></returns>
        public PageList<UserJoinCounterModel> Get_UserJoinCounterPageList(int pageIndex, int pageSize, string name, string openId, int targetCode, string targetId, string SN, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.UserJoinCounter.AsQueryable();

                //活动名
                //实例化对象
                ScratchCard scratchCardItem = null;
                Dictionary<string, ScratchCard> scratchCardDic = new Dictionary<string, ScratchCard>();


                //必须先选择活动类型
                if (targetCode != 0)
                {
                    query = query.Where(x => x.TargetCode == targetCode);
                    //判断活动类型
                    if (targetCode == (int)TargetCode.ScratchCard)
                    {
                        //判断是否是指定活动
                        if (targetId.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.TargetID.Equals(targetId));
                            scratchCardItem = entities.ScratchCard.FirstOrDefault(x => x.UNID.Equals(targetId));
                        }
                        else
                        {
                            //遍历活动名
                            var targetEntities = entities.ScratchCard;
                            if (name.IsNotNullOrEmpty())
                            {
                                scratchCardDic = targetEntities.Where(x => x.Name.Contains(name)).ToDictionary(x => x.UNID);
                                var targetIdList = targetEntities.Where(x => x.Name.Contains(name)).Select(x => x.UNID).ToList();
                                query = query.Where(x => targetIdList.Contains(x.TargetID));
                            }
                            else
                            {
                                scratchCardDic = targetEntities.ToDictionary(x => x.UNID);
                            }
                        }
                    }
                }
                else
                     return CreatePageList(new List<UserJoinCounterModel>(), 0, 0); ;


                if (openId.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.OpenID.Contains(openId));
                }
                query = query.Where(x => x.AppId.Equals(this.Client.AppId));

                if (SN.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.SN.Contains(SN));
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

                var list = new List<UserJoinCounterModel>();
                query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        //查找出活动名
                        if (!targetId.IsNotNullOrEmpty())
                        {
                            if (targetCode == (int)TargetCode.ScratchCard)
                            {
                                scratchCardDic.TryGetValue(x.TargetID, out scratchCardItem);
                            }
                        }
                        list.Add(new UserJoinCounterModel()
                        {
                            UNID = x.UNID,
                            Name = scratchCardItem?.Name,
                            CreatedTime = x.CreatedTime,
                            PrizeResult = x.IsPrize == 0 ? "未中奖" : (x.PrizeGrade != 0 ? string.Format("{0}等奖", x.PrizeGrade) : "未中奖"),
                            OpenID = x.OpenID,
                            SN = x.SN,
                            TargetCode = EnumHelper.GetEnumDescription((TargetCode)x.TargetCode),
                            IsCach = x.IsPrize==1 ?(x.IsCach == 1 ? "已兑奖" : "未兑奖"):"",
                            CashTime= x.IsPrize == 1 ? (x.IsCach == 1 ?x.CachTime:null) : null
                        });
                    }
                });

                return CreatePageList(list, pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 兑奖
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public string Cash(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var userJoinCounter = entities.UserJoinCounter.Find(unid);
                if(userJoinCounter==null)
                    return "数据为空";
                if (userJoinCounter.IsPrize != 1)
                    return "该记录未中奖";

                userJoinCounter.IsCach = 1;
                userJoinCounter.CachTime = DateTime.Now;

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }
        }
    }
}

