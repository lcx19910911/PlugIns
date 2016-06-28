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
using Domain.UserJoinCounter;
using System.Web;
using Domain.Mall.Recommend;

namespace Service
{
    public class MallRecommendService : BaseService, IMallRecommendService
    {
        public MallRecommendService()
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
        PageList<Domain.Mall.Recommend.RecommendModel> Get_RecommendPageList(int pageIndex, int pageSize, string name, int targetCode)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Recommend.AsQueryable();

                //活动名
                //实例化对象
                Goods goodsItem = null;
                Category categoryItem = null;
                Dictionary<string, Goods> goodsDic = new Dictionary<string, Goods>();
                Dictionary<string, Category> categoryDic = new Dictionary<string, Category>();

                //必须先选择活动类型
                if (targetCode != 0)
                {
                    query = query.Where(x => x.TargetCode == targetCode);
                    //判断活动类型
                    if (targetCode == (int)TargetCode.Goods)
                    {
                        //遍历活动名
                        var targetEntities = entities.Goods;
                        if (name.IsNotNullOrEmpty())
                        {
                            goodsDic = targetEntities.Where(x => x.Name.Contains(name)).ToDictionary(x => x.UNID);
                            var targetIdList = targetEntities.Where(x => x.Name.Contains(name)).Select(x => x.UNID).ToList();
                            query = query.Where(x => targetIdList.Contains(x.TargetID));
                        }
                        else
                        {
                            goodsDic = targetEntities.ToDictionary(x => x.UNID);
                        }
                    }
                    else if (targetCode == (int)TargetCode.Category)
                    {
                        //遍历活动名
                        var targetEntities = entities.Category;
                        if (name.IsNotNullOrEmpty())
                        {
                            categoryDic = targetEntities.Where(x => x.Name.Contains(name)).ToDictionary(x => x.UNID);
                            var targetIdList = targetEntities.Where(x => x.Name.Contains(name)).Select(x => x.UNID).ToList();
                            query = query.Where(x => targetIdList.Contains(x.TargetID));
                        }
                        else
                        {
                            categoryDic = targetEntities.ToDictionary(x => x.UNID);
                        }
                    }
                }
                else
                    return CreatePageList(new List<RecommendModel>(), 0, 0, 0); ;


                var list = new List<RecommendModel>();
                var count = query.Count();
                query.OrderBy(x => x.Sort).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        RecommendModel model = new RecommendModel();


                        model.UNID = x.UNID;
                        model.TargetCode = EnumHelper.GetEnumDescription((TargetCode)x.TargetCode);
                        model.TargetCode = EnumHelper.GetEnumDescription((TargetCode)x.TargetCode);

                        if (targetCode == (int)TargetCode.Goods)
                        {
                            goodsDic.TryGetValue(x.TargetID, out goodsItem);
                        }
                        else if (targetCode == (int)TargetCode.Goods)
                        {
                            categoryDic.TryGetValue(x.TargetID, out categoryItem);
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
                            IsCach = x.IsPrize == 1 ? (x.IsCach == 1 ? "已兑奖" : "未兑奖") : "",
                            CashTime = x.IsPrize == 1 ? (x.IsCach == 1 ? x.CachTime : null) : null
                        });
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);
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
                if (userJoinCounter == null)
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

