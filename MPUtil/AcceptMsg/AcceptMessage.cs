using MPUtil.AcceptMsg.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace MPUtil.AcceptMsg
{
    #region 枚举-消息类型
    /// <summary>
    /// 公众平台消息类型
    /// </summary>
    public enum Enum_WXAccepMsg_Type
    {
        文本消息=1,
        图片消息=2,
        语音消息=3,
        视频消息=4,
        地理位置消息=5,
        链接消息=6,
        事件消息=7,
        小视频消息=8
    }
    #endregion

    /// <summary>
    /// 公众平台消息
    /// </summary>
    public class AcceptMessage
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public Enum_WXAccepMsg_Type MsgType { get; set; }
    }
}