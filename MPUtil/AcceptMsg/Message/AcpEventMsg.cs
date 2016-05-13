using MPUtil.AcceptMsg.Message.EventMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg.Message
{
    #region 事件类型
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum Enum_WXAcpEventMsg_EventType
    {
        未知 = 0,
        关注 = 1,
        取消关注 = 2,
        扫描 = 3,
        点击 = 4,
        浏览 = 5,
        上报地理位置 = 6,
        推送群发结果 = 7,
        扫码推事件=8,
        扫码且等待提示框的事件推送=9,
        弹出系统拍照发图的事件推送=10,
        弹出拍照或者相册发图的事件推送=11,
        弹出微信相册发图器的事件推送=12,
        弹出地理位置选择器的事件推送=13
    }
    #endregion

    /// <summary>
    /// 事件消息
    /// </summary>
    public class AcpEventMsg : AcceptMessage
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public Enum_WXAcpEventMsg_EventType Event { get; set; }

        public AcpEventMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            string eventStr = root.SelectSingleNode("Event").InnerText;
            switch (eventStr)
            {
                case "subscribe":
                    return new AcpEventSubscribeMsg().Convert(xmlmsg);
                case "unsubscribe":
                    return new AcpEventUnsubscribeMsg().Convert(xmlmsg);
                case "SCAN":
                    return new AcpEventScanMsg().Convert(xmlmsg);
                case "LOCATION":
                    return new AcpEventLocationMsg().Convert(xmlmsg);
                case "CLICK":
                    return new AcpEventClickMsg().Convert(xmlmsg);
                case "VIEW":
                    return new AcpEventViewMsg().Convert(xmlmsg);
                case "MASSSENDJOBFINISH":
                    return new AcpEventMassSendResultMsg().Convert(xmlmsg);
                default:
                    this.Event = Enum_WXAcpEventMsg_EventType.未知;
                    break;
            }
            return this;
        }
    }
}