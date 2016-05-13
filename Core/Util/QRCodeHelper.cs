using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using ZXing;
using ZXing.QrCode.Internal;
using ZXing.Rendering;
using Core.Extensions;


namespace Core.Util
{
    #region 枚举
    /// <summary>
    /// 指定缩放类型
    /// </summary>
    public enum ImageThumbnailType
    {
        /// <summary>
        /// 无
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 指定高宽缩放（可能变形）
        /// </summary>
        WH = 1,
        /// <summary>
        /// 指定宽，高按比例
        /// </summary>
        W = 2,
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        H = 3,
        /// <summary>
        /// 指定高宽裁减（不变形）
        /// </summary>
        Cut = 4,
        /// <summary>
        /// 按照宽度成比例缩放后，按照指定的高度进行裁剪
        /// </summary>
        W_HCut = 5,
    }
    #endregion
    /// <summary>
    /// 二维码帮助类
    /// </summary>
    public class QRCodeHelper
    { 
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="saveUrl">二维码存放路径(可为空)</param>
        /// <param name="data">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="logo">logo(可为空)</param>
        /// <param name="logoWidth">logo(宽度)</param>
        /// <param name="logoHeight">logo(高度)</param>
        /// <param name="border">logo边框(可为空)</param>
        /// <returns></returns>
        public static Bitmap Create(string saveUrl, string data, int width, int height, string logo, int logoWidth,int logoHeight,string border)
        {
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Renderer = new BitmapRenderer
                {
                    Foreground = Color.Black
                },
                Options = new ZXing.QrCode.QrCodeEncodingOptions
                {
                    DisableECI = true,
                    Height = height,
                    Width = width,
                    Margin = 0,
                    CharacterSet = "UTF-8",
                    ErrorCorrection = ErrorCorrectionLevel.M
                }
            };

            Bitmap bitmap = writer.Write(data);
            if (logo.IsNotNullOrEmpty())
            {
                Bitmap bits = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(logo);
                if (bits != null)
                {
                    //生成一个缩略logo图片
                    int iWidth = bits.Width;
                    int iHeight = bits.Height;
                    //如果宽和高都超过最大限制
                    System.Drawing.Bitmap icon = Thumbnail(logo, logoWidth, logoHeight, 100, iWidth > iHeight ?ImageThumbnailType.W : ImageThumbnailType.H);
                    //边框图片
                    //Bitmap bitsBorder = new System.Drawing.Bitmap((System.Drawing.Bitmap)System.Drawing.Image.FromFile(border), 100, 100);
                    if (icon != null)
                    {
                        try
                        {
                            //画了2个边框，一个是logo,一个在logo周围加了一个边框
                            using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
                            {
                                //graphics.DrawImage(bitsBorder, (bitmap.Width - bitsBorder.Width) / 2, (bitmap.Height - bitsBorder.Height) / 2);
                                graphics.DrawImage(icon, (bitmap.Width - icon.Width) / 2, (bitmap.Height - icon.Height) / 2);
                            }
                        }
                        catch (Exception)
                        {

                        }
                        finally
                        {
                            icon.Dispose();
                            GC.Collect();
                        }
                    }
                    if (saveUrl.IsNotNullOrEmpty())
                        bitmap.Save(saveUrl, ImageFormat.Jpeg);
                }
            }
            return bitmap;
        }

        

        #region Thumbnail


        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sourceFile">原图片</param>
        /// <param name="stream">压缩后保存到流中</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        /// <param name="quality">压缩质量 1-100</param>
        /// <param name="type">压缩缩放类型</param>
        /// <returns></returns>
        public static Bitmap Thumbnail(string sourceFile, int width, int height, int quality, ImageThumbnailType type)
        {
            using (System.Drawing.Image iSource = System.Drawing.Image.FromFile(sourceFile))
            {
                //缩放后的宽度和高度
                int toWidth = width;
                int toHeight = height;
                //
                int x = 0;
                int y = 0;
                int oWidth = iSource.Width;
                int oHeight = iSource.Height;

                switch (type)
                {
                    case ImageThumbnailType.WH://指定高宽缩放（可能变形）            
                        {
                            break;
                        }
                    case ImageThumbnailType.W://指定宽，高按比例      
                        {
                            toHeight = iSource.Height * width / iSource.Width;
                            break;
                        }
                    case ImageThumbnailType.H://指定高，宽按比例
                        {
                            toWidth = iSource.Width * height / iSource.Height;
                            break;
                        }
                    case ImageThumbnailType.Cut://指定高宽裁减（不变形）      
                        {
                            if ((double)iSource.Width / (double)iSource.Height > (double)toWidth / (double)toHeight)
                            {
                                oHeight = iSource.Height;
                                oWidth = iSource.Height * toWidth / toHeight;
                                y = 0;
                                x = (iSource.Width - oWidth) / 2;
                            }
                            else
                            {
                                oWidth = iSource.Width;
                                oHeight = iSource.Width * height / toWidth;
                                x = 0;
                                y = (iSource.Height - oHeight) / 2;
                            }
                            break;
                        }
                    case ImageThumbnailType.W_HCut://按照宽度成比例缩放后，按照指定的高度进行裁剪
                        {
                            toHeight = iSource.Height * width / iSource.Width;
                            if (height < toHeight)
                            {
                                oHeight = oHeight * height / toHeight;
                                toHeight = toHeight * height / toHeight;
                            }
                            break;
                        }
                    default:
                        break;
                }

                Bitmap ob = new Bitmap(toWidth, toHeight);
                Graphics g = Graphics.FromImage(ob);
                g.Clear(System.Drawing.Color.WhiteSmoke);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(iSource, new Rectangle(x, y, toWidth, toHeight), new Rectangle(0, 0, oWidth, oHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return ob;
            }
        }
        #endregion
    }
}
