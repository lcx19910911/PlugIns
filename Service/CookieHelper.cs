using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Extensions;
using Repository;
using Core.Model;
using MPUtil.UserMng;
using Core.Helper;
using Core;
using Model;

namespace Service
{
    public class CookieHelper
    {

        //更新用户的cookie
        public static void CreateWxUser(WXUser user)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(Params.UserCookieName);
                using (DbRepository entities = new DbRepository())
                {
                    cookie.Value = CryptoHelper.AES_Encrypt(user.ToJson(), Params.SecretKey);
                    cookie.Expires = DateTime.Now.AddMinutes(120);
                }
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
        public static WXUser GetCurrentWxUser()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[Params.UserCookieName];
            if (cookie == null)
                return null;
            WXUser user = (CryptoHelper.AES_Decrypt(cookie.Value, Params.SecretKey)).DeserializeJson<WXUser>();
            return user;
        }

        //更新用户的cookie
        public static void CreatePeople(Person person)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(Params.PeopleCookieName);
                cookie.Value = CryptoHelper.AES_Encrypt(person.ToJson(), Params.SecretKey);
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
        public static Person GetCurrentPeople()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[Params.PeopleCookieName];
            if (cookie == null)
                return null;
            string value = CryptoHelper.AES_Decrypt(cookie.Value, Params.SecretKey);
            return value.DeserializeJson<Person>();
        }


        //更新用户的cookie
        public static void CreateShopId(string shopId)
        {
            try
            {
                HttpCookie cookie = new HttpCookie("shop_id");
                cookie.Value = shopId;
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
        public static string GetCurrentShopId()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["shop_id"];
            if (cookie == null)
                return "";
            return cookie.Value;
        }
    }
}
