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
    /// 拼图
    /// </summary>
    public class PuzzleService : BaseService, IPuzzleService
    {
        public PuzzleService()
        {
            base.ContextCurrent = HttpContext.Current;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public PageList<Puzzle> Get_PuzzlePageList(int pageIndex, int pageSize, string name, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Puzzle.AsQueryable().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                query = query.Where(x => x.PersonId.Equals(this.Client.LoginUser.UNID));
                if (createdTimeStart != null)
                {
                    query = query.Where(x => x.OngoingTime >= createdTimeStart);
                }
                if (createdTimeEnd != null)
                {
                    createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                    query = query.Where(x => x.OverTime < createdTimeEnd);
                }              

                return CreatePageList(query.OrderByDescending(x => x.CreatedTime), pageIndex, pageSize);
            }
        }


        /// <summary>
        /// 获取用户所有的拼图
        /// </summary>
        /// <returns></returns>
        public List<Puzzle> Get_AllPuzzleList()
        {
            using (DbRepository entities = new DbRepository())
            {

                var query = entities.Puzzle.AsQueryable().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0 && x.PersonId.Equals(Client.LoginUser.UNID));
                var prizeDic = entities.Prize.ToDictionary(x => x.TargetID);
                var list = new List<ScratchCardResult>();
                var prizeModel = new Prize();
                //query.OrderByDescending(x => x.CreatedTime).ToList().ForEach(x =>
                //{
                //    if (x != null)
                //    {
                //        prizeDic.TryGetValue(x.UNID, out prizeModel);
                //        ScratchCardResult model = new ScratchCardResult()
                //        {
                //            ScratchCard = x.AutoMap<ScratchCard, ApiScratchCardModel>(),
                //            Prize = prizeModel.AutoMap<Prize, ApiPrizeModel>()
                //        };
                //        model.ScratchCard.OngoingImage = UrlHelper.GetFullPath(model.ScratchCard.OngoingImage);
                //        model.ScratchCard.PreheatingImage = UrlHelper.GetFullPath(model.ScratchCard.PreheatingImage);
                //        model.ScratchCard.OverImage = UrlHelper.GetFullPath(model.ScratchCard.OverImage);
                //        list.Add(model);
                //    }
                //});

                return null;
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add_Puzzle(Puzzle model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || model.OngoingTime == null
                || model.OverTime == null
                || !model.Image.IsNotNullOrEmpty()
                )
                return "数据为空";
            if (model.OngoingTime < DateTime.Now)
                return "开始时间需比晚于当前时间";
            if (model.OverTime < model.OngoingTime || model.OverTime < DateTime.Now)
                return "结束时间必须大于当前时间和开始时间";
            using (DbRepository entities = new DbRepository())
            {
                model.UNID = Guid.NewGuid().ToString("N");
                model.CreatedTime = DateTime.Now;
                model.UpdatedTime = DateTime.Now;
                model.Flag = (long)GlobalFlag.Normal;
                model.PersonId = this.Client.LoginUser.UNID;

                entities.Puzzle.Add(model);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update_Puzzle(Puzzle model, string unid)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || model.OngoingTime == null
                || model.OverTime == null
                || !model.Image.IsNotNullOrEmpty()
                )
                return "数据为空";
            if (model.OngoingTime < DateTime.Now)
                return "开始时间需比晚于当前时间";
            if (model.OverTime < model.OngoingTime || model.OverTime < DateTime.Now)
                return "结束时间必须大于当前时间和开始时间";
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.Puzzle.Find(unid);
                if (oldEntity != null)
                {
                    oldEntity.UpdatedTime = DateTime.Now;
                    oldEntity.Name = model.Name;
                    oldEntity.Image = model.Image;
                    oldEntity.DifficultyType = model.DifficultyType;
                    oldEntity.IsBindScore = model.IsBindScore;
                    oldEntity.Description = model.Description;
                    oldEntity.BindUrl = model.BindUrl;
                    oldEntity.OngoingTime = model.OngoingTime;
                    oldEntity.OverTime = model.OverTime;

                }
                else
                    return "数据为空";

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }

        /// <summary>
        /// 查找拼图
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public Puzzle Find_Puzzle(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                return entities.Puzzle.Find(unid);
            }
        }


        /// <summary>
        /// 删除拼图
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public bool Delete_Puzzle(string unids)
        {
            if (!unids.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository()) 
            {
                //找到实体
                entities.Puzzle.Where(x => unids.Contains(x.UNID)).ToList().ForEach(x =>
                {
                    x.Flag = (x.Flag | (long)GlobalFlag.Removed);
                });
                return entities.SaveChanges() > 0 ? true : false;
            }
        }
    }
}

