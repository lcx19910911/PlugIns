using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using Core.Util;
using Core.Extensions;
using System.Collections.Specialized;

namespace Core.Web
{
    /// <summary>
    /// Http请求的辅助方法
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 获取指定网址的响应内容，默认以utf-8格式返回
        /// </summary>
        /// <param name="sUrl">指定网址</param>
        /// <returns></returns>
        public static string GetReponseText(string sUrl)
        {
            return GetReponseText(sUrl, Encoding.UTF8, "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648; .NET CLR 1.1.4322; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)");
        }
        /// <summary>
        ///  获取指定网址的响应内容，指定编码
        /// </summary>
        /// <param name="sUrl">指定网址</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        public static string GetReponseText(string sUrl, Encoding encode)
        {
            return GetReponseText(sUrl, encode, "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648; .NET CLR 1.1.4322; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)");
        }
        /// <summary>
        /// 获取指定网址的响应内容
        /// </summary>
        /// <param name="sUrl">指定网址</param>
        /// <param name="encode">编码方式</param>
        /// <param name="sAgent">浏览器类型</param>
        /// <returns></returns>
        public static string GetReponseText(string sUrl, Encoding encode, string sAgent)
        {
            return GetReponseText(sUrl, encode, sAgent, 0);
        }

        /// <summary>
        /// 获取指定网址的响应内容
        /// </summary>
        /// <param name="sUrl">指定网址</param>
        /// <param name="encode">编码方式</param>
        /// <param name="sAgent">浏览器类型</param>
        /// <param name="timeOut">过期时间，以毫秒为单位</param>
        /// <returns></returns>
        public static string GetReponseText(string sUrl, Encoding encode, string sAgent, int timeOut)
        {
            HttpWebRequest request = HttpWebRequest.Create(sUrl) as HttpWebRequest;
            //request.AllowAutoRedirect = false;
            request.Referer = sUrl;
            request.UserAgent = sAgent;
            if (timeOut > 0)
            {
                request.Timeout = timeOut;
            }
            string result;
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream, encode))
                {
                    result = reader.ReadToEnd();
                }
            }
            response.Close();
            return result;
        }

        /// <summary>
        /// 判断internet文件是否存在
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public static bool RemoteFileExists(string url, out long fileSize)
        {
            bool ret = false;
            fileSize = 0;
            WebResponse response = null;
            try
            {
                WebRequest req = WebRequest.Create(url);
                req.Method = "head";
                response = req.GetResponse();
                ret = ((HttpWebResponse)response).StatusCode == HttpStatusCode.OK;
                if (ret)
                {
                    fileSize = response.ContentLength;
                }
            }
            catch (Exception)
            {
                ret = false;
            }
            finally
            {
                if (null != response)
                {
                    response.Close();
                }
            }

            return ret;
        }

        private static HttpWebRequest GetRequest(string url, bool usePost = false)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.Method = usePost ? "POST" : "GET";
            request.KeepAlive = true;
            request.CookieContainer = new CookieContainer();
            //if (useProxy)
            //{
            //    var proxy = ProxyHelper.GetProxyUri();
            //    if (!proxy.IsNullOrEmpty())
            //    {
            //        request.Proxy = new WebProxy(proxy);
            //    }
            //}
            return request;
        }

        private static string GetResponseText(HttpWebResponse response)
        {
            var encoding = response.CharacterSet.IsNullOrEmpty() ? Encoding.UTF8 : Encoding.GetEncoding(response.CharacterSet);
            var stream = response.GetResponseStream();
            if (response.ContentEncoding.ToLower().Contains("gzip"))
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }
            else if (response.ContentEncoding.ToLower().Contains("deflate"))
            {
                stream = new DeflateStream(stream, CompressionMode.Decompress);
            }
            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static bool TryGetHtml(string url, out string html)
        {
            var proxy = string.Empty;
            try
            {
                var request = GetRequest(url);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    html = GetResponseText(response);
                }
                return true;
            }
            catch (WebException ex)
            {
                LogHelper.WriteException("TryGetHtml. Url:[" + url + "]", ex);
                var response = ex.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                {
                    html = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("TryGetHtml. Url:[" + url + "]", ex);
            }
            html = null;
            return false;
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="args"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static bool TryPostHtml(string url, Dictionary<string, string> args, out string html)
        {
            var proxy = string.Empty;
            try
            {
                var request = GetRequest(url, true);
                var ps = GetRequestParams(args);
                if (!ps.IsNullOrEmpty())
                {
                    var arr = Encoding.UTF8.GetBytes(ps);
                    request.ContentLength = arr.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(arr, 0, arr.Length);
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    html = GetResponseText(response);
                }
                return true;
            }
            catch (WebException ex)
            {
                LogHelper.WriteException("TryGetHtml. Url:[" + url + "]", ex);
                var response = ex.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                {
                    html = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("TryGetHtml. Url:[" + url + "]", ex);
            }
            html = null;
            return false;
        }

        private static string GetRequestParams(Dictionary<string, string> args)
        {
            var sb = new StringBuilder();
            foreach (var arg in args)
            {
                sb.AppendFormat("{0}={1}&", arg.Key, arg.Value);
            }
            return sb.ToString().TrimEnd('&');
        }
        /// <summary>
        /// 执行post请求
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="data">发送的数据</param>
        /// <returns></returns>
        public static string ServerPostRequest(string url, string data)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = System.Text.Encoding.UTF8;
            byte[] databyte = encoding.GetBytes(data);
            // 设置参数
            request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = databyte.Length;
            outstream = request.GetRequestStream();
            outstream.Write(databyte, 0, databyte.Length);
            outstream.Close();
            //发送请求并获取相应回应数据
            response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            instream = response.GetResponseStream();
            sr = new StreamReader(instream, encoding);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            string err = string.Empty;
            return content;
        }

        /// <summary>
        /// 文件表单提交
        /// </summary>
        /// <param name="url">Post路径</param>
        /// <param name="filePath">文件</param>
        /// <param name="fileKeyName">文件表单Key</param>
        /// <param name="stringDict">表单内容键值集合</param>
        /// <returns></returns>
        public static string FilePostRequest(string url, string filePath, string fileKeyName, NameValueCollection stringDict)
        {
            string responseContent;
            MemoryStream memStream = new MemoryStream();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // 边界符
            string boundary = "-----------------------------" + DateTime.Now.Ticks.ToString("x");
            // 设置属性
            webRequest.Method = "POST";
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.CookieContainer = new CookieContainer();

            //  开始
            StringBuilder poststr_begin = new StringBuilder();
            if (stringDict != null && stringDict.Count > 0)
            {
                foreach (var item in stringDict.AllKeys)
                {
                    poststr_begin.Append(boundary + "\r\n");
                    poststr_begin.AppendFormat("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n", item, stringDict[item]);
                }
            }
            poststr_begin.Append(boundary + "\r\n");
            poststr_begin.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", fileKeyName, filePath);
            //  结束
            string poststr_end = "\r\n" + boundary + "-----------------------------\r\n";

            //  写入开始内容
            byte[] beginbytes = Encoding.UTF8.GetBytes(poststr_begin.ToString());
            memStream.Write(beginbytes, 0, beginbytes.Length);
            //  写入文件
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }
            //  写入结束内容
            byte[] endbytys = Encoding.ASCII.GetBytes(poststr_end);
            memStream.Write(endbytys, 0, endbytys.Length);

            webRequest.ContentLength = memStream.Length;
            memStream.Position = 0;
            byte[] tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            using (StreamReader httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }
            fileStream.Close();
            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }

    }
}
