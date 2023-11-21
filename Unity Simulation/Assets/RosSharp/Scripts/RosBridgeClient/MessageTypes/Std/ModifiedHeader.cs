/* 
    Author: Siemens AG, <https://github.com/siemens/ros-sharp> 
    Adjustments: Header class is updated and sequance bytes were removed
        for ROS2 support.
    Bilkent University, 2022, Mehmet Emre Çakal (emre.cakal@ug.bilkent.edu.tr)
*/

namespace RosSharp.RosBridgeClient.MessageTypes.Std
{
    public class ModifiedHeader : Message
    {
        public const string RosMessageName = "std_msgs/Header";

        //  Standard metadata for higher-level stamped data types.
        //  This is generally used to communicate timestamped data 
        //  in a particular coordinate frame.
        //  
        //  sequence ID: consecutively increasing ID 
        public Time stamp { get; set; }
        // Frame this data is associated with
        //  0: no frame
        //  1: global frame
        public string frame_id { get; set; }

        public ModifiedHeader()
        {
            this.stamp = new Time();
            this.frame_id = "";
        }

        public ModifiedHeader(Time stamp, string frame_id)
        {
            this.stamp = stamp;
            this.frame_id = frame_id;
        }
    }
}
