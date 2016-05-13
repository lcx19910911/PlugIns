using Core.Extensions;
using Core.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MPUtil.UserMng
{
    /// <summary>
    /// 关注者方法
    /// </summary>
    public class FansFunction
    {
        /**
         * 当公众号关注者数量超过10000时，可通过填写next_openid的值，从而多次拉取列表的方式来满足需求。
         * 具体而言，就是在调用接口时，将上一次调用得到的返回中的next_openid值，作为下一次调用中的next_openid值。
         **/
        /// <summary>
        /// 获取关注者列表
        /// 返回Hashtable对象：{"errcode":40013,"errmsg":"invalid appid"} 或者 {"total":2,"count":2,"data":{"openid":["","OPENID1","OPENID2"]},"next_openid":"NEXT_OPENID"}
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="nextOpenId">第一个拉取的OPENID，不填默认从头开始拉取</param>
        public Hashtable GetFans(string accessToken, string nextOpenId)
        {
            string wxurl = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}",accessToken,nextOpenId);
            string result = HttpHelper.GetReponseText(wxurl);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();
            return resHash;

        }
    }
}