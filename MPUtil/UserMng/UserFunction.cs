using Core.Extensions;
using Core.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MPUtil.UserMng
{
    public class UserFunction
    {
        #region 修改备注名
        /// <summary>
        /// 修改备注名
        /// 返回Hashtable对象：{ "errcode":0, "errmsg":"ok"}
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="remark">新的备注名，长度必须小于30字符</param>
        /// <returns></returns>
        public Hashtable UpdateRemark(string accessToken,string openId,string remark)
        {
            string wxurl="https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token="+accessToken;
            string postdata = string.Format("{\"openid\":\"{0}\",\"remark\":\"{1}\"}",openId,remark);
            Dictionary<string, string> dataDic = new Dictionary<string, string>();
            dataDic.Add("openid", openId);
            dataDic.Add("remark", remark);
            string result = "";
            HttpHelper.TryPostHtml(wxurl, dataDic,out result);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();
            return resHash;
        }
        #endregion 

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// 返回Hashtable对象：{"user": WXUser对象 } 或者 {"errcode":错误代码,"errmsg":错误信息}
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static Hashtable GetInfo(string accessToken, string openId)
        {
            string wxurl = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN",accessToken,openId);
            string result = HttpHelper.GetReponseText(wxurl);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();
            if (resHash.Contains("errcode"))
            {
                return resHash;
            }
            else
            {
                resHash = new Hashtable();
                resHash.Add("user", result.DeserializeJson<WXUser>());
                return resHash;
            }
        }
        #endregion 
    }
}