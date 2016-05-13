using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg.Message.EventMsg
{
    /// <summary>
    /// 弹出地理位置选择器的事件推送
    /// </summary>
    public class AcpEventLocationSelectMsg:AcpEventMsg
    {
        /// <summary>
        /// 事件KEY值，由开发者在创建菜单时设定
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// X坐标信息
        /// </summary>
        public string Location_X { get; set; }
        /// <summary>
        /// Y坐标信息
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// 精度，可理解为精度或者比例尺、越精细的话 scale越高
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置的字符串信息
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 朋友圈POI的名字，可能为空
        /// </summary>
        public string Poiname { get; set; }

        public new AcpEventLocationSelectMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.事件消息;
            this.Event = Enum_WXAcpEventMsg_EventType.弹出地理位置选择器的事件推送;
            this.EventKey = root.SelectSingleNode("EventKey").InnerText;

            XmlNode sendLocationNode = root.SelectSingleNode("SendLocationInfo");
            this.Location_X = sendLocationNode.SelectSingleNode("Location_X").InnerText;
            this.Location_Y = sendLocationNode.SelectSingleNode("Location_Y").InnerText;
            this.Scale = sendLocationNode.SelectSingleNode("Scale").InnerText;
            this.Label = sendLocationNode.SelectSingleNode("Label").InnerText;
            this.Poiname = sendLocationNode.SelectSingleNode("Poiname").InnerText;
            return this;
        }
    }
}