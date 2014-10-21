using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace ScreenHelp
{
    public class BitmapHelp
    {
        public static byte[] BitmaptoBytes(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] bytes = ms.GetBuffer();  //byte[]   bytes=   ms.ToArray(); 这两句都可以，至于区别么，下面有解释
            ms.Close();
            return bytes;
        }

        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            using (MemoryStream ms1 = new MemoryStream(bytes))
            {
                Bitmap bm = (Bitmap)Image.FromStream(ms1);
                

                BitmapData data = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                Bitmap bitmap2 = new Bitmap(bm.Width, bm.Height, data.Stride, PixelFormat.Format16bppArgb1555, data.Scan0);
                bm.Dispose();
                ms1.Dispose();
                ms1.Close();
                data = null;

                return bitmap2;
            }
        }

        ///
        /// Resize图片
        ///
        /// 原始Bitmap
        /// 新的宽度
        /// 新的高度
        /// 保留着，暂时未用
        /// 处理以后的图片
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        public static System.Windows.Media.Imaging.BitmapImage BytestoBitmapImage(byte[] bytes)
        {
            var bib = new System.Windows.Media.Imaging.BitmapImage();
            bib.BeginInit();
            bib.StreamSource = new System.IO.MemoryStream(bytes);
            bib.EndInit();
            return bib;
        }

        /// <summary>
        /// 颜色Drawing转Me
        /// </summary>
        /// <param name="mebrush"></param>
        /// <returns></returns>
        public static System.Windows.Media.Brush DrawingbrtoMediabr(System.Drawing.Brush mebrush)
        {
            System.Drawing.Color mecolor = ((System.Drawing.SolidBrush)mebrush).Color;
            System.Windows.Media.Brush br = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(mecolor.A, mecolor.R, mecolor.G, mecolor.B));
            return br;

        }
        /// <summary>
        /// 颜色media转Drawing
        /// </summary>
        /// <param name="mebrush"></param>
        /// <returns></returns>
        public static System.Drawing.Brush mebrushtodrawbrush(System.Windows.Media.Brush mebrush)
        {
            System.Windows.Media.Color mecolor = ((System.Windows.Media.SolidColorBrush)mebrush).Color;
            System.Drawing.Brush br = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(mecolor.A, mecolor.R, mecolor.G, mecolor.B));
            return br;

        }


        /// <summary>
        /// bitmap转image 此方法好像会导致内存泄露
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {

            System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            try
            {
                using (System.IO.Stream ms = new System.IO.MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
            }
            catch (Exception ex)
            {

            }

            return bitmapImage;
        }
    }
}
