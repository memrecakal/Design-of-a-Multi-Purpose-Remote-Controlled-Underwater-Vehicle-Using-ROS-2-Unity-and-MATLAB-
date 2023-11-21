//Mehmet Emre Çakal

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
public class angle_slider_controller : UnityPublisher<MessageTypes.Std.String>
{

    private MessageTypes.Std.String message;
    public Slider slider_obj;
    public Text desired_angle_text;

    protected override void Start()
    {
        base.Start();
        InitializeMessage();
        //slider_obj.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
    }

    public void InitializeMessage()
    {
        base.startagain();
        message = new MessageTypes.Std.String();
    }

    /*
    public void ValueChangeCheck() { //onSliderChanged(float value)
        message.data = slider_obj.value.ToString();
        Publish(message);
    }
    */

    private void FixedUpdate() {
        message.data = slider_obj.value.ToString();
        //desired_angle_text.text = "Desired Angle: " + slider_obj.value.ToString();
        //message.data = desired_angle;    
        Publish(message);
    }
}
}
