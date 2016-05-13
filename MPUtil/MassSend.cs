using Core.Extensions;
using Core.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace MPUtil
{
    /// <summary>
    /// 高级群发消息
    /// </summary>
    public class MassSend
    {
        #region 上传图文消息素材
        /// <summary>
        /// 上传图文消息素材
        /// 返回Hashtable对象：{"type":"news","media_id":"CsEf3ldqkAYJAU6EJeIkStVDSvffUJ54vqbThMgplD-VJXXof6ctX5fI6-aYyUiQ","created_at":1391857799,"state":"success","errmsg":"系统错误"}
        /// state:"success","error"分别代表成功或者失败
        /// </summary>
        /// <param name="ACCESS_TOKEN"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public Hashtable UploadNews(string ACCESS_TOKEN, List<UploadNewsItem> items)
        {
            Hashtable retHash = new Hashtable();
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("articles", items);
                string jsonStr = hash.ToJson();
                string wxurl = string.Format("https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}", ACCESS_TOKEN);

                string result = HttpHelper.ServerPostRequest(wxurl, jsonStr);
                Hashtable resHash =  result.DeserializeJson<Hashtable>();
                if (resHash.ContainsKey("errcode"))
                {
                    retHash.Add("state", "error");
                    retHash.Add("errmsg", resHash["errmsg"]);
                }
                else
                {
                    retHash.Add("state", "success");
                    retHash.Add("type", resHash["type"]);
                    retHash.Add("media_id", resHash["media_id"]);
                    retHash.Add("created_at", resHash["created_at"]);
                }
                return retHash;
            }
            catch (Exception ex)
            {
                retHash.Add("state", "error");
                retHash.Add("errmsg", "系统错误");
                return retHash;
            }

        }
        #endregion

        #region 上传用于群发的视频
        /// <summary>
        /// 上传用于群发的视频
        /// 返回Hashtable对象：{"type":"news","media_id":"CsEf3ldqkAYJAU6EJeIkStVDSvffUJ54vqbThMgplD-VJXXof6ctX5fI6-aYyUiQ","created_at":1391857799,"state":"success","errmsg":"系统错误"}
        /// state:"success","error"分别代表成功或者失败
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId">media_id需通过基础支持中的上传下载多媒体文件来得到</param>
        /// <param name="title">标题</param>
        /// <param name="desc">描述</param>
        public Hashtable UploadVideo(string accessToken, string mediaId, string title, string desc)
        {
            Hashtable retHash = new Hashtable();
            try
            {
                string wxurl = string.Format("https://file.api.weixin.qq.com/cgi-bin/media/uploadvideo?access_token={0}", accessToken);
                StringBuilder postData = new StringBuilder();
                postData.Append("{");
                postData.AppendFormat("\"media_id\": \"{0}\",", mediaId);
                postData.AppendFormat("\"title\": \"{0}\",", title);
                postData.AppendFormat("\"description\": \"{0}\"", desc);
                postData.Append("}");
                string result = HttpHelper.ServerPostRequest(wxurl, postData.ToString());
                Hashtable resHash =  result.DeserializeJson<Hashtable>();
                if (resHash.ContainsKey("errcode"))
                {
                    retHash.Add("state", "error");
                    retHash.Add("errmsg", resHash["errmsg"]);
                }
                else
                {
                    retHash.Add("state", "success");
                    retHash.Add("type", resHash["type"]);
                    retHash.Add("media_id", resHash["media_id"]);
                    retHash.Add("created_at", resHash["created_at"]);
                }
                return retHash;
            }
            catch (Exception ex)
            {
                retHash.Add("state", "error");
                retHash.Add("errmsg", "系统错误");
                return retHash;
            }
        }
        #endregion

        #region 根据分组进行群发
        /// <summary>
        /// 根据分组进行群发
        /// 返回Hashtable对象:{"state":"success","errmsg":"send job submission success","msg_id":34182}
        /// state:"success","error"分别代表成功或者失败
        /// </summary>
        /// <param name="groupId">群发到的分组的group_id</param>
        /// <param name="accessToken"></param>
        /// <param name="mediaId">用于群发的消息的media_id</param>
        /// <param name="type">消息类型</param>
        /// <param name="content">文本消息的内容（可为空）</param>
        /// <param name="videoTitle">视频标题（可为空）</param>
        /// <param name="videoDesc">视频描述（可为空）</param>
        public Hashtable SendByGroup(int groupId, string accessToken, string mediaId, Enum_WXMassSend_Msg_Type type, string content, string videoTitle, string videoDesc)
        {
            Hashtable retHash = new Hashtable();

            string wxurl = string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}", accessToken);
            StringBuilder postdata = new StringBuilder();
            switch (type)
            {
                case Enum_WXMassSend_Msg_Type.图文:
                    postdata.Append("{");
                    postdata.Append("\"filter\":{");
                    postdata.AppendFormat("\"group_id\":\"{0}\"", groupId);
                    postdata.Append("},");
                    postdata.Append("\"mpnews\":{");
                    postdata.AppendFormat("\"media_id\":\"{0}\"", mediaId);
                    postdata.Append("},");
                    postdata.Append("\"msgtype\":\"mpnews\"");
                    postdata.Append("}");
                    break;
                case Enum_WXMassSend_Msg_Type.文本:
                    postdata.Append("{");
                    postdata.Append("\"filter\":{");
                    postdata.AppendFormat("\"group_id\":\"{0}\"", groupId);
                    postdata.Append("},");
                    postdata.Append("\"text\":{");
                    postdata.AppendFormat("\"content\":\"{0}\"", content);
                    postdata.Append("},");
                    postdata.Append("\"msgtype\":\"text\"");
                    postdata.Append("}");
                    break;
                case Enum_WXMassSend_Msg_Type.语音:
                    postdata.Append("{");
                    postdata.Append("\"filter\":{");
                    postdata.AppendFormat("\"group_id\":\"{0}\"", groupId);
                    postdata.Append("},");
                    postdata.Append("\"voice\":{");
                    postdata.AppendFormat("\"media_id\":\"{0}\"", mediaId);
                    postdata.Append("},");
                    postdata.Append("\"msgtype\":\"voice\"");
                    postdata.Append("}");
                    break;
                case Enum_WXMassSend_Msg_Type.图片:
                    postdata.Append("{");
                    postdata.Append("\"filter\":{");
                    postdata.AppendFormat("\"group_id\":\"{0}\"", groupId);
                    postdata.Append("},");
                    postdata.Append("\"image\":{");
                    postdata.AppendFormat("\"media_id\":\"{0}\"", mediaId);
                    postdata.Append("},");
                    postdata.Append("\"msgtype\":\"image\"");
                    postdata.Append("}");
                    break;
                case Enum_WXMassSend_Msg_Type.视频:
                    postdata.Append("{");
                    postdata.Append("\"filter\":{");
                    postdata.AppendFormat("\"group_id\":\"{0}\"", groupId);
                    postdata.Append("},");
                    postdata.Append("\"mpvideo\":{");
                    postdata.AppendFormat("\"media_id\":\"{0}\",", mediaId);
                    postdata.Append("},");
                    postdata.Append("\"msgtype\":\"mpvideo\"");
                    postdata.Append("}");
                    break;
                default:
                    retHash.Add("state", "error");
                    retHash.Add("errmsg", "未知的消息类型");
                    break;
            }
            if (postdata.Length > 0)
            {
                string result = HttpHelper.ServerPostRequest(wxurl, postdata.ToString());
                Hashtable resHash =  result.DeserializeJson<Hashtable>();
                if (resHash["errcode"].GetInt() == 0)
                {
                    retHash["state"] = "success";
                    retHash["msg_id"] = resHash["msg_id"];
                }
                else
                {
                    retHash["state"] = "error";
                    retHash["errmsg"] = resHash["errmsg"];
                }
            }
            return retHash;
        }
        #endregion

        #region 根据OpenID列表群发
        /// <summary>
        /// 根据OpenID列表群发
        /// 返回Hashtable对象:{"state":"success","errmsg":"send job submission success","msg_id":34182}
        /// state:"success","error"分别代表成功或者失败
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="mediaId">用于群发的消息的media_id</param>
        /// <param name="openIdArr">OpenID列表，OpenID最少个，最多10000个</param>
        /// <param name="type">消息类型</param>
        /// <param name="content">文本消息的内容（可为空）</param>
        /// <param name="videoTitle">视频标题（可为空）</param>
        /// <param name="videoDesc">视频描述（可为空）</param>
        /// <returns></returns>
        public Hashtable SendByOpenIDList(string accessToken, string mediaId, string[] openIdArr, Enum_WXMassSend_Msg_Type type, string content, string videoTitle, string videoDesc)
        {
            string wxurl = string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}", accessToken);
        
            Hashtable retHash = new Hashtable();
            StringBuilder postData = new StringBuilder();
            switch (type)
            {
                case Enum_WXMassSend_Msg_Type.图文:
                    postData.Append("{");
                    postData.AppendFormat("\"touser\":{0},", openIdArr.ToJson());
                    postData.Append("\"mpnews\":{");
                    postData.AppendFormat("\"media_id\":\"{0}\"", mediaId);
                    postData.Append("},");
                    postData.Append("\"msgtype\":\"mpnews\"");
                    postData.Append("}");
                    break;
                case Enum_WXMassSend_Msg_Type.文本:
                    postData.Append("{");
                    postData.AppendFormat("\"touser\": {0},", openIdArr.ToJson());
                    postData.AppendFormat("\"msgtype\": \"text\", \"text\": { \"content\": \"{0}\"}", content);
                    postData.Append("}");
                    break;
                case Enum_WXMassSend_Msg_Type.语音:
                    postData.Append("{");
                    postData.AppendFormat("\"touser\":{0},", openIdArr.ToJson());
                    postData.Append("\"voice\":{");
                    postData.AppendFormat("\"media_id\":\"{0}\"", mediaId);
                    postData.Append(" },");
                    postData.Append("\"msgtype\":\"voice\"");
                    postData.Append("}");
                    break;
                case Enum_WXMassSend_Msg_Type.图片:
                    postData.Append("{");
                    postData.AppendFormat("\"touser\":{0},", openIdArr.ToJson());
                    postData.Append("\"image\":{");
                    postData.AppendFormat("\"media_id\":\"{0}\"", mediaId);
                    postData.Append(" },");
                    postData.Append("\"msgtype\":\"image\"");
                    postData.Append("}");
                    break;
                case Enum_WXMassSend_Msg_Type.视频:
                    postData.Append("{");
                    postData.AppendFormat("\"touser\":{0},", openIdArr.ToJson());
                    postData.Append("\"video\":{");
                    postData.AppendFormat("\"media_id\":\"{0}\",", mediaId);
                    postData.AppendFormat("\"title\":\"{0}\",", videoTitle);
                    postData.AppendFormat("\"description\":\"{0}\"", videoDesc);
                    postData.Append("},");
                    postData.Append("\"msgtype\":\"video\"");
                    postData.Append("}");
                    break;
                default:
                    retHash.Add("state", "error");
                    retHash.Add("errmsg", "未知的消息类型");
                    break;
            }
            if (postData.Length > 0)
            {
                string result = HttpHelper.ServerPostRequest(wxurl, postData.ToString());
                Hashtable resHash =  result.DeserializeJson<Hashtable>();
                if (resHash["errcode"].GetInt() == 0)
                {
                    retHash["state"] = "success";
                    retHash["msg_id"] = resHash["msg_id"];
                }
                else
                {
                    retHash["state"] = "error";
                    retHash["errmsg"] = resHash["errmsg"];
                }
            }
            return retHash;
        }
        #endregion

        #region 删除群发消息
        /// <summary>
        /// 删除群发消息
        /// 返回Hashtable对象：{"state":"success","errmsg":"send job submission success","errcode":34182}
        /// state:"success","error"分别代表成功或者失败
        /// </summary>
        /// <param name="msgId">消息ID</param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public Hashtable DeleteMsg(int msgId,string accessToken)
        {
            Hashtable retHash = new Hashtable();

            string wxurl = string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/delete?access_token={0}",accessToken);
            string postdata = "{\"msg_id\":" + msgId + "}";
            string result = HttpHelper.ServerPostRequest(wxurl,postdata);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();

            if (resHash["errcode"].GetInt() == 0)
            {
                retHash.Add("state","success");
            }
            else
            {
                retHash.Add("state","error");
                retHash.Add("errcode", resHash["errcode"]);
                retHash.Add("errmsg",resHash["errmsg"]);
            }
            return retHash;
        }
        #endregion 
    }

    #region 群发消息的类型
    /// <summary>
    /// 群发消息的类型
    /// </summary>
    public enum Enum_WXMassSend_Msg_Type
    {
        文本 = 1,
        图文,
        语音,
        图片,
        视频
    }
    #endregion

    #region 图文信息群发的项
    /// <summary>
    /// 图文信息群发的项
    /// </summary>
    public class UploadNewsItem
    {
        /// <summary>
        /// 图文消息缩略图的media_id，可以在基础支持-上传多媒体文件接口中获得
        /// </summary>
        public string thumb_media_id { get; set; }
        /// <summary>
        /// 图文消息的作者
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 图文消息的标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 在图文消息页面点击“阅读原文”后的页面
        /// </summary>
        public string content_source_url { get; set; }
        /// <summary>
        /// 图文消息页面的内容，支持HTML标签
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 图文消息的描述
        /// </summary>
        public string digest { get; set; }
        /// <summary>
        /// 是否显示封面，1为显示，0为不显示
        /// </summary>
        public string show_cover_pic { get; set; }
    }
    #endregion
}