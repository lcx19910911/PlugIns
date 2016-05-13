using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MPUtil.ServiceMsg.Message
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class ServiceTextMsg : ServiceMessage
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

        public new string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",",this.ToUser);
            sb.Append("\"msgtype\":\"text\",");
            sb.Append("\"text\":");
            sb.Append("{");
            sb.AppendFormat("\"content\":\"{0}\"",this.Content);
            sb.Append("}");
            sb.Append("}");
            return sb.ToString();
        }
    }
}