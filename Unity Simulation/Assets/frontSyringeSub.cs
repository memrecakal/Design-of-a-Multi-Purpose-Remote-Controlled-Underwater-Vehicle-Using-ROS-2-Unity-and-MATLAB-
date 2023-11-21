using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class frontSyringeSub : UnitySubscriber<MessageTypes.Std.String>
    {
        
        public bool isMessageReceived;
        public string front_syringe_occupancy_from_matlab;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Std.String message)
        {
            front_syringe_occupancy_from_matlab = message.data;
            isMessageReceived = true;
        }
        


    }
}
