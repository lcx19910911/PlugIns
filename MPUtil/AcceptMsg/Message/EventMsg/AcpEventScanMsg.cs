using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg.Message.EventMsg
{
    /// <summary>
    /// 扫描事件消息
    /// </summary>
    public class AcpEventScanMsg:AcpEventMsg
    {
        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }


        public new AcpEventScanMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.事件消息;
            this.Event = Enum_WXAcpEventMsg_EventType.扫描;

            XmlNode eventKeyNode = root.SelectSingleNode("EventKey");
            XmlNode ticketNode = root.SelectSingleNode("Ticket");
            this.EventKey = eventKeyNode.InnerText;
            this.Ticket = ticketNode.InnerText;

            return this;
        }

        
    }
}