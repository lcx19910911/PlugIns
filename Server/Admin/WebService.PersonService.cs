using Core.AuthAPI;
using Core.Model;
using EnumPro;
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
        public Person Manager_Person(ResultData source)
        {
            using (DbRepository entities = new DbRepository())
            {

                var entity = entities.Person.FirstOrDefault(x => x.ComId.Equals(source.comid));

                if (entity == null)
                {
                    entity = new Person();
                    entity.UNID = Guid.NewGuid().ToString("N");
                    entity.CreatedTime = DateTime.Now;
                    entity.UpdatedTime = DateTime.Now;
                    entity.Flag = (long)GlobalFlag.Normal;
                    entity.ComId = source.comid;
                    entity.IsChildren = 0;
                    entities.Person.Add(entity);

                    entities.SaveChanges();
                }

                return entity;
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
                var person = entities.Person.Where(x => x.Flag == (long)GlobalFlag.Normal).FirstOrDefault(x => x.Password == md5Password && x.Account.Equals(account)&&x.IsChildren==1);
                return person;
            }
        }
    }
}
