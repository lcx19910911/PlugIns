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
        public LoginUser(string UNID, string Account, string Name, string Mobile, long MenuLimitFlag, TargetCode targetCode,string TargetID)
        {
            this.UNID = UNID;
            this.Account = Account;
            this.Name = Name;
            this.Mobile = Mobile;
            this.MenuLimitFlag = MenuLimitFlag;
            this.TargetCode = targetCode;
            this.TargetID = TargetID;
        }

        public TargetCode TargetCode { get; set; }


        public string TargetID { get; set; }

        /// <summary>
        /// 公众号appid
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }

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
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 菜单权限集合
        /// </summary>
        public long MenuLimitFlag { get; set; }
    }
}
