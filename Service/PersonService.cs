using Core.AuthAPI;
using Core.Extensions;
using Core.Helper;
using Core.Model;
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
    /// 人员
    /// </summary>
    public class PersonService : BaseService, IPersonService
    {
        public PersonService()
        {
            base.ContextCurrent = HttpContext.Current;
        }



        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Person Login(string account, string password)
        {
            using (DbRepository entities = new DbRepository())
            {
                string md5Password = Core.Util.CryptoHelper.MD5_Encrypt(password);
                var person = entities.Person.Where(x => x.Flag == (long)GlobalFlag.Normal).FirstOrDefault(x => x.Password == md5Password && x.Account.Equals(account)&&x.IsChildren==(int)YesOrNoCode.Yes);
                return person;
            }
        }

        /// <summary>
        /// 管理人员信息
        /// </summary>
        /// <param name="data">接口返回对象</param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Person Manager_Person(ResultData source, string account, string password)
        {
            if (source == null || !account.IsNotNullOrEmpty() || !password.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.Person.FirstOrDefault(x => x.ComId.Equals(source.comid));
                string md5Password = Core.Util.CryptoHelper.MD5_Encrypt(password);
                if (entity==null)
                {
                    entity = new Person()
                    {
                        UNID = Guid.NewGuid().ToString("N"),
                        ComId = source.comid,
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now,
                        Flag = (long)GlobalFlag.Normal,
                        IsChildren = (int)YesOrNoCode.No,
                        Name = source.name,
                        Account= account,
                        Password= md5Password,
                        Remark = "平台用户"
                    };
                    entities.Person.Add(entity);
                }
                return entity;
            }
        }



        /// <summary>
        /// 新增平台人员信息
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="uid">uid</param>
        /// <returns></returns>
        public Person Add_Person(string name, int uid)
        {
            if (!name.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.Person.FirstOrDefault(x => x.ComId.Equals(uid));
                if (entity == null)
                {
                    entity = new Person()
                    {
                        UNID = Guid.NewGuid().ToString("N"),
                        ComId = uid.ToString(),
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now,
                        Flag = (long)GlobalFlag.Normal,
                        IsChildren = (int)YesOrNoCode.No,
                        Name = name,
                        Remark = "平台用户"
                    };
                    entities.Person.Add(entity);
                }
                return entity;
            }
        }


        /// <summary>
        /// 根据平台id登陆
        /// </summary>
        /// <param name="comId">平台id</param>
        /// <returns></returns>
        public Person LoginByComId(int comId)
        {
            using (DbRepository entities = new DbRepository())
            {
                var person = entities.Person.Where(x => x.Flag == (long)GlobalFlag.Normal).FirstOrDefault(x => x.ComId.Equals(comId.ToString())&& x.IsChildren == (int)YesOrNoCode.No);
                return person;
            }
        }
    }
}
