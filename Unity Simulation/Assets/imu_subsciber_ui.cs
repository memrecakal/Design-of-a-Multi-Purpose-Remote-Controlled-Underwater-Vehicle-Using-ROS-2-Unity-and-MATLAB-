//Mehmet Emre Çakal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
public class imu_subsciber_ui : UnitySubscriber<MessageTypes.Std.String>
{
        public bool isMessageReceived;
        public string messageData;
        public Slider actual_angle_slider;
        public Text actual_angle_text;

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
            actual_angle_slider.value = -float.Parse(messageData);
            actual_angle_text.text = "Sensor Angle: " + (-float.Parse(messageData)).ToString(); 
            Debug.Log(messageData);
        }

}
}
