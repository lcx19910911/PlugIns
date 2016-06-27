using  EnumPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Core.Model
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
        public LoginUser(Person person)
        {
            this.UNID = person.UNID;
            this.Account = person.Account;
            this.Name = person.Name;
            this.ComId = person.ComId;
            this.ShopId = person.ShopId;
            this.IsChildren = person.IsChildren == 1 ? true : false;
        }


        public LoginUser()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string UNID { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 外链Id
        /// </summary>
        public string ComId { get; set; }

        /// <summary>
        /// 店家Id
        /// </summary>
        public string ShopId { get; set; }

        public bool IsChildren { get; set; }
    }
}
