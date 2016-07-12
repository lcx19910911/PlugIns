using Core.AuthAPI;
using Core.Extensions;
using Core.Helper;
using Core.Model;
using EnumPro;
using Extension;
using IService;
using Model;
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
    /// 微信用户
    /// </summary>
    public  class UserService : BaseService, IUserService
    {

        public UserService()
        {
            base.ContextCurrent = HttpContext.Current;
        }


        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public User Find_User(string openId)
        {
            if (string.IsNullOrEmpty(openId))
                return null;
            using (DbRepository entities = new DbRepository())
            {
                return entities.User.Find(openId);
            }
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public int Find_PersonUserScore(string personId,string openId)
        {
            if (string.IsNullOrEmpty(personId)|| string.IsNullOrEmpty(openId))
                return 0;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.UserScore.FirstOrDefault(x => x.OpenId.Equals(openId) && x.PersonId.Equals(personId));
                return entity==null?0: entity.Score;
            }
        }


        /// <summary>
        /// 编辑管理用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update_User(WXUser model)
        {
            if (model == null)
                return false;
            using (DbRepository entities = new DbRepository())
            {
                var user = entities.User.Find(model.openid);

                if (user == null)
                {
                    var addEntity = new User()
                    {
                        OpenId = model.openid,
                        NickName = model.nickname,
                        Country = model.country,
                        Province = model.province,
                        City = model.city,
                        Sex = model.sex.GetInt(),
                        HeadImgUrl = model.headimgurl,
                        CreatedTime=DateTime.Now
                    };

                    entities.User.Add(addEntity);
                }
                else
                {
                    user.NickName = model.nickname;
                    user.Country = model.country;
                    user.Province = model.province;
                    user.City = model.city;
                    user.Sex = model.sex.GetInt();
                    user.HeadImgUrl = model.headimgurl;
                }
                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">昵称</param>
        /// <param name="openId"></param>
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
                    query = query.Where(x => x.OpenId.Contains(openId));
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


        /// <summary>
        /// 获取用户的积分记录
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="personId"></param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public List<ScoreDetails> Get_UserScore(string openId, string personId, int pageIndex)
        {
            using (DbRepository entities = new DbRepository())
            {
                return entities.ScoreDetails.Where(x => x.OpenId.Equals(openId) & x.PersonId.Equals(personId)).OrderByDescending(x=>x.CreatedTime).Skip((pageIndex - 1) * 10).Take(10).ToList();
            }
        }
    }
}
