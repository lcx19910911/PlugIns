using Core.Model;
using  EnumPro;
using Extension;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class WebService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="source">实体</param>
        /// <param name="password">密码</param>
        /// <returns>影响条数</returns>
        public string Add_Person(Domain.Person.Add source, string password)
        {
            using (DbRepository entities = new DbRepository())
            {
                var addEntity = source.AutoMap<Domain.Person.Add, Person>();
                if (password != null)
                {
                    addEntity.Password = Core.Util.CryptoHelper.MD5_Encrypt(password);
                }

                entities.Entry(addEntity).State = System.Data.Entity.EntityState.Added;

                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.CreatedTime = DateTime.Now;
                addEntity.UpdatedTime = DateTime.Now;
                addEntity.Flag = (long)GlobalFlag.Normal;
                addEntity.TargetCode = (int)TargetCode.Admin;

                entities.Person.Add(addEntity);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="unid">主键</param>
        /// <param name="source">实体</param>
        /// <param name="password">密码</param>
        /// <returns>影响条数</returns>
        public void Update_Person(string unid, Domain.Person.Update source, string password)
        {

            using (DbRepository entities = new DbRepository())
            {
                var dbSet = entities.Set<Person>();
                var sourceEntity = dbSet.Find(unid);
                if (sourceEntity != null)
                {
                    source.AutoMap<Domain.Person.Update, Person>(sourceEntity);
                    if (!string.IsNullOrEmpty(password))
                    {
                        sourceEntity.Password = Core.Util.CryptoHelper.MD5_Encrypt(password);
                    }
                }
                entities.SaveChanges();
            }
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="keyword">关键字 - 搜索项</param>
        /// <returns></returns>
        public PageList<Person> Get_PersonPageList(int pageIndex, int pageSize, string keyword)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Person.AsQueryable();

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(x => x.Name.Contains(keyword)|| x.Mobile.Contains(keyword));
                }

                return CreatePageList(query.OrderByDescending(x => x.CreatedTime), pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        public string UpdatePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                return "数据为空";
            if (!newPassword.Equals(confirmPassword))
                return "确认密码输入不一致";
            if (Client.LoginUser == null)
                return "数据异常";
            using (DbRepository entities = new DbRepository())
            {
                var user = entities.Person.Find(Client.LoginUser.UNID);
                if (user == null)
                    return "数据异常";
                string md5Password = Core.Util.CryptoHelper.MD5_Encrypt(oldPassword);
                if (!user.Password.Equals(md5Password))
                    return "旧密码不对";

                user.Password= Core.Util.CryptoHelper.MD5_Encrypt(newPassword);

                return entities.SaveChanges() > 0 ? "" : "保存错误";
            }
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="cardNo">工号</param>
        /// <param name="password">密码</param>
        /// <returns>返回登录的用户对象，如果登录失败则为null</returns>
        public Person Login(string account, string password)
        {
            using (DbRepository entities = new DbRepository())
            {
                string md5Password = Core.Util.CryptoHelper.MD5_Encrypt(password);
                var person = entities.Person.Where(x => x.Flag == (long)GlobalFlag.Normal).FirstOrDefault(x => x.Password == md5Password && x.Account.Equals(account));
                return person;
            }
        }
    }
}
