using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MPUtil.ServiceMsg.Message
{
    /// <summary>
    /// 图文消息
    /// </summary>
    public class ServiceNewsMsg : ServiceMessage
    {
        /// <summary>
        /// 多条图文消息
        /// </summary>
        public List<ServiceNewsMsgItem> Articles { get; set; }

        public string Reverse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.AppendFormat("\"touser\":\"{0}\",", this.ToUser);
            sb.Append("\"msgtype\":\"news\",");
            sb.Append("\"news\":{");
            sb.Append("\"articles\": [");
            for (int i = 0; i < this.Articles.Count; i++)
            {
                if (i == 0)
                    sb.Append("{");
                else
                    sb.Append(",{");
                sb.AppendFormat("\"title\":\"{0}\",", this.Articles[i].Title);
                sb.AppendFormat("\"description\":\"{0}\",", this.Articles[i].Description);
                sb.AppendFormat("\"url\":\"{0}\",", this.Articles[i].Url);
                sb.AppendFormat("\"picurl\":\"{0}\"", this.Articles[i].PicUrl);
                sb.Append("}");
            }
            sb.Append("]");
            sb.Append(" }");
            sb.Append("}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 图文消息
    /// </summary>
    public class ServiceNewsMsgItem : ServiceMessage
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 点击后跳转的链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80
        /// </summary>
        public string PicUrl { get; set; }
    }
}