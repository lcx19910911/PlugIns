using MPUtil.AcceptMsg.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg
{
    /// <summary>
    /// 接收消息的接口
    /// </summary>
    public static class AcceptMsgFunction
    {
        #region XML字符串转化为消息实体
        /// <summary>
        /// XML字符串转化为消息实体
        /// </summary>
        /// <param name="xmlmsg">XML字符串</param>
        /// <returns></returns>
        public static AcceptMessage Convert(string xmlmsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlmsg);
                XmlElement root = xmlDoc.DocumentElement;
                string msgType = root.SelectSingleNode("MsgType").InnerText;

                switch (msgType)
                {
                    case "text":
                        return new AcpTextMsg().Convert(xmlmsg);
                    case "image":
                        return new AcpPicMsg().Convert(xmlmsg);
                    case "voice":
                        return new AcpVoiceMsg().Convert(xmlmsg);
                    case "video":
                        return new AcpVideoMsg().Convert(xmlmsg);
                    case "location":
                        return new AcpLocationMsg().Convert(xmlmsg);
                    case "link":
                        return new AcpLinkMsg().Convert(xmlmsg);
                    case "event":
                        return new AcpEventMsg().Convert(xmlmsg);
                    case "shortvideo":
                        return new AcpShortVideo().Convert(xmlmsg);
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
        #endregion

        #region 将接收的加密消息转为Hashtable
        /// <summary>
        /// 将接收的加密消息转为Hashtable
        /// 返回值：{ 'ToUserName':'toUser','Encrypt':'msg_encrypt' }
        /// </summary>
        /// <param name="encryptMsg"></param>
        /// <returns></returns>
        public static Hashtable ConvertEncryptMsg(string encryptMsg)
        {
            Hashtable hash = new Hashtable();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(encryptMsg);
            XmlElement root = xmlDoc.DocumentElement;
            hash.Add("ToUserName",root.SelectSingleNode("ToUserName").InnerText);
            hash.Add("Encrypt", root.SelectSingleNode("Encrypt").InnerText);
            return hash;
        }
        #endregion 
    }
}