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
using EnumPro;

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
        public PageList<Domain.Mall.Recommend.RecommendModel> Get_RecommendPageList(int pageIndex, int pageSize, string name, int recommendCode)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Recommend.AsQueryable().Where(x=>x.PersonId.Equals(Client.LoginUser.UNID));

                //活动名
                //实例化对象
                Goods goodsItem = null;
                Category categoryItem = null;
                Dictionary<string, Goods> goodsDic = new Dictionary<string, Goods>();
                Dictionary<string, Category> categoryDic = new Dictionary<string, Category>();

                //必须先选择活动类型
                if (recommendCode != 0)
                {
                    query = query.Where(x => x.RecommendCode == recommendCode);
                    //判断活动类型
                    if (recommendCode == (int)RecommendCode.HomeGoods)
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
                    else if (recommendCode == (int)RecommendCode.HomeCategory)
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
                        model.RecommendCode = x.RecommendCode;
                        model.CreatedTime = x.CreatedTime;
                        model.Sort = x.Sort;
                        model.Title = x.Title;

                        if (recommendCode == (int)RecommendCode.HomeGoods)
                        {
                            goodsDic.TryGetValue(x.TargetID, out goodsItem);
                            model.TargetName = goodsItem.Name;
                            model.TargetID = goodsItem.UNID;
                        }
                        else if (recommendCode == (int)RecommendCode.HomeCategory)
                        {
                            categoryDic.TryGetValue(x.TargetID, out categoryItem);
                            model.TargetName = categoryItem.Name;
                            model.TargetID = categoryItem.UNID;
                        }

                        list.Add(model);
                    }
                });

                return CreatePageList(list, pageIndex, pageSize, count);
            }
        }



        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_Recommend(Recommend model)
        {
            if (model == null
                || !model.TargetID.IsNotNullOrEmpty()
                || model.RecommendCode== 0
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Recommend.AsQueryable();
                if (query.Where(x => x.TargetID.Equals(model.TargetID)).Count() != 0)
                    return "推荐已存在";

                var addEntity = new Recommend();
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.Sort = model.Sort;
                addEntity.Title = model.Title;
                addEntity.CreatedTime = DateTime.Now;
                addEntity.PersonId = Client.LoginUser.UNID;
                addEntity.TargetCode = model.TargetCode;
                addEntity.TargetID = model.TargetID;
                addEntity.RecommendCode = model.RecommendCode;

                entities.Recommend.Add(addEntity);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_GoodsRecommend(Recommend model)
        {
            if (model == null
                || !model.TargetID.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Goods.AsQueryable();
                if (query.Where(x => x.UNID.Equals(model.TargetID)).Count() != 0)
                    return "推荐已存在";

                var addEntity = new Recommend();
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.Sort = model.Sort;
                addEntity.Title = model.Title;
                addEntity.CreatedTime = DateTime.Now;
                addEntity.PersonId = Client.LoginUser.UNID;
                addEntity.TargetCode = (int)TargetCode.Goods;
                addEntity.TargetID = model.TargetID;
                addEntity.RecommendCode = (int)RecommendCode.HomeGoods;

                entities.Recommend.Add(addEntity);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_CategoryRecommend(Recommend model)
        {
            if (model == null
                || !model.TargetID.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Recommend.AsQueryable();
                if (query.Where(x => x.TargetID.Equals(model.TargetID)).Count() != 0)
                    return "推荐已存在";

                var addEntity = new Recommend();
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.Sort = model.Sort;
                addEntity.Title = model.Title;
                addEntity.CreatedTime = DateTime.Now;
                addEntity.PersonId = Client.LoginUser.UNID;
                addEntity.TargetCode = (int)TargetCode.Category;
                addEntity.TargetID = model.TargetID;
                addEntity.RecommendCode = (int)RecommendCode.HomeCategory;

                entities.Recommend.Add(addEntity);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public bool Delete_Recommend(string unids)
        {
            if (!unids.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.Recommend.Where(x => unids.Contains(x.UNID)).ToList().ForEach(x => {
                    entities.Recommend.Remove(x);
                });
                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 获取推荐商品
        /// </summary>
        /// <returns></returns>
        public List<Tuple<Repository.Goods, string>> Get_RecommendGoods(string personId)
        {
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                var recommendList = entities.Recommend.Where(x => x.RecommendCode == (int)RecommendCode.HomeGoods && x.PersonId.Equals(personId)).OrderBy(x=>x.Sort).ToList(); 
                var recommendIdList= recommendList.Select(x=>x.TargetID).ToList();

                List<Tuple<Repository.Goods, string>> list = new List<Tuple<Goods, string>>();

                entities.Goods.Where(x => recommendIdList.Contains(x.UNID)).ToList().ForEach(x =>
                {
                    list.Add(new Tuple<Goods, string>(x, recommendList.FirstOrDefault(y => y.TargetID.Equals(x.UNID))?.Title));
                });

                return list;
            }
        }

        /// <summary>
        /// 获取推荐分类
        /// </summary>
        /// <returns></returns>
        public List<Tuple<Repository.Category, string>> Get_RecommendCategory(string personId)
        {
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                var recommendList = entities.Recommend.Where(x => x.RecommendCode == (int)RecommendCode.HomeCategory && x.PersonId.Equals(personId)).OrderBy(x => x.Sort).ToList();
                var recommendIdList = recommendList.Select(x => x.TargetID).ToList();

                List<Tuple<Repository.Category, string>> list = new List<Tuple<Category, string>>();

                entities.Category.Where(x => recommendIdList.Contains(x.UNID)).ToList().ForEach(x =>
                {
                    list.Add(new Tuple<Category, string>(x, recommendList.FirstOrDefault(y => y.TargetID.Equals(x.UNID))?.Title));
                });

                return list;
            }
        }
    }
}

