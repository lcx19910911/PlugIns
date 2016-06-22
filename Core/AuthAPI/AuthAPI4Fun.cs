using Newtonsoft.Json.Linq;
using System;
using System.Web.Configuration;
using System.Web.Security;
using Core.Extensions;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Core.AuthAPI
{
    public class AuthAPI4Fun
    {
        
        /// <summary>
        /// 检查应用接入的数据完整性
        /// </summary>
        /// <param name="signature">加密签名内容</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机字符串</param>
        /// <param name="appid">应用接入Id</param>
        /// <returns></returns>
        public static CheckResult ValidateSignature(string signature, string timestamp, string nonce)
        {
            CheckResult result = new CheckResult();
            result.msg = "数据完整性检查不通过";

            #region 校验签名参数的来源是否正确
            string[] ArrTmp = { Params.SecretKey, timestamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();

            if (tmpStr == signature && timestamp.IsNumeric())
            {
                DateTime dtTime =ParseTimeMillis(long.Parse(timestamp));
                double minutes = DateTime.Now.Subtract(dtTime).TotalMinutes;
                if (minutes > Params.TimspanExpiredMinutes)
                {
                    result.msg = "签名时间戳失效";
                }
                else
                {
                    result.msg = "";
                    result.code = 100;
                    //result.channel = channelInfo.Channel;
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 将时间戳转换为日期
        /// </summary>
        /// <param name="millis"></param>
        /// <returns></returns>
        public static DateTime ParseTimeMillis(long millis)
        {
            TimeSpan ts = new TimeSpan(millis * 10000);
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return startTime.Add(ts);
        }


        /// <summary>
        /// 检查用户的Token有效性
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static CheckResult ValidateToken(string token)
        {
            //返回的结果对象
            CheckResult result = new CheckResult();
            result.msg = "令牌检查不通过";

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    string decodedJwt = JsonWebToken.Decode(token, Params.SecretKey);
                    if (!string.IsNullOrEmpty(decodedJwt))
                    {
                        #region 检查令牌对象内容
                        dynamic root = JObject.Parse(decodedJwt);
                        int accountid = (int)root.accountid;
                        int uid = (int)root.uid;
                        int jwtcreated = (int)root.iat;

                        //检查令牌的有效期，7天内有效
                        TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                        int timestamp = (int)t.TotalDays;
                        if (timestamp - jwtcreated > Params.ExpiredDays)
                        {
                            throw new ArgumentException("用户令牌失效.");
                        }

                        //成功校验
                        result.code = 100;
                        result.msg = "";
                        TokenInfo tokeninfo = new TokenInfo();
                        tokeninfo.AccountID = accountid;
                        tokeninfo.UID = uid;

                        result.tokenInfo = tokeninfo;

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    //LogTextHelper.Error(ex);
                }
            }
            return result;
        }

        /// <summary>
        /// 生成签名字符串
        /// </summary>
        /// <param name="appSecret">接入秘钥</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        public static string SignatureString(string appSecret, string timestamp, string nonce)
        {
            string[] ArrTmp = { appSecret, timestamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            return tmpStr.ToLower();
        }

        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static APIResult Login(string username, string password)
        {
            APIResult result = new APIResult();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Params.LoginUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset:utf-8";

                long timestamp = GetCurrentTimeMillis();
                string nonce = new Random().NextDouble().ToString();
                string signature = SignatureString(Params.SecretKey, timestamp.ToString(), nonce);

                Dictionary<string, string> postParameters = new Dictionary<string, string>();
                postParameters.Add("username", username);
                postParameters.Add("password", password);
                postParameters.Add("timestamp", timestamp.ToString()); 
                postParameters.Add("nonce", nonce);
                postParameters.Add("signature", signature);

                StringBuilder paraStrBuilder = new StringBuilder();
                foreach (string key in postParameters.Keys)
                {
                    paraStrBuilder.AppendFormat("{0}={1}&", key, postParameters[key]);
                }
                string para = paraStrBuilder.ToString();
                if (para.EndsWith("&"))
                    para = para.Remove(para.Length - 1, 1);
                //编码要跟服务器编码统一
                byte[] bt = Encoding.UTF8.GetBytes(para);
                string responseData = String.Empty;

                request.ContentLength = bt.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bt, 0, bt.Length);
                }
                var response = request.GetResponse() as HttpWebResponse;
                string html = WebResponseGet(response);
                result = html.DeserializeJson<APIResult>();
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public static long GetCurrentTimeMillis()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(DateTime.Now - startTime).TotalMilliseconds;
        }


        public static string WebResponseGet(HttpWebResponse webResponse)
        {
            StreamReader responseReader = null;
            string responseData = "";
            try
            {
                responseReader = new StreamReader(webResponse.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webResponse.GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }
            return responseData;
        }
    }
}
