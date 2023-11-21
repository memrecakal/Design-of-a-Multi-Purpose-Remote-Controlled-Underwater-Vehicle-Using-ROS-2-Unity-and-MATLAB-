using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class JoystickHandler : MonoBehaviour
    {
        public FixedJoystick variableJoystick;
        public float turnMultip = 0.3f;
        private Vector3 direction;
        public float leftActivation;
        public int leftActivationMapped;
        public float rightActivation;
        public int rightActivationMapped;


        private void FixedUpdate() {
            joystick_handler();
        }

        public void joystick_handler() {
            direction =  Vector3.forward * (variableJoystick.Vertical)*100 + Vector3.right * (variableJoystick.Horizontal)*100;
            
            leftActivation = ((variableJoystick.Vertical)*100*(1-turnMultip) + (variableJoystick.Horizontal)*100*turnMultip);
            leftActivationMapped = (int)Remap(leftActivation, 0, 82, 0, 100);
        
            rightActivation = ((variableJoystick.Vertical)*100*(1-turnMultip) - (variableJoystick.Horizontal)*100*turnMultip);
            rightActivationMapped = (int)Remap(rightActivation, 0, 82, 0, 100);
        }

        public float Remap (float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

    }
}
