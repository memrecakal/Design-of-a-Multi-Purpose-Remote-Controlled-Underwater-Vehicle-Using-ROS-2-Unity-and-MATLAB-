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
    public class StringPublisher : UnityPublisher<MessageTypes.Std.String>
    {
        public string StringData;
        public Text text_component;
        private int counter = 0;

        private MessageTypes.Std.String message;

        //protected override

        protected override void Start()
        {
           /*  
            base.Start();
            InitializeMessage();
            
            text_component.text = "awake";
            
            counter = 0; */
        }




        public void InitializeMessage()
        {
            base.Start();
            message = new MessageTypes.Std.String();
            text_component.text = "initialize message";
        }
        public void UpdateMessage()
        {
/*             if (publicationId == "") {
                base.Start();
                InitializeMessage();
                text_component.text = "awake";
                counter = 0;
            }

 */
            message.data = StringData + " " + counter.ToString();
            text_component.text = counter.ToString();
            Publish(message);
            counter++;
        }

    }
}
