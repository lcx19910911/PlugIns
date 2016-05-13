using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg.Message.EventMsg
{
    /// <summary>
    /// 扫码推事件的事件推送
    /// </summary>
    public class AcpEventScanCodeMsg : AcpEventMsg
    {
        /// <summary>
        /// 事件KEY值，由开发者在创建菜单时设定
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 扫描信息
        /// </summary>
        public string ScanCodeInfo { get; set; }
        /// <summary>
        /// 扫描类型，一般是qrcode
        /// </summary>
        public string ScanType { get; set; }
        /// <summary>
        /// 扫描结果，即二维码对应的字符串信息
        /// </summary>
        public string ScanResult { get; set; }

        public new AcpEventScanCodeMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.事件消息;
            this.Event = Enum_WXAcpEventMsg_EventType.扫码推事件;
            this.EventKey = root.SelectSingleNode("EventKey").InnerText;
            this.ScanCodeInfo = root.SelectSingleNode("ScanCodeInfo").InnerText;
            this.ScanType = root.SelectSingleNode("ScanType").InnerText;
            this.ScanResult = root.SelectSingleNode("ScanResult").InnerText;

            return this;
        }
    }
}