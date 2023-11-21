/* 
    Author: Siemens AG, <https://github.com/siemens/ros-sharp> 
    Adjustments: Compressed Image class is updated and old header 
        class inheriance is removed for ROS2 support.
    Bilkent University, 2022, Mehmet Emre Çakal (emre.cakal@ug.bilkent.edu.tr)
*/

using RosSharp.RosBridgeClient.MessageTypes.Std;

namespace RosSharp.RosBridgeClient.MessageTypes.Sensor
{
    public class ModifiedCompressedImage : Message
    {
        public const string RosMessageName = "sensor_msgs/CompressedImage";

        //  This message contains a compressed image
        public ModifiedHeader header { get; set; }
        //  Header timestamp should be acquisition time of image
        //  Header frame_id should be optical frame of camera
        //  origin of frame should be optical center of camera
        //  +x should point to the right in the image
        //  +y should point down in the image
        //  +z should point into to plane of the image
        public string format { get; set; }
        //  Specifies the format of the data
        //    Acceptable values:
        //      jpeg, png
        public byte[] data { get; set; }
        //  Compressed image buffer

        public ModifiedCompressedImage()
        {
            this.header = new ModifiedHeader();
            this.format = "";
            this.data = new byte[0];
        }

        public ModifiedCompressedImage(ModifiedHeader header, string format, byte[] data)
        {
            this.header = header;
            this.format = format;
            this.data = data;
        }
    }
}