using MPUtil.ServiceMsg.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPUtil.ServiceMsg
{
    /// <summary>
    /// 客服消息类型
    /// </summary>
    public enum Enum_WXServiceMsg_Type
    {
        文本消息=1,
        图片消息=2,
        语音消息=3,
        视频消息=4,
        音乐消息=5,
        图文消息=6
    }
    /// <summary>
    /// 客服消息
    /// </summary>
    public class ServiceMessage
    {
        /// <summary>
        /// 普通用户openid
        /// </summary>
        public string ToUser { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public Enum_WXServiceMsg_Type MsgType { get; set; }

        /// <summary>
        /// 将消息实体转为化微信JSON数据包
        /// </summary>
        /// <returns></returns>
        public string Reverse()
        {
            switch (this.MsgType)
            {
                case Enum_WXServiceMsg_Type.文本消息:
                    return (this as ServiceTextMsg).Reverse();
                case Enum_WXServiceMsg_Type.图片消息:
                    return (this as ServicePicMsg).Reverse();
                case Enum_WXServiceMsg_Type.语音消息:
                    return (this as ServiceVoiceMsg).Reverse();
                case Enum_WXServiceMsg_Type.视频消息:
                    return (this as ServiceVideoMsg).Reverse();
                case Enum_WXServiceMsg_Type.音乐消息:
                    return (this as ServiceMusicMsg).Reverse();
                case Enum_WXServiceMsg_Type.图文消息:
                    return (this as ServiceNewsMsg).Reverse();
                default:
                    return string.Empty;
            }
        }
    }
}