using HandoutsGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Instructor___Omlate.Stream;
using AForge.Video.DirectShow;

namespace Instructor___Omlate
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    public partial class Toolbar : Window
    {
        Device device;
        StreamClass stream;
        private List<int> frameRates;
        private List<string> videoSizes;

        bool streamStarted, desktopStream;
        public Toolbar()
        {
            InitializeComponent();
            
            device = new Device();
            stream = new StreamClass();
            stream.StreamName = "class1";

            desktopStream = false;
            this.Top = 768 - 100;
            this.Left = (1369/2) - 220;
            Loaded += Toolbar_Loaded;
        }

        void Toolbar_Loaded(object sender, RoutedEventArgs e)
        {
            if (device.WebCams.Count == 0)
            {
                MessageBox.Show("No Video Device Deteced");
            }
            else
            {
                frameRates = new List<int>();
                videoSizes = new List<string>();
                //videoDevicesCombo.Items.AddRange(device.WebCams.ToArray());
                VideoCapabilities[] caps = device.GetDeviceCapablities(device.WebCams[0]);
                for (int i = 0; i < caps.Count(); i++)
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            // Begin dragging the window
            this.DragMove();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            if (!streamStarted)
            {
                //slider = new LectureSlidesShow();
                //slider.Device = this.device;
                //slider.Stream = stream;
                //slider.Stream.StreamName = "class1";
                streamStarted = true;
                desktopStream = true;
                startcam.IsEnabled = false;

                new SlideShow().ShowDialog();
            }
            else
            {
                MessageBox.Show("Stream Already Started");
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
        //    if (desktopStream)
        //        //next slide button
        //        SaveImagePlusTime();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            if (desktopStream)
            {
                stream.EndStream();
                streamStarted = false;
            }
            startdesktop.IsEnabled = true;
            next.IsEnabled = true;
            startcam.IsEnabled = true;
        }
        

        private void startcam_Click(object sender, RoutedEventArgs e)
        {
            if (!streamStarted)
            {
                desktopStream = false;
                startdesktop.IsEnabled = false;
                next.IsEnabled = false;
                if (device.WebCams.Count > 0)
                {
                    stream.StreamName = "class1";
                    stream.AudioDevice = device.Mics[0];
                    stream.VideoDevice = device.WebCams[0];
                    stream.FrameRate = frameRates.Last();
                    stream.VideoSize = videoSizes.Last();
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

        private void lougout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void chatButton_Click(object sender, RoutedEventArgs e)
        {
            ChatBox.MainWindow mn = new ChatBox.MainWindow();
            mn.Show();
        }
    }
}
