using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg.Message.EventMsg
{
    /// <summary>
    /// 弹出拍照或者相册发图的事件推送
    /// </summary>
    public class AcpEventPicPhotoAlbumMsg:AcpEventMsg
    {
        /// <summary>
        /// 事件KEY值，由开发者在创建菜单时设定
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 发送的图片数量
        /// </summary>
        public string Count { get; set; }
        /// <summary>
        /// 图片的MD5值，开发者若需要，可用于验证接收到图片
        /// </summary>
        public List<string> PicMd5Sum { get; set; }

        public new AcpEventPicPhotoAlbumMsg Convert(string xmlmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlmsg);
            XmlElement root = xmlDoc.DocumentElement;
            this.ToUserName = root.SelectSingleNode("ToUserName").InnerText;
            this.FromUserName = root.SelectSingleNode("FromUserName").InnerText;
            this.CreateTime = root.SelectSingleNode("CreateTime").InnerText;
            this.MsgType = Enum_WXAccepMsg_Type.事件消息;
            this.Event = Enum_WXAcpEventMsg_EventType.弹出拍照或者相册发图的事件推送;
            this.EventKey = root.SelectSingleNode("EventKey").InnerText;

            XmlNode picInfoNode = root.SelectSingleNode("SendPicsInfo");
            this.Count = picInfoNode.SelectSingleNode("Count").InnerText;

            XmlNodeList itemNodeList = picInfoNode.SelectSingleNode("PicList").SelectNodes("item");
            this.PicMd5Sum = new List<string>();
            foreach (XmlNode item in itemNodeList)
            {
                this.PicMd5Sum.Add(item.SelectSingleNode("PicMd5Sum").InnerText);
            }
            return this;
        }
    }
}