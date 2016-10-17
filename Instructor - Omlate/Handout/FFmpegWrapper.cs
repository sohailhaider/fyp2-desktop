using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frapper;

namespace HandoutsGeneration
{
    class FFmpegWrapper
    {
        FFMPEG ffmpeg;

        /// <summary>
        /// The FFMPEGWrapper Class Constructor
        /// </summary>
        public FFmpegWrapper()
        {
            ffmpeg = new FFMPEG(@"E:\ffmpeg.exe");
        }
        /// <summary>
        /// Split a part from Audio file
        /// </summary>
        /// <param name="inputFile">The Input Audio File</param>
        /// <param name="startTime">Time to start Split from</param>
        /// <param name="duration">Duration of Split clip</param>
        /// <param name="outputFile">The Output Audio File</param>
        public void SplitAudio(String inputFile, String startTime, String duration, String outputFile)
        {
            String splitCommand = "-i " + inputFile + " -acodec copy -t " + duration + " -ss " + startTime + " " + outputFile;
            ffmpeg.RunCommand(splitCommand);
        }

        /// <summary>
        /// Combine and input audioFile and imageFile and outputs a new video file at outputPath
        /// </summary>
        /// <param name="audioFile">The input image file</param>
        /// <param name="imageFile">The input audio file</param>
        /// <param name="outputPath">Output Video save location</param>
        public void CombineAudioAndImage(String audioFile, String imageFile, String outputPath)
        {
            String combineCommand = "-i " + imageFile + " -i " + audioFile + " -c:v copy -c:a copy " + outputPath;
            ffmpeg.RunCommand(combineCommand);
            //String combineCommand1 = "-loop 1 -i "+imageFile+" -i "+audioFile+" -c:v libx264 -c:a aac -strict experimental -b:a 192k -shortest "+outputPath+".mp4";
            //ffmpeg.RunCommand(combineCommand1);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="videoFiles"></param>
        /// <param name="outputPath"></param>
        public void CombineVideoFiles(String videoFiles, String outputPath)
        {
            String combineCommand = "-f concat -i " + videoFiles + " -c copy " + outputPath;
            ffmpeg.RunCommand(combineCommand);
        }
    }
}
