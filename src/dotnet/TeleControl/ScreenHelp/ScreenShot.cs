using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace ScreenHelp
{
    /// <summary>
    /// 用于屏幕截图
    /// 并将图像组包
    /// </summary>
    public class ScreenShot
    {
        /// <summary>
        /// 截图
        /// </summary>
        /// <returns></returns>
        static Bitmap ShotScreen()
        {
            Screen scr = Screen.PrimaryScreen;
            Rectangle rc = scr.Bounds;
            var bitmap = new Bitmap(rc.Width, rc.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
            }
            return bitmap;
        }
        /// <summary>
        /// 获得当前屏幕的截图数据
        /// </summary>
        /// <returns></returns>
        public static byte[] getBitmapBites()
        {
            Bitmap bit = ShotScreen();
            Bitmap bit1 = BitmapHelp.KiResizeImage(bit, 1024, 768);
            byte[] bytes = BitmapHelp.BitmaptoBytes(bit1);
            if (bit != null)
                bit.Dispose();
            if (bit1 != null)
                bit1.Dispose();
            return bytes;
        }

    
    }
}
