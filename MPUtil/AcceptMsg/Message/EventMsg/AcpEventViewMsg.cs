using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg.Message.EventMsg
{
    /// <summary>
    /// 点击菜单跳转链接时的事件推送
    /// </summary>
    public class AcpEventViewMsg : AcpEventMsg
    {
        /// <summary>
        /// 事件KEY值，设置的跳转URL
        /// </summary>
        public string EventKey { get; set; }

        public new AcpEventViewMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.事件消息;
            this.Event = Enum_WXAcpEventMsg_EventType.浏览;
            this.EventKey = root.SelectSingleNode("EventKey").InnerText;

            return this;
        }
    }
}