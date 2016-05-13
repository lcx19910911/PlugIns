using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;


namespace MPUtil.AcceptMsg.Message
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class AcpVideoMsg:AcceptMessage
    {
        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string ThumbMediaId { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public string MsgId { get; set; }

        public AcpVideoMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.视频消息;
            this.MsgId = root.SelectSingleNode("MsgId").InnerText;
            this.MediaId = root.SelectSingleNode("MediaId").InnerText;
            this.ThumbMediaId = root.SelectSingleNode("ThumbMediaId").InnerText;
            return this;
        }
    }
}