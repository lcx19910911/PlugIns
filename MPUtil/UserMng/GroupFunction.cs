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
    /// 分组管理
    /// </summary>
    public class GroupFunction
    {
        #region 创建分组
        /// <summary>
        /// 创建分组
        /// 返回Hashtable对象：{"group": {"id": 107,"name": "test"},"errcode":40013,"errmsg":"invalid appid","state":"success"} 或者 {"errcode":40013,"errmsg":"invalid appid"}
        /// </summary>
        /// <param name="accessToken">调用接口凭证</param>
        /// <param name="groupName">分组名字（30个字符以内）</param>
        public Hashtable Create(string accessToken, string groupName)
        {
            string wxurl = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token=" + accessToken;
            string postdata = string.Format("{\"group\":{\"name\":\"{0}\"}}", groupName);

            string result = HttpHelper.ServerPostRequest(wxurl, postdata);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();
            return resHash;
        }
        #endregion

        #region 查询所有分组
        /// <summary>
        /// 查询所有分组
        /// 返回Hashtable对象：{"groups": [{"id": 0,"name": "未分组","count": 72596},{"id": 1,"name": "黑名单","count": 36}]} 或者 {"errcode":40013,"errmsg":"invalid appid"}
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public Hashtable GetAll(string accessToken)
        {
            string wxurl = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token=" + accessToken;
            string result = HttpHelper.GetReponseText(wxurl);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();

            return resHash;
        }
        #endregion

        #region 查询用户所在的分组
        /// <summary>
        /// 查询用户所在的分组
        /// 返回Hashtable对象：{"groupid": 102 } 或者 {"errcode":40013,"errmsg":"invalid appid"}
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        public Hashtable GetGroupId(string accessToken, string openId)
        {
            string wxurl = "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token=" + accessToken;
            string postdata = string.Format("{\"openid\":\"{0}\"}", openId);
            string result = HttpHelper.ServerPostRequest(wxurl,postdata);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();
            return resHash;
        }
        #endregion

        #region 修改分组名
        /// <summary>
        /// 修改分组名
        /// 返回Hashtable对象：{"errcode": 0, "errmsg": "ok"}
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="groupId">分组ID</param>
        /// <param name="newName">分组新名称</param>
        /// <returns></returns>
        public Hashtable UpdateName(string accessToken, int groupId, string newName)
        {
            string wxurl = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token=" + accessToken;
            string postdata = string.Format("{\"group\":{\"id\":{0},\"name\":\"{1}\"}}", groupId, newName);

            string result = HttpHelper.ServerPostRequest(wxurl, postdata);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();
            return resHash;
        }
        #endregion

        #region 移动用户分组
        /// <summary>
        /// 移动用户分组
        /// 返回Hashtable对象：{"errcode": 0, "errmsg": "ok"}
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="toGroupId">分组ID</param>
        /// <returns></returns>
        public Hashtable Move(string accessToken,string openId,int toGroupId)
        {
            string wxurl = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token=" + accessToken;
            string postdata=string.Format("{\"openid\":\"{0}\",\"to_groupid\":{1}}",openId,toGroupId);

            string result = HttpHelper.ServerPostRequest(wxurl, postdata);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();
            return resHash;
        }
        #endregion
    }
}