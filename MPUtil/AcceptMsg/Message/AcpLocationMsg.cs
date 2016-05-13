using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg.Message
{
    /// <summary>
    /// 地理消息
    /// </summary>
    public class AcpLocationMsg:AcceptMessage
    {
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public string Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }

        public AcpLocationMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.地理位置消息;
            this.MsgId = root.SelectSingleNode("MsgId").InnerText;
            this.Location_X = root.SelectSingleNode("Location_X").InnerText;
            this.Location_Y = root.SelectSingleNode("Location_Y").InnerText;
            this.Scale = root.SelectSingleNode("Scale").InnerText;
            this.Label = root.SelectSingleNode("Label").InnerText;
            return this;
        }
    }
}