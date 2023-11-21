/*
© Siemens AG, 2017-2019
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)
Adjustments: Two new methods for Mono Behavior RosConnector class
    in the RosSharpt.RosBridgeClient namespace are written to 
    provide real-time RosBridge IP change. 
    Bilkent University, 2022, Mehmet Emre Çakal (emre.cakal@ug.bilkent.edu.tr)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

<http://www.apache.org/licenses/LICENSE-2.0>.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Threading;
using RosSharp.RosBridgeClient.Protocols;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
    public class RosConnector : MonoBehaviour
    {
        public int SecondsTimeout = 10;

        public RosSocket RosSocket { get; private set; }
        public RosSocket.SerializerEnum Serializer;
        public Protocol protocol;
        public string RosBridgeServerUrl = "ws://192.168.1.5:9090";

        public ManualResetEvent IsConnected { get; private set; }


        private Text inputurl_comp;
        private Text connection_status_comp;
        public JoystickPublisher joystickPublisher_comp;
        public slidercontroller slidercontroller_comp;
        public angle_slider_controller angle_Slider_Controller_comp;
        public ImageSubscriber ImageSubscriber;
        public modelRotation modelRotation_comp;
        private bool rosconnected = false;
        

        public virtual void Awake()
        {
            IsConnected = new ManualResetEvent(false);
            new Thread(ConnectAndWait).Start();

            if (!IsConnected.WaitOne(SecondsTimeout * 1000)) {}
        }

        private void Start() {
            inputurl_comp = GameObject.Find("rosinput_text").GetComponent<Text>();
            connection_status_comp = GameObject.Find("statusinfo_text").GetComponent<Text>();
        }

        public void ConnectAndWait()
        {
            RosSocket = ConnectToRos(protocol, RosBridgeServerUrl, OnConnected, OnClosed, Serializer);

            if (!IsConnected.WaitOne(SecondsTimeout * 1000))
                Debug.LogWarning("Failed to connect to RosBridge at: " + RosBridgeServerUrl);
        }

        public void InitializeMessages() {
            joystickPublisher_comp.InitializeMessage();
            slidercontroller_comp.InitializeMessage();
            angle_Slider_Controller_comp.InitializeMessage();
            modelRotation_comp.startAgain();
        }
        
        public void ConnectAndWaitAgain() {
            connection_status_comp.text = "Connecting...";
            connection_status_comp.color = Color.yellow;

            RosSocket.Close();

            RosBridgeServerUrl = inputurl_comp.text;

            RosSocket = ConnectToRos(protocol, RosBridgeServerUrl, OnConnected, OnClosed, Serializer);
            

            if (rosconnected == true) {
                connection_status_comp.color = Color.green;
                connection_status_comp.text = "Connected";
            } else {
                connection_status_comp.color = Color.red;
                connection_status_comp.text = "Disconnected";
            }

        }
        

        public static RosSocket ConnectToRos(Protocol protocolType, string serverUrl, EventHandler onConnected = null, EventHandler onClosed = null, RosSocket.SerializerEnum serializer = RosSocket.SerializerEnum.Microsoft)
        {
            IProtocol protocol = ProtocolInitializer.GetProtocol(protocolType, serverUrl);
            protocol.OnConnected += onConnected;
            protocol.OnClosed += onClosed;

            return new RosSocket(protocol, serializer);
        }

        private void OnApplicationQuit()
        {
            RosSocket.Close();
        }

        private void OnConnected(object sender, EventArgs e)
        {
            rosconnected = true;
            IsConnected.Set();
            Debug.Log("Connected to RosBridge: " + RosBridgeServerUrl);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            rosconnected = false;
            IsConnected.Reset();
            Debug.Log("Disconnected from RosBridge: " + RosBridgeServerUrl);
        }
    }
}