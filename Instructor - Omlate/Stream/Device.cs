using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video.DirectShow;

namespace Instructor___Omlate.Stream
{
    public class Device
    {
        public List<string> WebCams { get; set; }
        private List<string> WebCamsMonkList { get; set; }
        public List<string> Mics { get; set; }
        private List<string> MicsMonkList { get; set; }
        private FilterInfoCollection videoDevicesCollection;
        private FilterInfoCollection audioDevicesCollection;
        /// <summary>
        /// The Device Constructor get all the audio and video devices
        /// </summary>
        public Device()
        {
            videoDevicesCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            audioDevicesCollection = new FilterInfoCollection(FilterCategory.AudioInputDevice);
            
            GetDevicesNamesAndMonks();
        }
        /// <summary>
        /// Saves the Monk names of devices
        /// </summary>
        private void GetDevicesNamesAndMonks()
        {
            WebCams = new List<string>();
            WebCamsMonkList = new List<string>();
            Mics = new List<string>();
            MicsMonkList = new List<string>();

            //set video devics lsit
            for (int i = 0; i < videoDevicesCollection.Count; i++)
            {
                WebCams.Add(videoDevicesCollection[i].Name);
                WebCamsMonkList.Add(videoDevicesCollection[i].MonikerString);
            }
            for (int i = 0; i < audioDevicesCollection.Count; i++)
            {
                Mics.Add(audioDevicesCollection[i].Name);
                MicsMonkList.Add(audioDevicesCollection[i].MonikerString);
            }
        }
        /// <summary>
        /// Get Device settings
        /// </summary>
        /// <param name="deviceName">Device Name</param>
        /// <returns>VideoCapabilities array with Frame rate and Frame size of each element</returns>
        public VideoCapabilities[] GetDeviceCapablities(string deviceName)
        {
            VideoCaptureDevice d = new VideoCaptureDevice(GetMonkString(deviceName));
            return d.VideoCapabilities;
        }
        /// <summary>
        /// Get the Monk string of given device
        /// </summary>
        /// <param name="deviceName">Device Name</param>
        /// <returns>String the Monk String</returns>
        private string GetMonkString(string deviceName)
        { 
            if(WebCams.IndexOf(deviceName)!=-1)
            {
                return WebCamsMonkList[WebCams.IndexOf(deviceName)];
            }
            return null;
        }
    }
}
