//Mehmet Emre Çakal

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
public class capsule_imu_sensor_publisher : UnityPublisher<MessageTypes.Std.String>
{
    private imu_sensor imu_Sensor; 
    private MessageTypes.Std.String message;

    void Start()
    {
        base.Start();
        InitializeMessage();
        imu_Sensor = GameObject.Find("IMU").GetComponent<imu_sensor>();
    }
    public void InitializeMessage()
        {
            base.Start();
            message = new MessageTypes.Std.String();
        }
    public void UpdateMessage()
        {
            message.data = imu_Sensor.angle_of_attack.ToString();
            Publish(message);
        }
    void FixedUpdate()
    {
        UpdateMessage();
    }
}
}
