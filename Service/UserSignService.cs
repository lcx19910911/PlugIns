using Core;
using Core.AuthAPI;
using Core.Extensions;
using Core.Helper;
using Core.Model;
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
        public bool User_Sign(string openId)
        {
            if (string.IsNullOrEmpty(openId))
                return false;
            using (DbRepository entities = new DbRepository())
            {
                var user = entities.User.Find(openId);
                if (user == null)
                    return false;

                var yesterday =DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                var lastSign = entities.UserSign.Where(x=>x.OpenId.Equals(openId)&&x.SignDate> yesterday).OrderByDescending(x => x.SignDate).FirstOrDefault();
                if (lastSign != null)
                {
                    var todaySign = new UserSign()
                    {
                        UNID = Guid.NewGuid().ToString("N"),
                        SignDate = DateTime.Now,
                        SignNum = lastSign.SignNum++,
                        OpenId = openId
                    };
                    if (todaySign.SignNum / 10 == 0)
                    {                     
                        var tenScoreDetials = new ScoreDetails()
                        {
                            UNID = Guid.NewGuid().ToString("N"),
                            OpenId = openId,
                            CreatedTime = DateTime.Now,
                            Description = "连续签到10天获得积分",
                            IsAdd = (int)YesOrNoCode.Yes,
                            Value = Params.TendayScore
                        };
                        entities.ScoreDetails.Add(tenScoreDetials);
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
                        OpenId = openId
                    };
                    entities.UserSign.Add(todaySign);
                }

                var scoreDetials = new ScoreDetails()
                {
                    UNID = Guid.NewGuid().ToString("N"),
                    OpenId = openId,
                    CreatedTime = DateTime.Now,
                    Description = "签到获得积分",
                    IsAdd = (int)YesOrNoCode.Yes,
                    Value = Params.SignScore
                };
                entities.ScoreDetails.Add(scoreDetials);

                return entities.SaveChanges() > 0 ? true : false;
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
