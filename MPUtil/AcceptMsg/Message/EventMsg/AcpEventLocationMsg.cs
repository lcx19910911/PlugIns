using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg.Message.EventMsg
{
    /// <summary>
    /// 上报地理位置事件消息
    /// </summary>
    public class AcpEventLocationMsg : AcpEventMsg
    {
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        public string Precision { get; set; }

        public new AcpEventLocationMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.事件消息;
            this.Event = Enum_WXAcpEventMsg_EventType.上报地理位置;
            this.Latitude = root.SelectSingleNode("Latitude").InnerText;
            this.Longitude = root.SelectSingleNode("Longitude").InnerText;
            this.Precision = root.SelectSingleNode("Precision").InnerText;
            return this;
        }
    }
}