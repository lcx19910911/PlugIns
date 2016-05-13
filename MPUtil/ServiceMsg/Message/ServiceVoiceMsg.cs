using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MPUtil.ServiceMsg.Message
{
    /// <summary>
    /// 语音消息
    /// </summary>
    public class ServiceVoiceMsg : ServiceMessage
    {
        /// <summary>
        /// 发送的语音的媒体ID
        /// </summary>
        public string MediaId { get; set; }

        public new string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", this.ToUser);
            sb.Append("\"msgtype\":\"voice\",");
            sb.Append("\"voice\":");
            sb.Append("{");
            sb.AppendFormat("\"media_id\":\"{0}\"", this.MediaId);
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }
    }
}