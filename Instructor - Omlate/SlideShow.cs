using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using AForge.Video.DirectShow;
using AForge.Video;
using Instructor___Omlate.ContentLoad;
using HandoutsGeneration;
using Instructor___Omlate.Stream;

namespace Instructor___Omlate
{
    public partial class SlideShow : Form
    {
        private DateTime startTime;
        private DispatcherTimer timer;
        int slides;

        Device device;
        StreamClass stream;
        private List<int> frameRates;
        private List<string> videoSizes;

        private HandoutGenerator hoGen;
        bool streamStarted, desktopStream;

        OpenFileDialog openFileDiag;
        string lectureFileName;

        private PPTFileLoader pptLoader;

        //bool flag = false;

        public SlideShow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            device = new Device();
            stream = new StreamClass();
            pptLoader = new PPTFileLoader();
            openFileDiag = new OpenFileDialog();
            timer.Interval = new TimeSpan(0, 0, 1);
            
            openFileDiag.Filter = "Powerpoint file (*.ppt)|*.ppt|All files (*.*)|*.*";
            stream.StreamName = "class1";
            desktopStream = false;
        }

        private void SlideShow_Load(object sender, EventArgs e)
        {
            //pptLoader.ChangeToProjecterView();
            if (device.WebCams.Count == 0)
            {
                MessageBox.Show("No Video Device Deteced");
            }
            else
            {
                frameRates = new List<int>();
                videoSizes = new List<string>();
                VideoCapabilities[] caps = device.GetDeviceCapablities(device.WebCams[0]);
                for (int i = 0; i < caps.Length; i++)
                {
                    int fr = caps[i].FrameRate;
                    string vs = caps[i].FrameSize.Width + "x" + caps[i].FrameSize.Height;
                    videoSizes.Add(vs);
                    frameRates.Add(fr);
                }
            }
            if (device.Mics.Count == 0)
            {
                MessageBox.Show("No Audio Device Detected");
            }
        }


        private void streamCamButton_Click(object sender, EventArgs e)
        {
            if (!streamStarted)
            {
                desktopStream = false;
                //startdesktop.IsEnabled = false;
                //next.IsEnabled = false;
                if (device.WebCams.Count > 0)
                {
                    stream.StreamName = "class1";
                    stream.AudioDevice = device.Mics[0];
                    stream.VideoDevice = device.WebCams[0];
                    stream.FrameRate = frameRates[frameRates.Count - 1];
                    stream.VideoSize = videoSizes[videoSizes.Count - 1];
                    stream.CreateCommand(Instructor___Omlate.Stream.StreamClass.StreamType.WebCamStream);
                    stream.StartStream();
                    streamStarted = true;
                }
                else
                {
                    MessageBox.Show("No WebCam found");
                }
            }
            else
            {
                MessageBox.Show("Stream Already Started");
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            pptLoader.ChangeDisplayToNormal();
            Application.Exit();
        }

        private void openFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDiag.ShowDialog() == DialogResult.OK)
            {
                lectureFileName = openFileDiag.FileName;
                pptLoader.LoadPPTSlides(lectureFileName, slideShowPanel);
            }
            else
            {
                MessageBox.Show("No File Selected");
            }
        }

        private void nextSlideButton_Click(object sender, EventArgs e)
        {
            if (pptLoader.Presentation.SlideShowWindow.View.Slide.SlideNumber < pptLoader.Presentation.Slides.Count)
            {
                SaveImagePlusTime();
                pptLoader.Presentation.SlideShowWindow.View.Next();
            }
        }

        private void backSlideButton_Click(object sender, EventArgs e)
        {
            if (pptLoader.Presentation.SlideShowWindow.View.Slide.SlideNumber > 1)
            {
                SaveImagePlusTime();
                pptLoader.Presentation.SlideShowWindow.View.Previous();
            }
        }
        private void SaveImagePlusTime()
        {
            slides++;
            TimeSpan time = (DateTime.Now - startTime);
            hoGen.WriteSlideTime(time.ToString());
            hoGen.SaveScreenShoot(slides);
        }

        private void streamDesktopButton_Click(object sender, EventArgs e)
        {
            if (!streamStarted)
            {
                //startcam.IsEnabled = false;
                if (device.Mics.Count > 0)
                {
                    if (device.WebCams.Contains("screen-capture-recorder"))
                    {
                        desktopStream = true;
                        stream.VideoDevice = "screen-capture-recorder";
                        stream.AudioDevice = device.Mics[0];
                        stream.StreamName = "class1";
                        stream.CreateCommand(Instructor___Omlate.Stream.StreamClass.StreamType.DesktopStream);
                        stream.StartStream();
                        streamStarted = true;
                        slides = 0;
                        startTime = DateTime.Now;
                        hoGen = new HandoutGenerator(startTime);
                        hoGen.StartRecordingAudio();
                        timer.IsEnabled = true;
                        timer.Start();
                    }
                    else
                    {
                        MessageBox.Show("Desktop Capture Component is missing");
                    }
                }
                else
                {
                    MessageBox.Show("No Audio Device not Found");
                }
            }
            else
            {
                MessageBox.Show("Stream Already Started");
            }
        }

        private void endLectureButton_Click(object sender, EventArgs e)
        {
            pptLoader.ChangeDisplayToNormal();
            if (desktopStream)
            {
                SaveImagePlusTime();
                timer.IsEnabled = false;
                timer.Stop();

                hoGen.StopRecordingAudio();
                slides = 0;
                hoGen = null;

                stream.EndStream();
                streamStarted = false;
                MessageBox.Show("Lecture Ended");
                slideShowPanel.Controls.Clear();
            }
            else
            {
                stream.EndStream();
                streamStarted = false;
            }
            //startdesktop.IsEnabled = true;
            //next.IsEnabled = true;
            //startcam.IsEnabled = true;
        }

        private void chatButton_Click(object sender, EventArgs e)
        {
            ChatBox.MainWindow mn = new ChatBox.MainWindow();
            TopMost = false;
            mn.ShowDialog();
            mn.Topmost = true;
        }
    }
}
