using Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Core.Web;

namespace MPUtil
{
    /// <summary>
    /// 素材管理
    /// </summary>
    public class MaterialMng
    {
        #region 添加永久素材(图文、视频除外)
        /// <summary>
        /// 添加永久素材(图文、视频除外)
        /// 返回值：{"media_id":MEDIA_ID} 或者 {"errcode":错误代码,"errmsg":错误消息}
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="filePath"></param>
        /// <param name="type">image,voice</param>
        /// <returns></returns>
        public Hashtable AddMaterialForever(string accessToken, string filePath, string type)
        {
            string wxurl = string.Format("https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={0}", accessToken);
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("type",type);
            string result = HttpHelper.FilePostRequest(wxurl, filePath, "media", nvc);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();
            if (resHash["errcode"].GetInt() != 0)
            {
                resHash["errmsg"] = ErrorCode.GetErrmsg(resHash["errcode"].ToString());
            }
            return resHash;
        }
        #endregion 

        #region 添加永久视频素材
        /// <summary>
        /// 添加永久视频素材
        /// 返回值：{"media_id":MEDIA_ID,"url":URL} 或者 {"errcode":错误代码,"errmsg":错误消息}
        /// </summary>
        /// <param name="accessToken">公众号的全局唯一票据</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="title">标题</param>
        /// <param name="introduction">介绍</param>
        /// <returns></returns>
        public Hashtable AddVideoForever(string accessToken, string filePath, string title, string introduction)
        {
            string wxurl = string.Format("https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={0}", accessToken);

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("type","video");
            nvc.Add("description", "{\"title\":\"" + title + "\",\"introduction\":\"" + introduction + "\"}");
            string result = HttpHelper.FilePostRequest(wxurl, filePath, "media", nvc);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();
            return resHash;       
        }
        #endregion

        #region 删除永久素材
        /// <summary>
        /// 删除永久素材
        /// 返回值：{"errcode":0,"errmsg":错误消息 }
        /// 调用成功时，errcode将为0
        /// </summary>
        /// <param name="accesToken"></param>
        /// <param name="mediaId">微信素材ID</param>
        /// <returns></returns>
        public Hashtable DelMaterial(string accesToken, string mediaId)
        {
            string wxurl = "https://api.weixin.qq.com/cgi-bin/material/del_material?access_token=" + accesToken;
            string result = HttpHelper.ServerPostRequest(wxurl, "media_id=" + mediaId);
            Hashtable resHash =  result.DeserializeJson<Hashtable>();
            return resHash;
        }
        #endregion
    }
}
