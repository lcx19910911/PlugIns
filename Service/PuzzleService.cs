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

        /// <summary>
        /// 获取下一个拼图
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public Puzzle Get_NextPuzzle(string unid, string openId, string personId)
        {
            using (DbRepository entities = new DbRepository())
            {
                var ongingList = Get_OngingPuzzleList(entities, openId, personId);
                if (ongingList == null||ongingList.Count==0)
                    return null;
                if (unid.IsNotNullOrEmpty())
                {
                    int index = ongingList.FindIndex(x => x.UNID.Equals(unid));
                    if (ongingList.Count > index + 1)
                        return ongingList[index + 1];
                    else
                        return ongingList[0];
                }
                else
                {
                    if (ongingList.Count != 0)
                    {
                        Random rd = new Random();
                        int index = rd.Next(0, ongingList.Count);
                        return ongingList[index];
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// 获取用户正在活动内的拼图活动
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="openId">用户openid</param>
        /// <returns></returns>
        private List<Puzzle> Get_OngingPuzzleList(DbRepository entities, string openId, string personId)
        {
            var query = entities.Puzzle.AsQueryable().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0);
            query = query.Where(x => x.PersonId.Equals(personId));

            var dateTimeNow = DateTime.Now.Date;
            query = query.Where(x => x.OngoingTime <= dateTimeNow && x.OverTime > dateTimeNow);

            var hadPuzzleIdList = entities.UserPuzzle.Where(x => x.OpenId.Equals(openId) && x.PuzzleDate == dateTimeNow).Select(x => x.PuzzleId).ToList();
            query = query.Where(x => !hadPuzzleIdList.Contains(x.UNID));

            return query.OrderBy(x => x.OngoingTime).ToList();
        }


        /// <summary>
        /// 完成拼图结果
        /// </summary>
        /// <returns>操作结果  提示语句  是否绑定平台活动  平台活动名 绑定地址</returns>
        public Tuple<bool, string, bool, string, string> Complete(string unid)
        {
            var user = CacheHelper.Get<Repository.User>("user");
            var person = CacheHelper.Get<Person>("person");
            if (user == null || person == null)
                return new Tuple<bool, string, bool, string, string>(false,"身份过期", false, "", "");
            using (DbRepository entities = new DbRepository())
            {
                var puzzle = entities.Puzzle.Find(unid);
                if(puzzle==null)
                    return new Tuple<bool, string, bool, string, string>(false, "参数错误", false, "", "");
                var dateTime = DateTime.Now.Date;

                if (entities.UserPuzzle.FirstOrDefault(x => x.PuzzleId.Equals(unid) && x.PuzzleDate == dateTime) != null)
                {
                    return new Tuple<bool, string, bool, string, string>(false, "该拼图已玩过",false, "", "");
                }
                var userPuzzle = new UserPuzzle()
                {
                    UNID = Guid.NewGuid().ToString("N"),
                    OpenId = user.OpenId,
                    PuzzleDate = dateTime,
                    PuzzleId = unid
                };
                entities.UserPuzzle.Add(userPuzzle);

                if (puzzle.IsBindScore==(int)YesOrNoCode.Yes)
                {
                    //日常签到积分
                    var scoreDetials = new ScoreDetails()
                    {
                        UNID = Guid.NewGuid().ToString("N"),
                        OpenId = user.OpenId,
                        CreatedTime = DateTime.Now,
                        Description = "完成拼图获得积分",
                        IsAdd = (int)YesOrNoCode.Yes,
                        Value = puzzle.Score,
                        Type = (int)ScoreType.Puzzle,
                        PersonId = person.UNID,
                        TargetId=unid
                    };

                    entities.ScoreDetails.Add(scoreDetials);
                    //用户积分增加
                    var updateUserScore = entities.UserScore.FirstOrDefault(x => x.OpenId.Equals(user.OpenId) && x.PersonId.Equals(person.UNID));
                    if (updateUserScore == null)
                    {
                        var addUserScore = new UserScore()
                        {
                            UNID = Guid.NewGuid().ToString("N"),
                            OpenId = user.OpenId,
                            PersonId = person.UNID,
                            Score = puzzle.Score
                        };
                        entities.UserScore.Add(addUserScore);
                    }
                    else
                    {
                        updateUserScore.Score += puzzle.Score;
                    }

                    return entities.SaveChanges()>0?new Tuple<bool, string, bool, string, string>(true, string.Format("恭喜你获得：{0}积分",puzzle.Score),false, "", ""):new Tuple<bool, string, bool, string, string>(false, "保存出错", false, "", "");
                }
                else
                {
                    return entities.SaveChanges()>0?new Tuple<bool, string, bool, string, string>(true, puzzle.BindTitle, true, puzzle.BindName,puzzle.BindUrl): new Tuple<bool, string, bool, string, string>(false, "保存出错", false, "", "");
                }
            }
        }
    }
}

