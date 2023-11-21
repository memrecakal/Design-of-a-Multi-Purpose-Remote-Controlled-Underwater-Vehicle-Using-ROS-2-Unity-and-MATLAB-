//Mehmet Emre Çakal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
public class depth_subsciber_ui : UnitySubscriber<MessageTypes.Std.String>
{
        public bool isMessageReceived;
        public string messageData;
        public Slider actual_depth_slider;
        public Text actual_depth_text;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Std.String message)
        {
            messageData = message.data;
            isMessageReceived = true;
        }

        private void FixedUpdate() {
            actual_depth_slider.value = -float.Parse(messageData);
            actual_depth_text.text = "Sensor Depth: " + (-float.Parse(messageData)).ToString();
        }

}
}
