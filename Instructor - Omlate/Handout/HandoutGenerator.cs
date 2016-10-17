using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Frapper;

namespace HandoutsGeneration
{
    class HandoutGenerator
    {
        StreamWriter timingFile;
        String imgFolderPath;
        ScreenCapture scCapt;
        AudioRecorder audRec;
        FFmpegWrapper ffWrpr;


        /// <summary>
        /// initilize the new HandoutGenerator Object for lecture time
        /// </summary>
        /// <param name="lecTime">The lecture time</param>
        public HandoutGenerator(DateTime lecTime)
        {
            ffWrpr = new FFmpegWrapper();
            LectureTime = lecTime;
            Directory.CreateDirectory(@"./" + LectureTime.ToString("HHmmddMMyyyy"));
            String fileName = @"./" + LectureTime.ToString("HHmmddMMyyyy") + "/LectureTiming.txt";

            imgFolderPath = @"./" + LectureTime.ToString("HHmmddMMyyyy") + "/LectureImages/";
            Directory.CreateDirectory(imgFolderPath);
            timingFile = new StreamWriter(File.OpenWrite(fileName));

            scCapt = new ScreenCapture();
            audRec = new AudioRecorder(@"./" + LectureTime.ToString("HHmmddMMyyyy") + "/");
            audRec.GetRecordingDevices();


        }

        /// <summary>
        /// Write the time to file on slide change
        /// </summary>
        /// <param name="time"></param>
        public void WriteSlideTime(String time)
        {
            timingFile.WriteLine(time);
        }

        /// <summary>
        /// Save the Screenshot when slide is going to change
        /// </summary>
        /// <param name="slideNo">The current sldie number</param>
        public void SaveScreenShoot(int slideNo)
        {
            scCapt.TakeScreenShot().Save(imgFolderPath + slideNo + ".Jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        /// <summary>
        /// Start Recording the audio voice for lecture
        /// </summary>
        public void StartRecordingAudio()
        {
            audRec.StartRecording();
        }

        /// <summary>
        /// Stop Recording the audio voice for lecture
        /// </summary>
        public void StopRecordingAudio()
        {
            audRec.StopRecording();
            timingFile.Close();
            this.GenerateHandouts(LectureTime.ToString("HHmmddMMyyyy"));
        }

        public void GenerateHandouts(String lecturePath)
        {
            MakeAudioParts(lecturePath);
            CombineImagesWithAudios(lecturePath);
            CombineVideoClips(lecturePath);
        }
        /// <summary>
        /// Divied the output audio file into part with text file as input
        /// </summary>
        /// <param name="audioAndTimeFilesFolder">Path to Audio and Timing File</param>
        private void MakeAudioParts(String audioAndTimeFilesFolder)
        {
            LinkedList<String> times = this.GetDivideTimings(audioAndTimeFilesFolder + "/LectureTiming.txt");
            Directory.CreateDirectory(audioAndTimeFilesFolder + "/Audios/");
            
            TimeSpan start = TimeSpan.Zero;
            TimeSpan end;
            ClipsCount = 0;

            foreach (String t in times)
            {
                ClipsCount++;
                end = TimeSpan.Parse(t);

                //split the audio
                ffWrpr.SplitAudio(audioAndTimeFilesFolder + "/Audio.WAV",
                                  start.ToString(), (end - start).ToString(),
                                  audioAndTimeFilesFolder + "/Audios/audio" + ClipsCount + ".WAV");
                
                start = end;
            }
        }

        /// <summary>
        /// Combine the Images with Audio to generate Video clips
        /// </summary>
        /// <param name="lecturePath">The Root folder of Lecture data</param>
        private void CombineImagesWithAudios(String lecturePath)
        {
            Directory.CreateDirectory(lecturePath + "/ImagesWithAudios/");

            StreamWriter wr = new StreamWriter(lecturePath + "/ImagesWithAudios/videoFiles.txt");
            //combine this splitted audio with image to create a video
            for (int i = 1; i <= ClipsCount; i++)
            {
                ffWrpr.CombineAudioAndImage(lecturePath + "/Audios/audio" + i + ".WAV",
                                            lecturePath + "/LectureImages/" + i + ".jpeg",
                                            lecturePath + "/ImagesWithAudios/Video" + i+".mov");
                //wrting the video file name to file
                wr.WriteLine("file 'Video" + i + ".mov'");
            }
            wr.Close();
        }

        /// <summary>
        /// Combine all the parts of Video Clips and generate Lecture video
        /// </summary>
        /// <param name="lecturePath">The Root folder of Lecture data</param>
        private void CombineVideoClips(String lecturePath)
        {
            ffWrpr.CombineVideoFiles(lecturePath + "/ImagesWithAudios/videoFiles.txt ", lecturePath + "/Video.mov");
        }

        /// <summary>
        /// Get the slides timng in to linked list. It will be used to split audios
        /// </summary>
        /// <param name="timingFile"></param>
        /// <returns>A linked list of audio split times</returns>
        private LinkedList<String> GetDivideTimings(String timingFile)
        {
            StreamReader timingFileReader = new StreamReader(File.OpenRead(timingFile));
            string time;
            LinkedList<String> times = new LinkedList<string>();

            while ((time = timingFileReader.ReadLine()) != null)
            {
                times.AddLast(time);
            }
            timingFileReader.Close();
            return times;
        }

        public DateTime LectureTime { get; set; }

        /// <summary>
        /// Count of Audio clips or Slides
        /// </summary>
        public int ClipsCount { get; set; }
    }
}
