using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;


namespace MPUtil.AcceptMsg.Message
{
    /// <summary>
    /// 语音消息
    /// </summary>
    public class AcpVoiceMsg : AcceptMessage
    {
        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 语音识别结果，UTF8编码
        /// </summary>
        public string Recognition { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }

        public AcpVoiceMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.语音消息;
            this.MsgId = root.SelectSingleNode("MsgId").InnerText;
            this.MediaId = root.SelectSingleNode("MediaId").InnerText;
            this.Format = root.SelectSingleNode("Format").InnerText;
            if (root.SelectSingleNode("ToUserName") != null)
                this.Recognition = root.SelectSingleNode("ToUserName").InnerText;
            return this;
        }
    }
}