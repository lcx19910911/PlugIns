using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MPUtil.ServiceMsg.Message
{
    /// <summary>
    /// 音乐消息
    /// </summary>
    public class ServiceMusicMsg : ServiceMessage
    {
        /// <summary>
        /// 音乐链接
        /// </summary>
        public string MusicUrl { get; set; }
        /// <summary>
        /// 高品质音乐链接，wifi环境优先使用该链接播放音乐
        /// </summary>
        public string HQMusicUrl { get; set; }
        /// <summary>
        /// 缩略图的媒体ID
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 音乐标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 音乐描述
        /// </summary>
        public string Description { get; set; }

        public new string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", this.ToUser);
            sb.Append("\"msgtype\":\"music\",");
            sb.Append("\"music\":");
            sb.Append("{");
            sb.AppendFormat("\"title\":\"{0}\",", this.Title);
            sb.AppendFormat("\"description\":\"{0}\"", this.Description);
            sb.AppendFormat("\"musicurl\":\"MUSIC_URL\",", this.MusicUrl);
            sb.AppendFormat("\"hqmusicurl\":\"HQ_MUSIC_URL\",", this.HQMusicUrl);
            sb.AppendFormat("\"thumb_media_id\":\"{0}\",", this.ThumbMediaId);
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }
    }
}