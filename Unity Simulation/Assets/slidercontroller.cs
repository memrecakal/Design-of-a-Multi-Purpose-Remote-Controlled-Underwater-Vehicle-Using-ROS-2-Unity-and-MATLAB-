using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
public class slidercontroller : UnityPublisher<MessageTypes.Std.String>
{

    private MessageTypes.Std.String message;
    public Slider slider_obj;
    public Text desired_depth_text;

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
        desired_depth_text.text = "Desired Depth: " + slider_obj.value.ToString();
        Publish(message);
    }
}
}
