//Mehmet Emre Çakal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
public class capsule_depth_sensor_publisher : UnityPublisher<MessageTypes.Std.String>
{
    private depth_sensor depth_Sensor;
    private MessageTypes.Std.String message;
    
    void Start()
    {
        base.Start();
        InitializeMessage();
        depth_Sensor = GameObject.Find("DepthSensor").GetComponent<depth_sensor>();
    }

    public void InitializeMessage()
        {
            base.Start();
            message = new MessageTypes.Std.String();
        }

    public void UpdateMessage()
        {
            message.data = depth_Sensor.depth.ToString();
            Publish(message);
        }

    void FixedUpdate()
    {
        UpdateMessage();
    }
}
}
