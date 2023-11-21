//Mehmet Emre Çakal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
public class DepthTempSub : UnitySubscriber<MessageTypes.Std.String>
{
        public bool isMessageReceived;
        public string[] messageData;
        public string depth;
        public string temp;
        private Text depthStatusText;
        private Text tempStatusText;
        private Text pHStatusText;
        //Random random = new Random();

        protected override void Start()
        {
            base.Start();
            depthStatusText = GameObject.Find("DepthInfoText").GetComponent<Text>();
            tempStatusText = GameObject.Find("TempInfoText").GetComponent<Text>();
            pHStatusText = GameObject.Find("PhInfoText").GetComponent<Text>();

            
        }
        protected override void ReceiveMessage(MessageTypes.Std.String message)
        {
            messageData = message.data.Split(' ');
            depth = messageData[0].Substring(0, 5) + " m";
            temp = messageData[1].Substring(0, 5) + " °C";
            isMessageReceived = true;
        }

        private void Update() {
            depthStatusText.text = depth;
            tempStatusText.text = temp;
            pHStatusText.text = Random.Range(5.9f, 6.1f).ToString().Substring(0,4);
            //Debug.Log(messageData.ToString());
        }
}
}