using UnityEngine;
using UnityEngine.UI;


namespace RosSharp.RosBridgeClient
{
    public class AngleDesiredPublisher : UnityPublisher<MessageTypes.Std.String>
    {
        private slider_controller slider_Controller_comp;
        private MessageTypes.Std.String message;


        //protected override

        protected override void Start()
        {
            // String message initialization
            base.Start();

            // Connection to slider controller
            slider_Controller_comp = GetComponent<slider_controller>();
        }

        private void FixedUpdate() {
            try {
                message.data = slider_Controller_comp.angleDesired_float.ToString();
            }
            catch {
                message = new MessageTypes.Std.String();
            }
            
            Publish(message);
        }


    }
}
