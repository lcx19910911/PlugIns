using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MPUtil.ServiceMsg.Message
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class ServiceVideoMsg : ServiceMessage
    {
        /// <summary>
        /// 发送的视频的媒体ID
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 缩略图的媒体ID
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 视频消息的标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 视频消息的描述
        /// </summary>
        public string Description { get; set; }

        public new string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", this.ToUser);
            sb.Append("\"msgtype\":\"video\",");
            sb.Append("\"video\":");
            sb.Append("{");
            sb.AppendFormat("\"media_id\":\"{0}\"", this.MediaId);
            sb.AppendFormat("\"thumb_media_id\":\"{0}\",",this.ThumbMediaId);
            sb.AppendFormat("\"title\":\"{0}\",",this.Title);
            sb.AppendFormat("\"description\":\"{0}\"",this.Description);
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }
    }
}