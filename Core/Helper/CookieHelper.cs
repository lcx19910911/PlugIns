using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Extensions;
using Repository;
using Core.Model;

namespace Core.Helper
{
    public class CookieHelper
    {

        //更新用户的cookie
        public static void CreateLoginCookie(Person person)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(Params.CookieName);
                var obj = new LoginUser(person);
                cookie.Value = CryptoHelper.AES_Encrypt(obj.ToJson(), Params.SecretKey);
                cookie.Expires = DateTime.Now.AddMinutes(120);
                // 写登录Cookie
                HttpContext.Current.Response.Cookies.Remove(cookie.Name);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch { }
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public static LoginUser GetCurrentUser()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[Params.CookieName];
            if (cookie == null)
                return null;
            LoginUser user = (CryptoHelper.AES_Decrypt(cookie.Value, Params.SecretKey)).DeserializeJson<LoginUser>();
            return user;
        }



        /// <summary>
        /// 判断是否已经登录
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[Params.CookieName];
            if (cookie != null && cookie.Value.IsNotNullOrEmpty())
                return true;
            return false;
        }
    }
}
