using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using System.IO;

namespace HandoutsGeneration
{
    class AudioRecorder
    {
        WaveIn waveIn;
        WaveFileWriter writer;
        public AudioRecorder(String fPath)
        {
            SelectedDeviceNo = -1;
            SelectedDeviceDetails = null;
            FolderPath = fPath;
        }
        /// <summary>
        /// Get the recording device
        /// </summary>
        public void GetRecordingDevices()
        {
            int recordingDevicesCount = WaveIn.DeviceCount;
            if (recordingDevicesCount > 0)
            {
                SelectedDeviceNo = 0;
                WaveInCapabilities device = WaveIn.GetCapabilities(SelectedDeviceNo);
                SelectedDeviceDetails = device.ProductName + device.ManufacturerGuid + " at Channel: " + device.Channels;
            }
        }

        void waveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            RecordingStatus = false;
        }

        /// <summary>
        /// Check if the selected devices is working
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsDeviceFound()
        {
            if (SelectedDeviceNo != -1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Start the Audio recording 
        /// </summary>
        public void StartRecording()
        {
            if (IsDeviceFound())
            {
                waveIn = new WaveIn();
                waveIn.DeviceNumber = SelectedDeviceNo;
                waveIn.DataAvailable += waveIn_DataAvailable;
                waveIn.RecordingStopped += waveIn_RecordingStopped;
                int sampleRate = 8000;
                int channels = 1;
                waveIn.WaveFormat = new WaveFormat(sampleRate, channels);

                waveIn.StartRecording();
                RecordingStatus = true;
                writer = new WaveFileWriter(@FolderPath + "Audio.WAV", waveIn.WaveFormat);
            }
        }

        /// <summary>
        /// Stop the recording and Make Object to null
        /// </summary>
        public void StopRecording()
        {
            if(waveIn!=null)
            {
                waveIn.StopRecording();
                writer.Close();
            }
            writer = null;
            waveIn = null;
        }
        /// <summary>
        /// Event on when audio data in incoming. If so, write the data to file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            //save the audio to file
            if (RecordingStatus)
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            }
        }

        
        private int SelectedDeviceNo { get; set; }

        private string SelectedDeviceDetails { get; set; }

        private bool RecordingStatus { get; set; }

        public String FolderPath { get; set; }
    }
}
