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
                var query = entities.ScratchCard.AsQueryable();
                if (title != null)
                {
                    query = query.Where(x => x.Name.Contains(title));
                }

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
                || model.OngoingTime==null
                || model.OverTime==null
                || !model.RepeatNotice.IsNotNullOrEmpty()
                || model.PreheatingImageFile==null
                || model.OngoingImageFile == null
                || model.OverImageFile == null
                || !model.OnePrize.IsNotNullOrEmpty()
                || !model.TwoPrize.IsNotNullOrEmpty()
                || !model.ThreePrize.IsNotNullOrEmpty()
                || !model.OnePrize.IsNotNullOrEmpty()
                )
                return "数据为空";
            if (model.OngoingTime < DateTime.Now)
                return "开始时间小于当前时间";
            if (model.OverTime < model.OngoingTime||model.OverTime<DateTime.Now)
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
                entities.ScratchCard.Add(addEntity);

                var addPrizeEntity = model.AutoMap<Domain.ScratchCard.Update, Prize>();
                if (model.AllCountLimt < model.DayLimt)
                    return "个人抽奖次数总计要大于或等于每天次数限制";
                addPrizeEntity.UNID = Guid.NewGuid().ToString("N");
                addPrizeEntity.TargetCode = (int)TargetCode.ScratchCard;
                addPrizeEntity.TargetID = addEntity.UNID;
                addPrizeEntity.AllCount = addPrizeEntity.OnePrizeCount + addPrizeEntity.TwoPrizeCount + addPrizeEntity.ThreePrizeCount;
                entities.Prize.Add(addPrizeEntity);

               return  entities.SaveChanges()>0?"":"保存出错";
            }

        }


        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:保存出错 -1:开始时间小于当前时间  -2:结束时间必须大于当前时间和开始时间  -3:个人抽奖次数总计要大于或等于每天次数限制</returns>
        public string Update_ScratchCard(Domain.ScratchCard.Update model)
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
            if (model.OngoingTime < DateTime.Now)
                return "开始时间小于当前时间";
            if (model.OverTime < model.OngoingTime || model.OverTime < DateTime.Now)
                return "结束时间必须大于当前时间和开始时间";
            if (model.AllCountLimt < model.DayLimt)
                return "个人抽奖次数总计要大于或等于每天次数限制";
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.ScratchCard.Find(model.UNID);
                if (oldEntity != null)
                {
                    if (model.PreheatingImageFile != null)
                        oldEntity.PreheatingImage = UploadHelper.Save(model.PreheatingImageFile, "ScratchCard");
                    if (model.PreheatingImageFile != null)
                        oldEntity.OngoingImage = UploadHelper.Save(model.OngoingImageFile, "ScratchCard");
                    if (model.PreheatingImageFile != null)
                        oldEntity.OverImage = UploadHelper.Save(model.OverImageFile, "ScratchCard");

                    var oldPrizeEntity = entities.Prize.Where(x => x.TargetCode == (int)TargetCode.ScratchCard && x.TargetID.Equals(model.UNID)).FirstOrDefault();
                    if (oldPrizeEntity != null)
                    {
                        model.AutoMap<Domain.ScratchCard.Update, Prize>(oldPrizeEntity);
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
                    model.PrizeUNID = prizeEntity.UNID;
                }
                if(scratchScardEntity!=null)
                    scratchScardEntity.AutoMap<ScratchCard, Domain.ScratchCard.Update>(model);

             
                return model;
            }
         }
    }
}

