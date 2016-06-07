using MPUtil.MsgCrypt;
using MPUtil.ReplyMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Core.Web;
using Core.Extensions;

namespace MPUtil
{
    /// <summary>
    /// 基础支持
    /// </summary>
    public class BaseFunctions
    {
        #region 获取AccessToken
        /// <summary>
        /// 获取AccessToken
        /// 返回Hashtable对象：{"access_token":"ACCESS_TOKEN","expires_in":7200} 或者 {"errcode":40013,"errmsg":"invalid appid"}
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static Hashtable GetAccessToken(string appid, string secret)
        {
            try
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
                string result = HttpHelper.GetReponseText(url);
                Hashtable resHash = result.DeserializeJson<Hashtable>();
                return resHash;
            }
            catch (Exception ex)
            {
                Hashtable retHash = new Hashtable();
                retHash.Add("errcode", "-1");
                retHash.Add("errmsg", "系统错误");
                return retHash;
            }
        }
        #endregion


        #region 获取AccessToken
        /// <summary>
        /// 获取AccessToken
        /// 返回Hashtable对象：{"access_token":"ACCESS_TOKEN","expires_in":7200} 或者 {"errcode":40013,"errmsg":"invalid appid"}
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static Hashtable GetAccessToken(string code,string appid, string secret)
        {
            try
            {
                string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code=CODE&grant_type=authorization_code", appid, secret);
                string result = HttpHelper.GetReponseText(url);
                Hashtable resHash = result.DeserializeJson<Hashtable>();
                return resHash;
            }
            catch (Exception ex)
            {
                Hashtable retHash = new Hashtable();
                retHash.Add("errcode", "-1");
                retHash.Add("errmsg", "系统错误");
                return retHash;
            }
        }
        #endregion

        #region 获取jsapi_ticket
        /// <summary>
        /// 获取jsapi_ticket
        /// 返回Hashtable对象：{"errcode":0,"errmsg":"ok","ticket":"bxLdikRXVbTPdHSM05e5u5sUoXNKd8-41ZO3MhKoyN5OfkWITDGgnr2fwJ0m9E8NYzWKVZvdVtaUgWvsdshFKA","expires_in":7200}
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static Hashtable GetJSApiTicket(string accessToken)
        {
            try
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", accessToken);
                string result = HttpHelper.GetReponseText(url);
                Hashtable resHash =  result.DeserializeJson<Hashtable>();
                return resHash;
            }
            catch (Exception ex)
            {
                Hashtable retHash = new Hashtable();
                retHash.Add("errcode", "-1");
                retHash.Add("errmsg", "系统错误");
                return retHash;
            }
        }
        #endregion

        #region 获取微信服务器IP地址
        /// <summary>
        /// 获取微信服务器IP地址
        /// 返回值：{ "ip_list":["127.0.0.1","127.0.0.1"] }  或者  {"errcode":40013,"errmsg":"invalid appid"}
        /// </summary>
        /// <param name="acctoken"></param>
        /// <returns></returns>
        public Hashtable GetWeiXinServerIP(string acctoken)
        {
            try
            {
                string url = "https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token=" + acctoken;
                string result = HttpHelper.GetReponseText(url);
                Hashtable resHash =  result.DeserializeJson<Hashtable>();
                return resHash;
            }
            catch (Exception ex)
            {
                Hashtable retHash = new Hashtable();
                retHash.Add("errcode", -1);
                retHash.Add("errmsg", "系统错误");
                return retHash;
            }
        }
        #endregion

        #region 消息加密
        /// <summary>
        /// 消息加密
        /// </summary>
        /// <param name="fromUserName">发送人</param>
        /// <param name="token">开发者Token</param>
        /// <param name="appid">公众号AppId</param>
        /// <param name="strMsg">消息(xml格式)</param>
        /// <param name="aesKey">用于加密的EncodingAESKey</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns></returns>
        public static string EncryptMsg(string fromUserName,string token,string appid, string strMsg, string aesKey, string timestamp, string nonce)
        {
            try
            {
                string encryptReplyMsgStr = string.Empty;   //  回复的消息密文
                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(token, aesKey, appid);
                int encryptRes = wxcpt.EncryptMsg(strMsg, timestamp, nonce, ref encryptReplyMsgStr);    //  消息加密
                if (encryptRes != 0)
                    return "";
                return encryptReplyMsgStr;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        #endregion
    }
}