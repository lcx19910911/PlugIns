using  EnumPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Code
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
        public LoginUser(string UNID, string Account, string Name, string ComId,string ShopId,bool IsChildren)
        {
            this.UNID = UNID;
            this.Account = Account;
            this.Name = Name;
            this.ComId = ComId;
            this.ShopId = ShopId;
            this.IsChildren = IsChildren;
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
