using System;
using System.Configuration;
using System.Collections.Specialized;

namespace Core.Util
{
    /// <summary>
    /// ��ȡAppSettings�е�����
    /// </summary>
    public class ConfigHelper
    { 
        #region Ӧ�ó������ýڵ㼯������
        /// <summary>
        /// Ӧ�ó������ýڵ㼯������
        /// </summary>
        /// <returns></returns>
        public static NameValueCollection SettingsCollection
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }
        #endregion

        #region ��ȡappSettings�ڵ�ֵ
        /// <summary>
        /// ��ȡappSettings�ڵ�ֵ
        /// </summary>
        /// <param name="key">�ڵ�����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>�ڵ�ֵ</returns>
        public static string GetSetting(string key, string defaultValue)
        {
            try
            {
                if (SettingsCollection == null)
                    return defaultValue;
                return SettingsCollection[key] ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// ��ȡappSettings�ڵ�ֵ
        /// </summary>
        /// <param name="key">�ڵ�����</param>
        /// <returns>�ڵ�ֵ</returns>
        public static string GetSetting(string key)
        {
            return GetSetting(key, "");
        }

        /// <summary>
        /// ��ȡappSettings�ڵ�ֵ����ת��Ϊboolֵ
        /// </summary>
        /// <param name="key">�ڵ�����</param>
        /// <returns></returns>
        public static bool GetBoolean(string key)
        {
            string tmp = GetSetting(key);
            return tmp.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// ��ȡappSettings�ڵ�ֵ����ת��Ϊintֵ
        /// </summary>
        /// <param name="key">�ڵ�����</param>
        /// <param name="defaultValue">�ڵ㲻���ڻ�����ֵʱ��Ĭ��ֵ</param>
        /// <returns></returns>
        public static Int32 GetInt32(string key, Int32 defaultValue = 0)
        {
            string tmp = GetSetting(key);
            if (string.IsNullOrEmpty(tmp))
                return defaultValue;
            Int32 ret;
            if (Int32.TryParse(tmp, out ret))
                return ret;
            else
                return defaultValue;
        }

        /// <summary>
        /// ��ȡappSettings�ڵ�ֵ����ת��Ϊlongֵ
        /// </summary>
        /// <param name="key">�ڵ�����</param>
        /// <param name="defaultValue">�ڵ㲻���ڻ�����ֵʱ��Ĭ��ֵ</param>
        /// <returns></returns>
        public static Int64 GetInt64(string key, Int64 defaultValue = 0)
        {
            string tmp = GetSetting(key);
            if (string.IsNullOrEmpty(tmp))
                return defaultValue;
            Int64 ret;
            if (Int64.TryParse(tmp, out ret))
                return ret;
            else
                return defaultValue;
        }

        /// <summary>
        /// ��ȡappSettings�ڵ�ֵ����ת��ΪDateTimeֵ
        /// </summary>
        /// <param name="key">�ڵ�����</param>
        /// <param name="defaultValue">�ڵ㲻���ڻ���ʱ��ʱ��Ĭ��ֵ</param>
        /// <returns></returns>
        public static DateTime GetDateTime(string key, DateTime? defaultValue = null)
        {
            string tmp = GetSetting(key);
            if (!string.IsNullOrEmpty(tmp))
            {
                DateTime ret;
                if (DateTime.TryParse(tmp, out ret))
                    return ret;
            }
            if (defaultValue == null)
                return DateTime.MinValue;
            else
                return defaultValue.Value;
        }

        /// <summary>
        /// ��ȡ�����˵�appSettings�ڵ�ֵ
        /// </summary>
        /// <param name="key">�ڵ�����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>�ڵ�ֵ</returns>
        public static string GetEncSetting(string key, string defaultValue = null)
        {
            string ret = GetSetting(key, defaultValue);
            if (string.IsNullOrEmpty(ret) || ret == defaultValue)
                return ret;
            return CryptoHelper.DES_Decrypt(ret);
            //return GetConnectionString(key, Key);
        }
        #endregion

        #region ��ȡָ�����Ƶ������ַ���
        /// <summary>
        /// ��ȡָ�����Ƶ������ַ���
        /// </summary>
        /// <param name="connName">���Ӵ��ڵ�����</param>
        /// <param name="mKey">����Key</param>
        /// <returns>���ܺ�����Ӵ�</returns>
        public static string GetConnectionString(string connName, string mKey = null)
        {
            try
            {
                string conn = GetSetting(connName);
                if (mKey != null)
                {
                    conn = CryptoHelper.DES_Decrypt(conn, mKey);
                }
                else
                {
                    conn = CryptoHelper.DES_Decrypt(conn);
                }

                return conn;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
