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
            Repository.User user = CacheHelper.Get<Repository.User>("user");
            if (user==null||string.IsNullOrEmpty(user.OpenId))
                return false;
            using (DbRepository entities = new DbRepository())
            {
                var userEntity = entities.User.Find(user.OpenId);
                if (userEntity == null)
                    return false;
                var yesterday =DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                var lastSign = entities.UserSign.Where(x=>x.OpenId.Equals(user.OpenId) &&x.SignDate> yesterday).OrderByDescending(x => x.SignDate).FirstOrDefault();
                if (lastSign != null)
                {
                    var todaySign = new UserSign()
                    {
                        UNID = Guid.NewGuid().ToString("N"),
                        SignDate = DateTime.Now,
                        SignNum = lastSign.SignNum++,
                        OpenId = user.OpenId
                    };
                    if (todaySign.SignNum / 10 == 0)
                    {                     
                        var tenScoreDetials = new ScoreDetails()
                        {
                            UNID = Guid.NewGuid().ToString("N"),
                            OpenId = user.OpenId,
                            CreatedTime = DateTime.Now,
                            Description = "连续签到10天获得积分",
                            IsAdd = (int)YesOrNoCode.Yes,
                            Value = Params.TendayScore,
                            Type= (int)ScoreType.Sign
                        };
                        entities.ScoreDetails.Add(tenScoreDetials);
                        userEntity.Score += Params.TendayScore;
                    }
                    entities.UserSign.Add(todaySign);
                }
                else
                {
                    var todaySign = new UserSign()
                    {
                        UNID = Guid.NewGuid().ToString("N"),
                        SignDate = DateTime.Now,
                        SignNum = 0,
                        OpenId = user.OpenId
                    };
                    entities.UserSign.Add(todaySign);
                }

                var scoreDetials = new ScoreDetails()
                {
                    UNID = Guid.NewGuid().ToString("N"),
                    OpenId = user.OpenId,
                    CreatedTime = DateTime.Now,
                    Description = "签到获得积分",
                    IsAdd = (int)YesOrNoCode.Yes,
                    Value = Params.SignScore,
                    Type = (int)ScoreType.Sign
                };
                userEntity.Score += Params.SignScore;
                entities.ScoreDetails.Add(scoreDetials);

                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 最近十天的签到
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        public Dictionary<string,bool> Get_LastelyTenDaySign(string openId)
        {
            using (DbRepository entities = new DbRepository())
            {
                var dayStar = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
                var dayEnd= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                var lastSignList = entities.UserSign.Where(x => x.OpenId.Equals(openId)&&x.SignDate>dayStar&&x.SignDate< dayEnd).OrderBy(x => x.SignDate).ToList();
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
        public UserSign Get_LastSign(string openId)
        {
            using (DbRepository entities = new DbRepository())
            {
                var lastSign = entities.UserSign.Where(x => x.OpenId.Equals(openId)).OrderByDescending(x => x.SignDate).FirstOrDefault();
                return lastSign;
            }
        }

    }
}
