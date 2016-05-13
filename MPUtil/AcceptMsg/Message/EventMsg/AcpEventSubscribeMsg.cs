using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg.Message.EventMsg
{
    /// <summary>
    /// 订阅事件消息
    /// </summary>
    public class AcpEventSubscribeMsg : AcpEventMsg
    {
        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }

        public new AcpEventSubscribeMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.事件消息;
            this.Event = Enum_WXAcpEventMsg_EventType.关注;

            XmlNode eventKeyNode = root.SelectSingleNode("EventKey");
            XmlNode ticketNode = root.SelectSingleNode("Ticket");
            if (eventKeyNode != null)
                this.EventKey = eventKeyNode.InnerText;
            if (ticketNode != null)
                this.Ticket = ticketNode.InnerText;

            return this;
        }
    }
}