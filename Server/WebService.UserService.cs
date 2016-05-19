using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Core.Extensions;
using Repository;
using MPUtil;
using Core.Web;
using Core;
using MPUtil.UserMng;

namespace Server
{
    public partial class WebService
    {

        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public bool Update_User(string openID)
        {
            if (!openID.IsNotNullOrEmpty())
                return false;
            using (DbRepository entities = new DbRepository())
            {
                var user = entities.User.Find(openID);

                //得到token和user信息
                string access_token = CacheHelper.Get<string>("accessToken", CacheTimeOption.TwoHour, () => {
                    string token = BaseFunctions.GetAccessToken(this.Client.AppId, this.Client.AppSecret)["access_token"]?.ToString();
                    return token;
                });
                WXUser wxUser = (WXUser)UserFunction.GetInfo(access_token, openID)["user"];
                if (user == null)
                {
                    User entity = new User()
                    {
                        CreatedTime = DateTime.Now,
                        OpenID = openID,
                        NickName = wxUser.nickname,
                        City = wxUser.city,
                        Country = wxUser.country,
                        HeadImgUrl = wxUser.headimgurl,
                        Province = wxUser.province,
                        Sex = wxUser.sex.GetInt()
                    };
                    entities.User.Add(entity);
                }
                else
                {
                    user.NickName = wxUser.nickname;
                    user.Country = wxUser.country;
                    user.Province = wxUser.province;
                    user.City = wxUser.city;
                    user.Sex = wxUser.sex.GetInt();
                    user.HeadImgUrl = wxUser.headimgurl;
                }
                return entities.SaveChanges() > 0 ? true : false;
            }
        }
    }
}
