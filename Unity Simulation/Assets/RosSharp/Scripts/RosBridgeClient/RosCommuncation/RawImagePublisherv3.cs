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

using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace RosSharp.RosBridgeClient
{
    public class RawImagePublisherv3 : UnityPublisher<MessageTypes.Sensor.ModifiedImage>
    {
        public Camera ImageCamera;
        public string FrameId = "Camera";
        public int resolutionWidth = 640;
        public int resolutionHeight = 480;
        [Range(0, 100)]
        public int qualityLevel = 50;
        public float scanPeriod = 0.4f;
        public string encoding = "rgba16";
        
        private MessageTypes.Sensor.ModifiedImage message;
        
        private Texture2D texture2D;
        private Rect rect;
        private float previousScanTime = 0;

        private bool isPerformingScreenGrab;

        protected override void Start()
        {
            base.Start();
            InitializeGameObject();
            InitializeMessage();
            Camera.onPostRender += OnPostRenderCallback;
        }

        public void InitializeMessage()
        {
            base.Start();
            InitializeGameObject();
            
            message = new MessageTypes.Sensor.ModifiedImage();
            message.header.frame_id = FrameId;
            message.width = (uint) resolutionWidth;
            message.height = (uint) resolutionHeight;

            Camera.onPostRender += OnPostRenderCallback;
        }

        void Update()
    {
        // When the user presses the space key, perform the screen grab operation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPerformingScreenGrab = true;
        }

    }

         void OnPostRenderCallback(Camera cam)
    {
        if (isPerformingScreenGrab)
        {
            // Check whether the Camera that has just finished rendering is the one you want to take a screen grab from
            if (cam == Camera.main)
            {
                // Define the parameters for the ReadPixels operation
                Rect regionToReadFrom = new Rect(0, 0, resolutionWidth, resolutionHeight);
                int xPosToWriteTo = 0;
                int yPosToWriteTo = 0;
                bool updateMipMapsAutomatically = false;

                // Copy the pixels from the Camera's render target to the texture
                texture2D.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);

                // Upload texture data to the GPU, so the GPU renders the updated texture
                // Note: This method is costly, and you should call it only when you need to
                // If you do not intend to render the updated texture, there is no need to call this method at this point
                texture2D.Apply();

                // Reset the isPerformingScreenGrab state
                isPerformingScreenGrab = false;

                message.header.Update();
                message.data = texture2D.EncodeToJPG(qualityLevel);
                message.encoding = encoding;
                Publish(message);
            }
        }
    }
        private void InitializeGameObject()
        {
            texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
            ImageCamera.targetTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
            
        }

        private void FixedUpdate() {
            isPerformingScreenGrab = true;
        }

        
        /*
        private void UpdateMessage()
        {
            
            message.header.Update();
            texture2D.ReadPixels(rect, 0, 0);
            message.data = texture2D.EncodeToJPG(qualityLevel);
            message.encoding = encoding;
            Publish(message);
        }

        private void FixedUpdate() {
            RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
            UpdateImage(ImageCamera);
            previousScanTime = Time.realtimeSinceStartup;
        }
        */
    }
}
