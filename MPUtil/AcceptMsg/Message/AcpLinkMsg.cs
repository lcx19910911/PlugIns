using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg.Message
{
    /// <summary>
    /// 链接消息
    /// </summary>
    public class AcpLinkMsg:AcceptMessage
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }

        public AcpLinkMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.链接消息;
            this.MsgId = root.SelectSingleNode("MsgId").InnerText;
            this.Title = root.SelectSingleNode("Title").InnerText;
            this.Description = root.SelectSingleNode("Description").InnerText;
            this.Url = root.SelectSingleNode("Url").InnerText;
            return this;
        } 
    }
}