using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PPLibrary = Microsoft.Office.Interop.PowerPoint;

namespace Instructor___Omlate.ContentLoad
{
    public class PPTFileLoader
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SetWindowText(IntPtr hwnd, String lpString);

        private PPLibrary.Application application;
        public PPLibrary.Presentation Presentation { get; set; }

        private Process dispalyChanger;

        public PPTFileLoader()
        {

        }

        public void LoadPPTSlides(string fileName, System.Windows.Forms.Panel slideShowPanel)
        {
            IntPtr screenClasshWnd = (IntPtr)0;
            IntPtr x = (IntPtr)0;
            application = new Microsoft.Office.Interop.PowerPoint.Application();

            Presentation = application.Presentations.Open(fileName, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
            Microsoft.Office.Interop.PowerPoint.SlideShowSettings sst1 = Presentation.SlideShowSettings;

            sst1.ShowType = (Microsoft.Office.Interop.PowerPoint.PpSlideShowType)1;
            Microsoft.Office.Interop.PowerPoint.SlideShowWindow sw = sst1.Run();

            sw.Height = (slideShowPanel.Height) - 150;
            sw.Width = (slideShowPanel.Width) - 340;

            IntPtr formhWnd = FindWindow(x, "SlideShow");
            IntPtr pptptr = (IntPtr)sw.HWND;
            screenClasshWnd = FindWindow(x, "screenClass");
            SetParent(pptptr, slideShowPanel.Handle);

            this.application.SlideShowEnd += new Microsoft.Office.Interop.PowerPoint.EApplication_SlideShowEndEventHandler(SlideShowEnds);
        }

        private void SlideShowEnds(Microsoft.Office.Interop.PowerPoint.Presentation Pres)
        {
            Presentation.Close();
        }

        public void ChangeToProjecterView()
        {
            dispalyChanger = new Process();
            dispalyChanger.StartInfo.FileName = "DisplaySwitch.exe";
            dispalyChanger.StartInfo.Arguments = "/clone";
            dispalyChanger.Start();
        }
        public void ChangeDisplayToNormal()
        {
            dispalyChanger = new Process();
            dispalyChanger.StartInfo.FileName = "DisplaySwitch.exe";
            dispalyChanger.StartInfo.Arguments = "/internal";
            dispalyChanger.Start();
        }
    }
}
