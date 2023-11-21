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


namespace RosSharp.RosBridgeClient
{
    public class JoystickPublisher : UnityPublisher<MessageTypes.Std.String>
    {
        public FixedJoystick variableJoystick;
        private Vector3 direction;

        private MessageTypes.Std.String message1;


        //protected override

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }


        public void InitializeMessage()
        {
            base.startagain();
            message1 = new MessageTypes.Std.String();
        }

        private void FixedUpdate() {
            joystick_handler();
            message1.data = direction.ToString();
            Publish(message1);
        }

        public void joystick_handler() {
            direction =  Vector3.forward * (variableJoystick.Vertical + 1) / 2 + Vector3.right * (variableJoystick.Horizontal + 1) / 2;
        }

    }
}
