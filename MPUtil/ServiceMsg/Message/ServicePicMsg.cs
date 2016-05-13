using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MPUtil.ServiceMsg.Message
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class ServicePicMsg : ServiceMessage
    {
        /// <summary>
        /// 发送的图片的媒体ID
        /// </summary>
        public string MediaId { get; set; }

        public new string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", this.ToUser);
            sb.Append("\"msgtype\":\"image\",");
            sb.Append("\"image\":");
            sb.Append("{");
            sb.AppendFormat("\"media_id\":\"{0}\"",this.MediaId);
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }
    }
}