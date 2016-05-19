using Core.Extensions;
using Core.Model;
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
        /// 更新管理员信息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="loginAccount"></param>
        /// <returns></returns>
        public bool Update_Admin(string appId,string appSecret,string loginAccount)
        {
            if (!appId.IsNotNullOrEmpty()|| !appSecret.IsNotNullOrEmpty()|| !loginAccount.IsNotNullOrEmpty())
                return false;
            using (DbRepository entities = new DbRepository())
            {
                var admin = entities.Admin.FirstOrDefault(x=>x.LoginAccount.Equals(loginAccount));

                if (admin == null)
                {
                    Admin entity = new Admin()
                    {
                        CreatedTime = DateTime.Now,
                        AppId = appId,
                        AppSecret = appSecret,
                        LoginAccount = loginAccount
                    };
                    entities.Admin.Add(entity);
                }
                else
                {
                    admin.AppSecret = appSecret;
                    admin.AppId = appId;
                }
                return entities.SaveChanges() > 0 ? true : false;
            }
        }
    }
}
