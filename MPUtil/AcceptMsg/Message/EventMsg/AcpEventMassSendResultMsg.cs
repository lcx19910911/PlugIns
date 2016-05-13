using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Core.Extensions;

namespace MPUtil.AcceptMsg.Message.EventMsg
{
    /// <summary>
    /// 事件推送群发结果
    /// </summary>
    public class AcpEventMassSendResultMsg : AcpEventMsg
    {
        /// <summary>
        /// 群发的消息ID
        /// </summary>
        public int MsgID { get; set; }
        /// <summary>
        /// 群发的结构，为“send success”或“send fail”或“err(num)”
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 发送的总人数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 过滤后，准备发送的粉丝数
        /// </summary>
        public int FilterCount { get; set; }
        /// <summary>
        /// 发送成功的粉丝数
        /// </summary>
        public int SentCount { get; set; }
        /// <summary>
        /// 发送失败的粉丝数
        /// </summary>
        public int ErrorCount { get; set; }

        public new AcpEventMassSendResultMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.事件消息;
            this.Event = Enum_WXAcpEventMsg_EventType.推送群发结果;
            this.MsgID = root.SelectSingleNode("MsgID").InnerText.GetInt();
            this.Status = root.SelectSingleNode("Status").InnerText;
            this.TotalCount = root.SelectSingleNode("TotalCount").InnerText.GetInt();
            this.FilterCount = root.SelectSingleNode("FilterCount").InnerText.GetInt();
            this.SentCount = root.SelectSingleNode("SentCount").InnerText.GetInt();
            this.ErrorCount = root.SelectSingleNode("ErrorCount").InnerText.GetInt();
            return this;
        }
    }
}