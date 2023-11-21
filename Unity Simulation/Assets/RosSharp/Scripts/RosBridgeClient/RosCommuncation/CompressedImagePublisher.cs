/*
© CentraleSupelec, 2017
Author: Dr. Jeremy Fix (jeremy.fix@centralesupelec.fr)

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

// Adjustments to new Publication Timing and Execution Framework 
// © Siemens AG, 2018, Dr. Martin Bischoff (martin.bischoff@siemens.com)

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace RosSharp.RosBridgeClient
{
    public class CompressedImagePublisher : UnityPublisher<MessageTypes.Sensor.CompressedImage>
    {
        public Camera ImageCamera;
        public string FrameId = "Camera";
        public int resolutionWidth = 640;
        public int resolutionHeight = 480;
        [Range(0, 100)]
        public int qualityLevel = 50;
        public float scanPeriod = 0.4f;
        
        private MessageTypes.Sensor.CompressedImage message;
        private Texture2D texture2D;
        private Rect rect;
        private float previousScanTime = 0;

        protected override void Start()
        {
            base.Start();
            InitializeGameObject();
            InitializeMessage();
        }

        void OnEnable()
        {
            RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
        }
        
        void OnDisable()
        {
            RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
        }
        private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (Time.realtimeSinceStartup >= previousScanTime + scanPeriod)
            {
                UpdateImage(camera);
                previousScanTime = Time.realtimeSinceStartup;
            }
        }
        
        private void UpdateImage(Camera camera)
        {
            if (texture2D != null && camera == this.ImageCamera)
                UpdateMessage();
        }

        private void InitializeGameObject()
        {
            texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
            rect = new Rect(0, 0, resolutionWidth, resolutionHeight);
            ImageCamera.targetTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
        }

        public void InitializeMessage()
        {
            base.Start();
            InitializeGameObject();
            
            message = new MessageTypes.Sensor.CompressedImage();
            message.header.frame_id = FrameId;
            message.format = "jpeg";
        }

        private void UpdateMessage()
        {
            message.header.Update();
            texture2D.ReadPixels(rect, 0, 0);
            try
            {
                message.data = texture2D.EncodeToJPG(qualityLevel);
                Publish(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.Log(e);
                Debug.Log("zort");
            }
        }

    }
}