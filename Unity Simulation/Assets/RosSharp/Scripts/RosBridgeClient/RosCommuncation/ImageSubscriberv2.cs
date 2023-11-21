/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

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

using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class ImageSubscriberv2 : UnitySubscriber<MessageTypes.Sensor.CompressedImage>
    {
        public MeshRenderer meshRenderer;

        private Texture2D texture2D;
        private Image image_obj;
        private byte[] imageData;
        private bool isMessageReceived;
        private Sprite m_sprite;

        protected override void Start()
        {
			base.Start();
            image_obj = GetComponent<Image>();
            texture2D = new Texture2D(1, 1);
            meshRenderer.material = new Material(Shader.Find("Standard"));
        }
        private void Update()
        {
            if (isMessageReceived)
                ProcessMessagee();
        }

        protected override void ReceiveMessage(MessageTypes.Sensor.CompressedImage compressedImage)
        {
            imageData = compressedImage.data;
            isMessageReceived = true;
        }

        private void ProcessMessagee()
        {
            //m_sprite = ImageConversion.EncodeToJPG(imageData);
            image_obj.sprite = m_sprite;
            isMessageReceived = false;
        }

    }
}

