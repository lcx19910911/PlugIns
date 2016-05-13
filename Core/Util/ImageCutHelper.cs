
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace Utility
{
    /// <summary>
    /// 图片裁剪帮助类
    /// </summary>
    public class ImageCutHelper
    {
        public int X = 0;
        public int Y = 0;
        public int Width = 120;
        public int Height = 120;
        public ImageCutHelper(int x, int y, int width, int heigth)
        {
            X = x;
            Y = y;
            Width = width;
            Height = heigth;
        }
        /// <summary>
        /// 剪裁 -- 用GDI+
        /// </summary>
        /// <param name="b">原始Bitmap</param>
        /// <param name="StartX">开始坐标X</param>
        /// <param name="StartY">开始坐标Y</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns>剪裁后的Bitmap</returns>
        public Bitmap KiCut(Bitmap b)
        {
            try
            {

                if (b == null)
                    return null;

                int w = b.Width;
                int h = b.Height;
                int intWidth = 0;
                int intHeight = 0;
                if (h * Width / w > Height)
                {
                    intWidth = Width;
                    intHeight = h * Width / w;

                }
                else if (h * Width / w < Height)
                {
                    intWidth = w * Height / h;
                    intHeight = Height;

                }
                else
                {
                    intWidth = Width;
                    intHeight = Height;
                }

                Bitmap bmpOut_b = new System.Drawing.Bitmap(b, intWidth, intHeight);
                w = bmpOut_b.Width;
                h = bmpOut_b.Height;


                if (X >= w || Y >= h)
                    return null;

                if (X + Width > w)
                    Width = w - X;
                else
                    X = (w - Width) / 2;

                if (Y + Height > h)
                    Height = h - Y;

                Bitmap bmpOut = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(bmpOut_b, new Rectangle(0, 0, Width, Height), new Rectangle(X, Y, Width, Height), GraphicsUnit.Pixel);
                g.Dispose();

                return bmpOut;
            }
            catch
            {
                return null;
            }
        }
    }    
}
