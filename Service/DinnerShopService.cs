using Core.Extensions;
using Core.Helper;
using Core.Model;
using Domain.API;
using EnumPro;
using Extension;
using IService;
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
    /// 门店
    /// </summary>
    public class DinnerShopService : BaseService, IDinnerShopService
    {

        public DinnerShopService()
        {
            base.ContextCurrent = HttpContext.Current;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">门店名 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public PageList<Domain.DinnerShop.List> Get_DinnerShopPageList(int pageIndex, int pageSize, string name, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerShop.AsQueryable().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0).Where(x=>x.PersonId.Equals(this.Client.LoginUser.UNID));
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
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

                var list = new List<Domain.DinnerShop.List>();
                var count = query.Count();
                query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        list.Add(new Domain.DinnerShop.List()
                        {
                            UNID = x.UNID,
                            CreatedTime = x.CreatedTime,
                            Name = x.Name,
                            UpdatedTime = x.UpdatedTime,
                            Sort = x.Sort,
                            HoldTime = string.Format("{0}-{1}", x.StartShoptime, x.EndShoptime),
                            Flag = EnumHelper.GetEnumDescription((GlobalFlag)x.Flag)
                        });
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
        public string Add_DinnerShop(Domain.DinnerShop.Update model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                || !model.Account.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerShop.AsQueryable();
                if (query.Where(x => x.Name.Equals(model.Name) && ((x.Flag & (long)GlobalFlag.Removed) != 0)).Count() != 0)
                    return "店铺名称已存在";

                if (entities.Person.Where(x => x.Account.Equals(model.Account) && ((x.Flag & (long)GlobalFlag.Removed) != 0)).Count() != 0)
                    return "店铺登陆账号已存在";
                string password = Core.Util.CryptoHelper.MD5_Encrypt(model.Password);

                var addEntity = model.AutoMap<Domain.DinnerShop.Update, DinnerShop>();
                addEntity.UNID = Guid.NewGuid().ToString("N");

                var addPerson = new Person()
                {
                    UNID = Guid.NewGuid().ToString("N"),
                    Account = model.Account,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                    Flag = (long)GlobalFlag.Normal,
                    Password = password,
                    Remark = "店铺",
                    IsChildren=(int)YesOrNoCode.Yes,
                    ShopId= addEntity.UNID,
                    Name=model.Name
                };

                addEntity.Name = model.Name;
                addEntity.CreatedTime = DateTime.Now;
                addEntity.UpdatedTime = DateTime.Now;
                addEntity.Flag = (long)GlobalFlag.Normal;
                addEntity.PersonId = this.Client.LoginUser.UNID;


                entities.DinnerShop.Add(addEntity);
                entities.Person.Add(addPerson);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update_DinnerShop(Domain.DinnerShop.Update model, string unid)
        {
            if (model == null
                 || !model.Name.IsNotNullOrEmpty()
                 || !model.Account.IsNotNullOrEmpty()
                )
                return "数据为空";
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.DinnerShop.Find(unid);
                if (oldEntity != null)
                {
                    var query = entities.DinnerShop.AsQueryable();
                    if (query.Where(x => x.Name.Equals(model.Name) && !x.UNID.Equals(unid) && ((x.Flag & (long)GlobalFlag.Removed) != 0)).Count() != 0)
                        return "店铺名称已存在";

                    var personEntity = entities.Person.Where(x => x.UNID.Equals(oldEntity.PersonId) && ((x.Flag & (long)GlobalFlag.Removed) != 0)).FirstOrDefault();
                    if (personEntity == null)
                        return "店铺登陆账号不存在";
                    string password = Core.Util.CryptoHelper.MD5_Encrypt(model.Password);
                    if (personEntity.Account != model.Account || personEntity.Password != password)
                    {
                        if (entities.Person.Where(x => x.Account.Equals(model.Account) && !x.UNID.Equals(oldEntity.PersonId) && ((x.Flag & (long)GlobalFlag.Removed) != 0)).Count() != 0)
                            return "账号已存在";
                        personEntity.Account = model.Account;
                        if (!string.IsNullOrEmpty(model.Password))
                            personEntity.Password = password;
                    }

                    model.AutoMap<Domain.DinnerShop.Update, DinnerShop>(oldEntity);
                    oldEntity.UpdatedTime = DateTime.Now;
                }
                else
                    return "数据为空";

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }

        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="unids">unid，多个id用逗号分隔</param>
        /// <returns>影响条数</returns>
        public string Enable_DinnerShop(string unids)
        {
            if (string.IsNullOrEmpty(unids))
                return "无数据";
            using (DbRepository entities = new DbRepository())
            {
                //按逗号分隔符分隔开得到unid列表
                var unidArray = unids.Split(',');

                entities.DinnerShop.Where(x => unids.Contains(x.UNID)).ToList().ForEach(x =>
                {
                    x.Flag = x.Flag & ~(long)GlobalFlag.Unabled;
                });

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }
        }


        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="unids">unid，多个id用逗号分隔</param>
        /// <returns>影响条数</returns>
        public string Disable_DinnerShop(string unids)
        {
            if (string.IsNullOrEmpty(unids))
                return "无数据";
            using (DbRepository entities = new DbRepository())
            {
                //按逗号分隔符分隔开得到unid列表
                var unidArray = unids.Split(',');

                entities.DinnerShop.Where(x => unids.Contains(x.UNID)).ToList().ForEach(x =>
                {
                    x.Flag = x.Flag | (long)GlobalFlag.Unabled;
                });

                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public bool Delete_DinnerShop(string unids)
        {
            if (!unids.IsNotNullOrEmpty())
            {
                return false;
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.DinnerShop.Where(x => unids.Contains(x.UNID)).ToList().ForEach(x => {
                    x.Flag = (x.Flag | (long)GlobalFlag.Removed);
                });
                return entities.SaveChanges() > 0 ? true : false;
            }
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public Domain.DinnerShop.Update Find_DinnerShop(string unid)
        {
            if (!unid.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                Domain.DinnerShop.Update model = new Domain.DinnerShop.Update();

                var entity = entities.DinnerShop.Find(unid);
                var personEntity = entities.Person.Where(x => x.ShopId.Equals(unid)).FirstOrDefault();
                if (entity != null)
                {
                    entity.AutoMap<DinnerShop, Domain.DinnerShop.Update>(model);
                }
                else
                    return null;
                if (personEntity != null)
                {
                    model.Account = personEntity.Account;
                }
                else
                    return null;
                return model;
            }
        }


        /// <summary>
        /// 获取当前用户的店铺
        /// </summary>
        /// <returns></returns>
        public List<ApiDinnerShopModel> Get_DinnerShopList()
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.DinnerShop.AsQueryable().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0 && x.PersonId.Equals(this.Client.LoginUser.UNID));
                var list = new List<ApiDinnerShopModel>();
                var prizeModel = new Prize();
                query.OrderByDescending(x => x.CreatedTime).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        ApiDinnerShopModel model = x.AutoMap<DinnerShop, ApiDinnerShopModel>();
                        model.Image= UrlHelper.GetFullPath(model.Image);
                        list.Add(model);
                    }

                });

                return list;
            }
        }
    }
}
