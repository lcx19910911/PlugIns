using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;


namespace MPUtil.AcceptMsg.Message
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class AcpTextMsg : AcceptMessage
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }

        public AcpTextMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement; 

            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.文本消息;
            this.MsgId = root.SelectSingleNode("MsgId").InnerText;
            this.Content = root.SelectSingleNode("Content").InnerText;
            return this;
        }
    }
}