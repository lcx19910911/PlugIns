using Core;
using Core.AuthAPI;
using Core.Extensions;
using Core.Helper;
using Core.Model;
using Core.Web;
using EnumPro;
using Extension;
using IService;
using MPUtil.UserMng;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service
{
    /// <summary>
    /// 用户签到
    /// </summary>
    public  class UserSignService : BaseService, IUserSignService
    {

        public UserSignService()
        {
            base.ContextCurrent = HttpContext.Current;
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        public bool User_Sign()
        {
            var user = CacheHelper.Get<Repository.User>("user");
            var person = CacheHelper.Get<Person>("person");
            if (user == null || person == null)
                return false;
            using (DbRepository entities = new DbRepository())
            {
                var userEntity = entities.User.Find(user.OpenId);
                if (userEntity == null)
                    return false;
                var yesterday =DateTime.Now.AddDays(-1).Date;
                var lastSign = entities.UserSign.Where(x=>x.OpenId.Equals(user.OpenId)&&x.PersonId.Equals(person.UNID)).OrderByDescending(x => x.SignDate).First();

                //判断今天是否已签到
                if (lastSign.SignDate > yesterday)
                {
                    return false;
                }
                //判断是否连续签到
                if (lastSign != null)
                {
                    var todaySign = new UserSign()
                    {
                        UNID = Guid.NewGuid().ToString("N"),
                        SignDate = DateTime.Now,
                        OpenId = user.OpenId,
                        PersonId=person.UNID
                    };
                    if (lastSign.SignDate == yesterday)
                    {
                        todaySign.SignNum = lastSign.SignNum + 1;

                        //签到10天判断
                        if (todaySign.SignNum % 10 == 0 && todaySign.SignNum >= 10)
                        {
                            var tenScoreDetials = new ScoreDetails()
                            {
                                UNID = Guid.NewGuid().ToString("N"),
                                OpenId = user.OpenId,
                                CreatedTime = DateTime.Now,
                                Description = "连续签到10天获得积分",
                                IsAdd = (int)YesOrNoCode.Yes,
                                Value = Params.TendayScore,
                                Type = (int)ScoreType.Sign,
                                PersonId = person.UNID
                            };
                            entities.ScoreDetails.Add(tenScoreDetials);

                            //是否初次签到
                            var userScore = entities.UserScore.FirstOrDefault(x => x.OpenId.Equals(user.OpenId) && x.PersonId.Equals(person.UNID));
                            if (userScore == null)
                            {
                                var addUserScore = new UserScore()
                                {
                                    UNID = Guid.NewGuid().ToString("N"),
                                    OpenId = user.OpenId,
                                    PersonId = person.UNID,
                                    Score = Params.TendayScore
                                };
                                entities.UserScore.Add(addUserScore);
                            }
                            else
                            {
                                userScore.Score += Params.TendayScore;
                            }
                        }
                    }
                    else
                        todaySign.SignNum = 1;


                    entities.UserSign.Add(todaySign);
                }
                else
                {
                    var todaySign = new UserSign()
                    {
                        UNID = Guid.NewGuid().ToString("N"),
                        SignDate = DateTime.Now,
                        SignNum = 1,
                        OpenId = user.OpenId,
                        PersonId = person.UNID
                    };
                    entities.UserSign.Add(todaySign);
                }

                //日常签到积分
                var scoreDetials = new ScoreDetails()
                {
                    UNID = Guid.NewGuid().ToString("N"),
                    OpenId = user.OpenId,
                    CreatedTime = DateTime.Now,
                    Description = "签到获得积分",
                    IsAdd = (int)YesOrNoCode.Yes,
                    Value = Params.SignScore,
                    Type = (int)ScoreType.Sign,
                    PersonId=person.UNID
                };
                //用户积分增加
                var updateUserScore = entities.UserScore.FirstOrDefault(x => x.OpenId.Equals(user.OpenId) && x.PersonId.Equals(person.UNID));
                if (updateUserScore == null)
                {
                    var addUserScore = new UserScore()
                    {
                        UNID = Guid.NewGuid().ToString("N"),
                        OpenId = user.OpenId,
                        PersonId = person.UNID,
                        Score = Params.SignScore
                    };
                    entities.UserScore.Add(addUserScore);
                }
                else
                {
                    updateUserScore.Score += Params.SignScore;
                }
                entities.ScoreDetails.Add(scoreDetials);

                return entities.SaveChanges() > 0 ? true : false;
            }
        }




        /// <summary>
        /// 最近十天的签到
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        public Dictionary<string,bool> Get_LastelyTenDaySign(string openId,string personId)
        {
            using (DbRepository entities = new DbRepository())
            {
                var dayStar = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
                var dayEnd= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                var lastSignList = entities.UserSign.Where(x => x.OpenId.Equals(openId)&&x.PersonId.Equals(personId) &&x.SignDate>dayStar&&x.SignDate< dayEnd).OrderBy(x => x.SignDate).ToList();
                Dictionary<string, bool> dic = new Dictionary<string, bool>();
                while (dayStar < dayEnd)
                {
                    var model=lastSignList.Where(x => x.SignDate == dayStar).FirstOrDefault();
                    if (model != null)
                        dic.Add(dayStar.ToString("yyyy-MM-dd"), true);
                    else
                        dic.Add(dayStar.ToString("yyyy-MM-dd"), false);
                    dayStar = dayStar.AddDays(1);
                }
                return dic;
            }
        }

        /// <summary>
        /// 最近的一个签到
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        public UserSign Get_LastSign(string openId, string personId)
        {
            using (DbRepository entities = new DbRepository())
            {
                var lastSign = entities.UserSign.Where(x => x.OpenId.Equals(openId)&&x.PersonId.Equals(personId)).OrderByDescending(x => x.SignDate).FirstOrDefault();
                return lastSign;
            }
        }

    }
}
