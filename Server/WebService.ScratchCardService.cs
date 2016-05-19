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
using Extension;
using Enum;
using Core.Helper;
using Core.Web;

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
        public PageList<ScratchCardModel> Get_ScratchCardPageList(int pageIndex, int pageSize, string title, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.ScratchCard.AsQueryable().Where(x => (x.Flag&(long)GlobalFlag.Removed)==0);
                if (title.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(title));
                }
                query = query.Where(x => x.AppId.Equals(this.Client.AppId));
                if (createdTimeStart != null)
                {
                    query = query.Where(x => x.OngoingTime >= createdTimeStart);
                }
                if (createdTimeEnd != null)
                {
                    createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                    query = query.Where(x => x.OverTime < createdTimeEnd);
                }

                var list = new List<ScratchCardModel>();
                query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        list.Add(new ScratchCardModel()
                        {
                            UNID = x.UNID,
                            CreatedTime = x.CreatedTime,
                            KeyWord = x.KeyWord,
                            Name = x.Name,
                            OngoingTime = x.OngoingTime,
                            OverTime = x.OverTime,
                            UpdatedTime = x.UpdatedTime
                        });
                    }
                });

                return CreatePageList(list, pageIndex, pageSize);
            }
        }


        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:保存出错 -1:开始时间小于当前时间  -2:结束时间必须大于当前时间和开始时间  -3:个人抽奖次数总计要大于或等于每天次数限制</returns>
        public string Add_ScratchCard(Domain.ScratchCard.Update model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || !model.OngoingTitle.IsNotNullOrEmpty()
                || !model.OngoingTitle.IsNotNullOrEmpty()
                || !model.PreheatingTitle.IsNotNullOrEmpty()
                || !model.OverTitle.IsNotNullOrEmpty()
                || model.OngoingTime == null
                || model.OverTime == null
                || !model.RepeatNotice.IsNotNullOrEmpty()
                || model.PreheatingImageFile == null
                || model.OngoingImageFile == null
                || model.OverImageFile == null
                || !model.OnePrize.IsNotNullOrEmpty()
                || !model.TwoPrize.IsNotNullOrEmpty()
                || !model.ThreePrize.IsNotNullOrEmpty()
                || !model.OnePrize.IsNotNullOrEmpty()
                )
                return "数据为空";
            if (model.OngoingTime < DateTime.Now.AddMinutes(20))
                return "开始时间需比当前时间晚20分钟";
            if (model.OverTime < model.OngoingTime || model.OverTime < DateTime.Now)
                return "结束时间必须大于当前时间和开始时间";
            using (DbRepository entities = new DbRepository())
            {
                var addEntity = model.AutoMap<Domain.ScratchCard.Update, ScratchCard>();
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.CreatedTime = DateTime.Now;
                addEntity.UpdatedTime = DateTime.Now;
                addEntity.Flag = (long)GlobalFlag.Normal;
                addEntity.PreheatingImage = UploadHelper.Save(model.PreheatingImageFile, "ScratchCard");
                addEntity.OngoingImage = UploadHelper.Save(model.OngoingImageFile, "ScratchCard");
                addEntity.OverImage = UploadHelper.Save(model.OverImageFile, "ScratchCard");
                addEntity.AppId = this.Client.AppId;
                entities.ScratchCard.Add(addEntity);

                var addPrizeEntity = model.AutoMap<Domain.ScratchCard.Update, Prize>();
                if (model.AllCountLimt < model.DayLimt)
                    return "个人抽奖次数总计要大于或等于每天次数限制";

                if (model.ExpectedPeopleCount < (model.OnePrizeCount+model.TwoPrizeCount+model.ThreePrizeCount))
                    return "预计参与人数须大于奖品的总数";

                addPrizeEntity.UNID = Guid.NewGuid().ToString("N");
                addPrizeEntity.TargetCode = (int)TargetCode.ScratchCard;
                addPrizeEntity.TargetID = addEntity.UNID;
                addPrizeEntity.AllCount = addPrizeEntity.OnePrizeCount + addPrizeEntity.TwoPrizeCount + addPrizeEntity.ThreePrizeCount;
                entities.Prize.Add(addPrizeEntity);

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }


        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:保存出错 -1:开始时间小于当前时间  -2:结束时间必须大于当前时间和开始时间  -3:个人抽奖次数总计要大于或等于每天次数限制</returns>
        public string Update_ScratchCard(Domain.ScratchCard.Update model, string unid)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || !model.OngoingTitle.IsNotNullOrEmpty()
                || !model.OngoingTitle.IsNotNullOrEmpty()
                || !model.PreheatingTitle.IsNotNullOrEmpty()
                || !model.OverTitle.IsNotNullOrEmpty()
                || model.OngoingTime == null
                || model.OverTime == null
                || !model.RepeatNotice.IsNotNullOrEmpty()
                || !model.PreheatingImage.IsNotNullOrEmpty()
                || !model.OngoingImage.IsNotNullOrEmpty()
                || !model.OverImage.IsNotNullOrEmpty()
                || !model.OnePrize.IsNotNullOrEmpty()
                || !model.TwoPrize.IsNotNullOrEmpty()
                || !model.ThreePrize.IsNotNullOrEmpty()
                || !model.OnePrize.IsNotNullOrEmpty()
                )
                return "数据为空";
            if (model.OngoingTime < DateTime.Now.AddMinutes(20))
                return "开始时间需比当前时间晚20分钟";
            if (model.OverTime < model.OngoingTime || model.OverTime < DateTime.Now)
                return "结束时间必须大于当前时间和开始时间";
            if (model.AllCountLimt < model.DayLimt)
                return "个人抽奖次数总计要大于或等于每天次数限制";
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.ScratchCard.Find(unid);
                if (oldEntity != null)
                {
                    model.AutoMap<Domain.ScratchCard.Update, ScratchCard>(oldEntity);

                    if (model.PreheatingImageFile != null)
                        oldEntity.PreheatingImage = UploadHelper.Save(model.PreheatingImageFile, "ScratchCard");
                    if (model.OngoingImageFile != null)
                        oldEntity.OngoingImage = UploadHelper.Save(model.OngoingImageFile, "ScratchCard");
                    if (model.OverImageFile != null)
                        oldEntity.OverImage = UploadHelper.Save(model.OverImageFile, "ScratchCard");

                    var oldPrizeEntity = entities.Prize.Where(x => x.TargetCode == (int)TargetCode.ScratchCard && x.TargetID.Equals(unid)).FirstOrDefault();
                    if (oldPrizeEntity != null)
                    {
                        oldPrizeEntity.OnePrize = model.OnePrize;
                        oldPrizeEntity.TwoPrize = model.TwoPrize;
                        oldPrizeEntity.ThreePrize = model.ThreePrize;
                        oldPrizeEntity.IsShowCount = model.IsShowCount;
                        oldPrizeEntity.ExpectedPeopleCount = model.ExpectedPeopleCount;
                        oldPrizeEntity.DayLimt = model.DayLimt;
                        oldPrizeEntity.AllCountLimt = model.AllCountLimt;

                        oldPrizeEntity.AllCount = oldPrizeEntity.OnePrizeCount + oldPrizeEntity.TwoPrizeCount + oldPrizeEntity.ThreePrizeCount;
                    }
                    else
                        return "数据为空";
                }
                else
                    return "数据为空";

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }

        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public Domain.ScratchCard.Update Find_ScratchCard(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                Domain.ScratchCard.Update model = new Update();
                var scratchScardEntity = entities.ScratchCard.Find(unid);
                var prizeEntity = entities.Prize.Where(x => x.TargetCode == (int)TargetCode.ScratchCard && x.TargetID.Equals(unid)).FirstOrDefault();
                if (prizeEntity != null)
                {
                    prizeEntity.AutoMap<Prize, Domain.ScratchCard.Update>(model);
                }
                if (scratchScardEntity != null)
                    scratchScardEntity.AutoMap<ScratchCard, Domain.ScratchCard.Update>(model);


                return model;
            }
        }

        /// <summary>
        /// 参与刮刮卡活动
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public PrizeResult Do_ScratchCard(string unid)
        {
            PrizeResult result = new PrizeResult();
            result.IsPrize = false;
            if (!unid.IsNotNullOrEmpty())
            {
                result.Result = "数据为空";
                return result;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                var scratchScardEntity = entities.ScratchCard.Find(unid);
                //奖品设置
                var prizeEntity = entities.Prize.Where(x => x.TargetCode == (int)TargetCode.ScratchCard && x.TargetID.Equals(unid)).FirstOrDefault();
                if (prizeEntity == null || scratchScardEntity == null)
                {
                    result.Result = "数据为空";
                    return result;
                }

                if (scratchScardEntity.OngoingTime > DateTime.Now)
                {
                    result.Result = "还没到活动时间，请耐心等待";
                    return result;
                }
                else if (scratchScardEntity.OverTime < DateTime.Now)
                {
                    result.Result = "活动已经结束，敬请期待下次活动";
                    return result;
                }

                //当前的微信 openid
                string openId = CacheHelper.Get<string>("openId");
                if(!openId.IsNotNullOrEmpty())
                {
                    result.Result = "身份授权已过期，请重新刷新页面授权";
                    return result;
                }
                //参与抽奖的参与情况
                var hadPrizeList = entities.UserJoinCounter.Where(x =>x.TargetCode == (int)TargetCode.ScratchCard && x.TargetID.Equals(unid)).ToList();
                //一等奖个数
                int onePrizeNum = hadPrizeList.Where(x => x.IsPrize==1 && x.PrizeGrade == 1).Count();
                //二等奖个数
                int twoPrizeNum = hadPrizeList.Where(x => x.IsPrize == 1 && x.PrizeGrade == 2).Count();
                //三等奖个数
                int threePrizeNum = hadPrizeList.Where(x => x.IsPrize == 1 && x.PrizeGrade == 3).Count();
                //个人参与总次数
                int hadJoinCount = hadPrizeList.Where(x => x.OpenID.Equals(openId)).Count();
                if (prizeEntity.AllCountLimt <= hadPrizeList.Count)
                {
                    result.Result = "对不起，您活动次数已经达到最大次数限制";
                    return result;
                }
                //当前参与情况
                var startDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                var endDate = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                //当天参与情况
                if (hadPrizeList.Where(x => x.OpenID.Equals(openId)&& x.CreatedTime >= startDate && x.CreatedTime < endDate).Count() >= prizeEntity.DayLimt)
                {
                    result.Result =scratchScardEntity.RepeatNotice;
                    return result;
                }

                //随机概率
                Random randow = new Random();
                int num = randow.Next(1, prizeEntity.ExpectedPeopleCount);

                //保存的参与抽奖情况
                var userJoin = new UserJoinCounter();
                userJoin.UNID = Guid.NewGuid().ToString("N");
                userJoin.TargetCode = (int)TargetCode.ScratchCard;
                userJoin.TargetID = unid;
                userJoin.CreatedTime = DateTime.Now;
                userJoin.SN = userJoin.UNID.SubString(16);
                result.SN = userJoin.SN;
                userJoin.OpenID = openId;
                userJoin.IsCach = 0;

                //减去已中奖个数
                if (num <= prizeEntity.OnePrizeCount- onePrizeNum)
                {
                    userJoin.IsPrize = 1;
                    userJoin.PrizeGrade = 1;
                    result.Result = "一等奖";
                    result.IsPrize = true;
                }
                else if (prizeEntity.OnePrizeCount- onePrizeNum < num && num <= (prizeEntity.OnePrizeCount- onePrizeNum + prizeEntity.TwoPrizeCount-twoPrizeNum))
                {
                    userJoin.IsPrize = 1;
                    userJoin.PrizeGrade = 2;
                    result.Result = "二等奖";
                    result.IsPrize = true;
                }
                else if ((prizeEntity.OnePrizeCount - onePrizeNum + prizeEntity.TwoPrizeCount-twoPrizeNum) < num && num <= (prizeEntity.AllCount - onePrizeNum - twoPrizeNum - threePrizeNum))
                {
                    userJoin.IsPrize = 1;
                    userJoin.PrizeGrade = 3;
                    result.Result = "三等奖";
                    result.IsPrize = true;
                }
                else
                {
                    userJoin.IsPrize = 0;
                    userJoin.PrizeGrade = 0;
                    result.Result = "谢谢参与";
                    userJoin.SN = null;
                    result.SN = null;
                }

                entities.UserJoinCounter.Add(userJoin);

                //保存
                if (entities.SaveChanges() < 0)
                    return new PrizeResult() { IsError = true };
                else
                {
                    result.IsError = false;
                    return result;
                }
            }
        }

        /// <summary>
        /// 参与刮刮卡活动
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public bool Delete_ScratchCard(string unids)
        {
            if (!unids.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.ScratchCard.Where(x=>unids.Contains(x.UNID)).ToList().ForEach(x=> {
                    x.Flag = (x.Flag|(long)GlobalFlag.Removed);
                });
                return entities.SaveChanges() > 0 ? true : false;             
            }
        }
    }
}

