using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandoutsGeneration
{
    class ScreenCapture
    {
        Bitmap bitmap;
        Graphics graphics;

        /// <summary>
        /// Screen Capture Constructor
        /// </summary>
        public ScreenCapture() { 
            bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                               Screen.PrimaryScreen.Bounds.Height,
                               PixelFormat.Format32bppArgb);
            graphics = Graphics.FromImage(bitmap);
        }
        
        /// <summary>
        /// Takes the Screen Shot
        /// </summary>
        /// <returns>A Bitmap of Screen Shot</returns>
        public Bitmap TakeScreenShot()
        {
            graphics.CopyFromScreen(GetScreenCordX(), GetScreenCordY(),
                                    0, 0, Screen.PrimaryScreen.Bounds.Size,
                                    CopyPixelOperation.SourceCopy);
            return bitmap;
        }
        /// <summary>
        /// Get the Screen Bound X
        /// </summary>
        /// <returns>Returns X Cordinate</returns>
        private int GetScreenCordX()
        {
            return Screen.PrimaryScreen.Bounds.X;
        }
        /// <summary>
        /// Get the Screen Bound Y
        /// </summary>
        /// <returns>Returns Y Cordinate</returns>
        private int GetScreenCordY()
        {
            return Screen.PrimaryScreen.Bounds.Y;
        }
    }
}
