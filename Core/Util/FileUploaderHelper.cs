using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Core.Extensions;

namespace Core.Util
{
    public class FileUploaderHelper
    {
        string state = "success";   //上传文件状态
        string url = null;      //上传成功后文件路径
        string currenttype = null;  //当前文件后缀名
        string uploadmappath = null;    //上传文件的物理路径
        string filename = null;   //保存名称
        string originalname = null; //原始名称
        byte[] filebytes = null;

        #region 私有成员
        /// <summary>
        /// 获取上传信息
        /// </summary>
        /// <returns></returns>
        private string GetUploadInfo()
        {
            Hashtable hash = new Hashtable();
            hash.Add("state", state);
            hash.Add("url", url);
            hash.Add("filename", filename);
            hash.Add("originalname", originalname);
            return hash.ToJson();
        }
        /// <summary>
        /// 重命名文件
        /// 保留原始名称，若文件名称存在，则生成带(1)的文件
        /// </summary>
        /// <param name="isrename">是否重命名</param>
        /// <returns></returns>
        private string Rename(bool isrename)
        {
            if (isrename)
            {                
                return System.Guid.NewGuid().ToString().Replace("-", "") + "." + GetFileExt();
            }
            else
            {
                if (File.Exists(this.uploadmappath + this.originalname))
                {
                    string[] arr = this.originalname.Split('.');
                    Regex regex = new Regex(@"[(]\d[)]");
                    if (regex.IsMatch(arr[0]))
                    {
                        string num = arr[0].Substring(arr[0].LastIndexOf('(') + 1, arr[0].LastIndexOf(')') - arr[0].LastIndexOf('(') - 1);
                        string numstr = arr[0].Substring(arr[0].LastIndexOf('('), arr[0].LastIndexOf(')') - arr[0].LastIndexOf('(') + 1);
                        this.originalname = this.originalname.Replace(numstr, "(" + (num.GetInt() + 1) + ")");
                    }
                    else
                    {
                        this.originalname = arr[0] + "(1)." + arr[1];
                    }
                }
                return this.originalname;
            }
        }
        /// <summary>
        /// 文件类型检测
        /// </summary>
        /// <param name="filetype">文件后缀数组</param>
        /// <returns></returns>
        private bool CheckType(string[] filetype)
        {
            currenttype = GetFileExt();
            return Array.IndexOf(filetype, currenttype) == -1;
        }
        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <returns></returns>
        private string GetFileExt()
        {
            string[] temp = originalname.Split('.');
            return temp[temp.Length - 1].ToLower();
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        private void CreateFolder()
        {
            if (!Directory.Exists(uploadmappath))
            {
                Directory.CreateDirectory(uploadmappath);
            }
        }
        /// <summary>
        /// 文件大小检测
        /// </summary>
        /// <param name="size">文件大小限制(单位KB)</param>
        /// <returns></returns>
        private bool CheckSize(int size)
        {
            return filebytes.Length >= (size * 1024);
        }
        #endregion 私有成员

        /// <summary>
        /// 上传文件的主处理方法
        /// 返回JSON数据：{"state":"success","url":"上传路径","filename":"上传文件名","originalname":"原始文件名"}
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="originalName">原始名称(含后缀名)</param>
        /// <param name="pathbase">保存路径,(格式'/../')</param>
        /// <param name="filetype">允许的文件后缀</param>
        /// <param name="size">文件大小(单位KB)</param>
        /// <param name="isrename">是否重命名</param>
        /// <returns></returns>
        public string UpFile(byte[] fileBytes, string originalName, string pathbase, string[] filetype, int size, bool isrename)
        {
            this.uploadmappath = System.Web.HttpContext.Current.Server.MapPath(pathbase);  //获取文件上传路径
            this.originalname = originalName;
            this.filebytes = fileBytes;
            try
            {
                //目录创建
                CreateFolder();
                //格式验证
                if (CheckType(filetype))
                    state = "仅支持“" + string.Join(",", filetype) + "”格式的文件";
                //大小验证
                if (CheckSize(size))
                {
                    if (((decimal)size) / 1024 >= 1)
                    {
                        state = "文件大小不能超过 " + ((decimal)size / 1024).ToString() + " MB";
                    }
                    else
                    {
                        state = "文件大小不能超过 " + size.ToString() + " KB";
                    }

                }
                //保存图片
                if (state == "success")
                {
                    filename = Rename(isrename);
                    FileStream fs = new FileStream(uploadmappath + filename, FileMode.Create);
                    fs.Write(fileBytes, 0, fileBytes.Length);
                    fs.Flush();
                    fs.Close();
                    url = pathbase + filename;
                }
            }
            catch (Exception e)
            {
                state = "未知错误";
                url = "";
            }
            return GetUploadInfo();
        }
    }
}
