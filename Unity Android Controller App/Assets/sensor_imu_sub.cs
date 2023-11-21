﻿//Mehmet Emre Çakal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
public class sensor_imu_sub : UnitySubscriber<MessageTypes.Std.String>
{
        public bool isMessageReceived;
        public string messageData;
        private Transform modelTf;
        private string[] rotation_array;
        private string[] last_rotation_array;
        public Vector3 rotation_vector;
        private Vector3 last_rotation_vector;

        private float[] raw_sensor = new float[3];
        private float[] reliable_sensor = new float[3];
        private float[] last_reliable_sensor = new float[3];
        public float reliable_th = 200f;
        private bool first = true;

        

        protected override void Start()
        {
            base.Start();
            modelTf = GameObject.Find("submodel").GetComponent<Transform>();
        }
        protected override void ReceiveMessage(MessageTypes.Std.String message)
        {
            messageData = message.data;
            isMessageReceived = true;
        }
        public void startAgain() {
            Start();
            modelTf = GameObject.Find("submodel").GetComponent<Transform>();
        }
        private void FixedUpdate() {
            try {
                rotation_array = messageData.ToString().Split(' ');
                rotation_vector = new Vector3(float.Parse(rotation_array[1]), float.Parse(rotation_array[2]), float.Parse(rotation_array[0]));
                last_rotation_vector = rotation_vector;
            }
            catch (System.Exception e){
                Debug.Log(e);
                rotation_vector = last_rotation_vector;
            }

            modelTf.eulerAngles = rotation_vector - new Vector3(0f,0f,90f);
        }

        private Vector3 getDataFromImu() {
            rotation_array = messageData.ToString().Split(' ');
            raw_sensor = new float[3] {float.Parse(rotation_array[1]) * 3f, float.Parse(rotation_array[2]), float.Parse(rotation_array[0]) * 3f};

            if (first) {
                last_reliable_sensor = raw_sensor;
                first = false;
            }

            try {
                for (int i = 0; i<3; i++) {
                    Debug.Log($"{last_reliable_sensor[i]} {raw_sensor[i]}");
                    if (Mathf.Abs(last_reliable_sensor[i] - raw_sensor[i]) < reliable_th) {
                        reliable_sensor[i] = raw_sensor[i];
                    }
                }
                last_reliable_sensor = reliable_sensor;
            }

            catch {
                reliable_sensor = last_reliable_sensor;
                Debug.Log("YOK");
            }

            return new Vector3(reliable_sensor[1], reliable_sensor[2], reliable_sensor[0]);

        }

}
}