using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instructor___Omlate.Stream
{
    public class StreamClass
    {
        private string serverURL;
        public string StreamName { get; set; }
        public string VideoSize { get; set; }
        public int FrameRate { get; set; }
        public string VideoDevice { get; set; }
        public string AudioDevice { get; set; }

        private string streamCommand;
        private string ffmpegPath;
        /// <summary>
        /// StreamType enum. Webcam or Desktop
        /// </summary>
        public enum StreamType
        {
            WebCamStream,
            DesktopStream
        };
        /// <summary>
        /// StreamClass Constructor sets the serverURL and ffmpeg path
        /// </summary>
        public StreamClass()
        {
            serverURL = @"rtmp://"+Config.Red5HostIP+"/omlate/";
            ffmpegPath = @"E:\ffmpeg.exe";
        }
        /// <summary>
        /// Create the final streaming command for ffmpeg
        /// </summary>
        /// <param name="type"></param>
        public void CreateCommand(StreamType type)
        {
            streamCommand = "-f dshow ";
            if (type.Equals(StreamType.WebCamStream))
            {
                streamCommand += " -video_size " + VideoSize + " -framerate " + FrameRate
                    + " -vcodec mjpeg -i video=";
                streamCommand += "\"" + VideoDevice + "\"";
                streamCommand += ":audio=";
                streamCommand += "\"" + AudioDevice + "\"";
                streamCommand += " -rtbufsize 304128000 -f flv -f flv " + serverURL + StreamName;

               //var video="\"" + VideoDevice + "\"";
                //var audio="\"" + AudioDevice + "\"";
                //streamCommand += "-i video=" + video + " -f dshow -i audio=" + audio + " -vf scale=1280:720 -vcodec libx264 -r 60.97 -acodec libvo_aacenc -ac 2 -ar 44100 -ab 128k -rtbufsize  -pix_fmt yuv420p -tune zerolatency -preset ultrafast -f flv" + serverURL + StreamName;
            
            }
            else
            {
                streamCommand += " -i video=";
                streamCommand += "\"" + VideoDevice + "\"";
                streamCommand += ":audio=";
                streamCommand += "\"" + AudioDevice + "\"";
                streamCommand += " -r 20 -q 5 -f flv " + serverURL + StreamName;

                //streamCommand += " -i video=";
                //streamCommand += "\"" + VideoDevice + "\"";
                //streamCommand += " -f dshow -i audio=";
                //streamCommand += "\"" + AudioDevice + "\"";
                //streamCommand += " -vf scale=1280:720 -vcodec libx264" +
                //    " -r 60.97 -acodec libvo_aacenc -ac 2 -ar 44100 -ab 128 -pix_fmt yuv420p"+
                //    " -tune zerolatency -preset ultrafast -f flv " + serverURL + StreamName;
            }
        }
        /// <summary>
        /// strart stream by streamURL to ffmpeg.
        /// </summary>
        public void StartStream()
        {
            try
            {
                streamProcess = new Process();
                streamProcess.StartInfo.FileName = ffmpegPath;
                streamProcess.StartInfo.Arguments = streamCommand;
                //streamProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                streamProcess.Start();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + e.Message);
            }
        }
        /// <summary>
        /// End the current stream
        /// </summary>
        public void EndStream()
        {
            try
            {
                if (streamProcess != null)
                    if (!streamProcess.HasExited)
                        streamProcess.Kill();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + e.Message);
            }
        }

        public Process streamProcess { get; set; }
    }
}
