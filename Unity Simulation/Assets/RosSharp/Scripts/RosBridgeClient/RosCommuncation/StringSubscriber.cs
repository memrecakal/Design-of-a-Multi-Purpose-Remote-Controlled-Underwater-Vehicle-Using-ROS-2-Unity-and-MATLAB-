//Mehmet Emre Çakal

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class StringSubscriber : UnitySubscriber<MessageTypes.Std.String>
    {
        public bool isMessageReceived;
        public string messageData;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Std.String message)
        {
            messageData = message.data;
            isMessageReceived = true;
        }
    }
}