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

        ///// <summary>
        ///// (自己获取用户数据)
        ///// </summary>
        ///// <param name="unid"></param>
        ///// <returns></returns>
        //public bool Update_User(string openID)
        //{
        //    if (!openID.IsNotNullOrEmpty())
        //        return false;
        //    using (DbRepository entities = new DbRepository())
        //    {
        //        var user = entities.User.Find(openID);

        //        //得到token和user信息
        //        string access_token = CacheHelper.Get<string>("accessToken", CacheTimeOption.TwoHour, () => {
        //            string token = BaseFunctions.GetAccessToken(this.Client.AppId, this.Client.AppSecret)["access_token"]?.ToString();
        //            return token;
        //        });
        //        WXUser wxUser = (WXUser)UserFunction.GetInfo(access_token, openID)["user"];
        //        if (user == null)
        //        {
        //            User entity = new User()
        //            {
        //                CreatedTime = DateTime.Now,
        //                OpenID = openID,
        //                NickName = wxUser.nickname,
        //                City = wxUser.city,
        //                Country = wxUser.country,
        //                HeadImgUrl = wxUser.headimgurl,
        //                Province = wxUser.province,
        //                Sex = wxUser.sex.GetInt()
        //            };
        //            entities.User.Add(entity);
        //        }
        //        else
        //        {
        //            user.NickName = wxUser.nickname;
        //            user.Country = wxUser.country;
        //            user.Province = wxUser.province;
        //            user.City = wxUser.city;
        //            user.Sex = wxUser.sex.GetInt();
        //            user.HeadImgUrl = wxUser.headimgurl;
        //        }
        //        return entities.SaveChanges() > 0 ? true : false;
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public bool Update_User(User model)
        {
            if (model==null)
                return false;
            using (DbRepository entities = new DbRepository())
            {
                var user = entities.User.Find(model.OpenID);

                if (user == null)
                {
                    entities.User.Add(model);
                }
                else
                {
                    user.NickName = model.NickName;
                    user.Country = model.Country;
                    user.Province = model.Province;
                    user.City = model.City;
                    user.Sex = model.Sex;
                    user.HeadImgUrl = model.HeadImgUrl;
                }
                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public PageList<User> Get_UserPageList(int pageIndex, int pageSize, string name, string openId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.User.AsQueryable();
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.NickName.Contains(name));
                }
                if (openId.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.OpenID.Contains(openId));
                }
                if (createdTimeStart != null)
                {
                    query = query.Where(x => x.CreatedTime >= createdTimeStart);
                }
                if (createdTimeEnd != null)
                {
                    createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                    query = query.Where(x => x.CreatedTime < createdTimeEnd);
                }

                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return CreatePageList(list, pageIndex, pageSize, count);
            }
        }
    }
}
