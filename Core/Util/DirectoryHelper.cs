using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utility
{
    public static class DirectoryHelper
    {
        #region 根据字节流生成文件
        /// <summary>
        /// 根据字节流生成文件 
        /// </summary>
        /// <param name="filePath">文件保存路径</param>
        /// <param name="fileContent">文件内容</param>
        /// <param name="fileEncoding">编码</param>
        /// <returns></returns>
        public static bool CreateFile(string filePath, byte[] fileContent, System.Text.Encoding fileEncoding)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                BinaryWriter br = new BinaryWriter(fs, fileEncoding);
                br.Write(fileContent);
                br.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 初始化目录
        // <summary>
        /// 初始化目录
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static bool CreateDirectory(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                return false;
            }
            if (Directory.Exists(directory))
            {
                return true;
            }
            var dirInfo = Directory.CreateDirectory(directory);
            return dirInfo != null;
        }
        #endregion 

        #region 获取目录名称
        /// <summary>
        /// 获取目录名称
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return string.Empty;// DirectoryHelper.CreateDirectory(directory);
            }
            return new DirectoryInfo(directory).Name;
        }
        #endregion 

        #region 获取目录文件夹下的所有子目录
        /// <summary>
        /// 获取目录文件夹下的所有子目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filePattern"></param>
        /// <returns></returns>
        public static List<string> FindSubDirectories(string directory, int maxCount)
        {
            List<string> subDirectories = new List<string>();
            if (string.IsNullOrEmpty(directory))
            {
                return subDirectories;
            }
            if (maxCount <= 0)
            {
                return subDirectories;
            }
            string[] directories = Directory.GetDirectories(directory);
            foreach (string subDirectory in directories)
            {
                if (subDirectories.Count == maxCount)
                {
                    break;
                }
                subDirectories.Add(subDirectory);
            }
            return subDirectories;
        }
        public static List<string> FindSubDirectories(string directory)
        {
            return Directory.GetDirectories(directory, "*", SearchOption.AllDirectories).ToList<string>();
        }
        #endregion

        #region 根据时间查询子目录
        /// <summary>
        /// 根据时间查询子目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static List<string> FindSubDirectories(string directory, int maxCount, int days)
        {
            List<string> subDirectories = new List<string>();
            if (string.IsNullOrEmpty(directory))
            {
                return subDirectories;
            }
            if (maxCount <= 0)
            {
                return subDirectories;
            }
            string[] directories = Directory.GetDirectories(directory);
            DateTime lastTime = DateTime.Now.AddDays(-Math.Abs(days));
            foreach (string subDirectory in directories)
            {
                if (subDirectories.Count == maxCount)
                {
                    break;
                }
                DirectoryInfo dirInfo = new DirectoryInfo(subDirectory);
                if (dirInfo.LastWriteTime >= lastTime)
                {
                    subDirectories.Add(subDirectory);
                }
            }
            return subDirectories;
        }
        #endregion 

        #region 获取指定目录下的所有文件和文件夹大小
        /// <summary>
        /// 获取指定目录下的所有文件和文件夹大小
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>string，返回所有文件夹名字</returns>
        public static long GetDirectorySize(string path)
        {
            long dirSize = 0;
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles())
                dirSize += file.Length;
            foreach (DirectoryInfo subdir in dir.GetDirectories())
                dirSize += GetDirectorySize(subdir.FullName);
            return dirSize;
        }
        #endregion 
    }
}
