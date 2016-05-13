using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web;
using Core.Web;

namespace Core.Extensions
{
    /// <summary>
    /// 地理定位等相关操作
    /// </summary>
    public static class PositionExtensions
    {
        private const double PI = 3.14159265;
        private const double EARTH_RADIUS = 6378137;
        private const double RAD = Math.PI / 180.0;

        #region 根据提供的经度和纬度、以及半径，取得此半径内的最大最小经纬度
        /// <summary>
        /// 根据提供的经度和纬度、以及半径，取得此半径内的最大最小经纬度
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lng">经度</param>
        /// <param name="raidus">半径(单位米)</param>
        /// <returns></returns>
        public static double[] getAround(double lat, double lng, int raidus)
        {

            Double latitude = lat;
            Double longitude = lng;

            Double degree = (24901 * 1609) / 360.0;
            double raidusMile = raidus;

            Double dpmLat = 1 / degree;
            Double radiusLat = dpmLat * raidusMile;
            Double minLat = latitude - radiusLat;
            Double maxLat = latitude + radiusLat;

            Double mpdLng = degree * Math.Cos(latitude * (PI / 180));
            Double dpmLng = 1 / mpdLng;
            Double radiusLng = dpmLng * raidusMile;
            Double minLng = longitude - radiusLng;
            Double maxLng = longitude + radiusLng;
            return new double[] { minLat, minLng, maxLat, maxLng };
        }
        #endregion 根据提供的经度和纬度、以及半径，取得此半径内的最大最小经纬度

        #region 根据两点间经纬度坐标（double值），计算两点间距离，单位为米
        /// <summary>
        /// 根据两点间经纬度坐标（double值），计算两点间距离，单位为米
        /// </summary>
        /// <param name="lng1">经度1</param>
        /// <param name="lat1">纬度1</param>
        /// <param name="lng2">经度2</param>
        /// <param name="lat2">纬度2</param>
        /// <returns></returns>
        public static double getDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double radLat1 = lat1 * RAD;
            double radLat2 = lat2 * RAD;
            double a = radLat1 - radLat2;
            double b = (lng1 - lng2) * RAD;
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
        #endregion 根据两点间经纬度坐标（double值），计算两点间距离，单位为米

        #region 根据提供的具体地址获取纬度、经度
        /// <summary>
        /// 根据提供的具体地址获取纬度、经度
        /// </summary>
        /// <param name="address">具体地址</param>
        /// <param name="ak">百度秘钥</param>
        /// <returns></returns>
        public static string[] GetLatitudeAndLongitude(string address, string ak)
        {
            try
            {
                if (address.IsNullOrEmpty() || ak.IsNullOrEmpty())
                    return null;
                string strData = string.Format("address={0}&output=json&ak={1}&callback=showLocation", address, ak);
                string results = HttpHelper.GetReponseText("http://api.map.baidu.com/geocoder/v2/"+strData);

                //System.Text.RegularExpressions.Regex myregex = new Regex("\"lng\":(?<lng>[0-9]+.[0-9]+),\"lat\":(?<lat>[0-9]+.[0-9]+)");
                System.Text.RegularExpressions.Regex myregex = new Regex("\"lng\":([0-9]+.[0-9]+),\"lat\":([0-9]+.[0-9]+)");
                MatchCollection mc = myregex.Matches(results);

                if (mc == null)
                    return null;
                string latitude = string.Empty, longitude = string.Empty;
                foreach (Match mymatch in mc)
                {
                    latitude = mymatch.Groups[1].Value;
                    longitude = mymatch.Groups[2].Value;
                }
                return new string[] { latitude, longitude };
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion 根据提供的具体地址获取纬度、经度

        #region 根据经纬度获取具体的地址
        /// <summary>
        /// 根据经纬度获取具体的地址
        /// </summary>
        /// <param name="latitude">纬度</param>
        /// <param name="lonnitude">经度</param>
        /// <returns></returns>
        public static string GetGeoPosition(string lat, string lng)
        {
            WebClient client = new WebClient(); //webclient客户端对象
            string url = string.Format("http://maps.google.com/maps/api/geocode/xml?latlng={0},{1}&language=zh-CN&sensor=false", lat, lng);  //请求地址
            client.Encoding = Encoding.UTF8;   //编码格式
            string responseTest = client.DownloadString(url);  //下载xml响应数据 
            XmlDocument doc = new XmlDocument();    //创建XML文档对象 
            if (responseTest.IsNullOrEmpty())
                return "";
            doc.LoadXml(responseTest);  //加载xml字符串 
            //获取状态信息
            string xpath = @"GeocodeResponse/status";
            XmlNode node = doc.SelectSingleNode(xpath);
            string status = node.InnerText.ToString();
            if (status == "OK")
            {
                //获取地址信息
                xpath = @"GeocodeResponse/result/formatted_address";
                node = doc.SelectSingleNode(xpath);
                string address = node.InnerText.ToString();
                return address;//输出地址信息
            }
            return "";
        }
        #endregion 根据经纬度获取具体的地址
    }
}
