using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class JoystickPublisherFinal : UnityPublisher<MessageTypes.Std.String>
    {
        public JoystickHandler joystickHandler;

        private MessageTypes.Std.String messageLeft;


        protected override void Start()
        {
            base.Start();
        }

        private void FixedUpdate() {
            try {
                messageLeft.data = "0"+joystickHandler.leftActivationMapped.ToString() + " 1"+joystickHandler.rightActivationMapped.ToString();
            }
            catch {
                messageLeft = new MessageTypes.Std.String();
            }
            
            Publish(messageLeft);
        }

    }
}
