//Mehmet Emre Çakal
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class rearSyringeSub : UnitySubscriber<MessageTypes.Std.String>
    {
        public bool isMessageReceived;
        public float rear_syringe_occupancy_from_matlab;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Std.String message)
        {
            rear_syringe_occupancy_from_matlab = float.Parse(message.data);
            isMessageReceived = true;
        }
    }
}
