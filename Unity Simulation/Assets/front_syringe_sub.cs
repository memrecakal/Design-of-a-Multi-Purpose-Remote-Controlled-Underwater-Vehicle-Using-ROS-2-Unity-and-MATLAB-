//Mehmet Emre Çakal
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class front_syringe_sub : UnitySubscriber<MessageTypes.Std.String>
    {
        
        public bool isMessageReceived;
        public float front_syringe_occupancy_from_matlab;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Std.String message)
        {
            front_syringe_occupancy_from_matlab = float.Parse(message.data);
            isMessageReceived = true;
        }
    }
}
