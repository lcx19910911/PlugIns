using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Core.Extensions
{
    public class EmailExtensions
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to">接收方邮件地址</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件正文内容</param>
        /// <param name="host">STMP服务器地址</param>
        /// <param name="account">SMTP服务帐号</param>
        /// <param name="pwd">SMTP服务密码</param>
        /// <param name="from">发送方邮件地址</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public static bool SendMail(string to, string title, string content, string host, string account, string pwd, string from, int port)
        {

            SmtpClient _smtpClient = new SmtpClient();
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            _smtpClient.Host = host; ;//指定SMTP服务器
            _smtpClient.Port = port;
            _smtpClient.Credentials = new System.Net.NetworkCredential(account, pwd);//用户名和密码
            MailMessage _mailMessage = new MailMessage(from, to);
            _mailMessage.Subject = title;//主题
            _mailMessage.Body = content;//内容
            _mailMessage.BodyEncoding = System.Text.Encoding.UTF8;//正文编码
            _mailMessage.IsBodyHtml = true;//设置为HTML格式
            _mailMessage.Priority = MailPriority.High;//优先级
            try
            {
                _smtpClient.Send(_mailMessage);
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
