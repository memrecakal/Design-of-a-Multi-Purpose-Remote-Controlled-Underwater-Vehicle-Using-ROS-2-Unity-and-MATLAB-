using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
public class slider_controller : MonoBehaviour
{
    private Slider actual_angle_slider_comp;
    private Slider actual_depth_slider_comp;
    private Slider desired_angle_slider_comp;
    private Slider desired_depth_slider_comp;
    private sensor_imu_sub sensor_Imu_Sub_comp;

    public float depthDesired_float;
    public float angleDesired_float;

    void Start()
    {
        actual_angle_slider_comp = GameObject.Find("actual_angle_slider").GetComponent<Slider>();
        actual_depth_slider_comp = GameObject.Find("actual_depth_slider").GetComponent<Slider>();
        desired_angle_slider_comp = GameObject.Find("desired_angle_slider").GetComponent<Slider>();
        desired_depth_slider_comp = GameObject.Find("desired_depth_slider").GetComponent<Slider>();

        sensor_Imu_Sub_comp = GetComponent<sensor_imu_sub>();
    }

    
    void FixedUpdate()
    {
        // Sub to sensor/imu and change slider's value
        actual_angle_slider_comp.value = sensor_Imu_Sub_comp.rotation_vector[2];

        // Uptade desired values
        depthDesired_float = desired_depth_slider_comp.value;
        angleDesired_float = desired_angle_slider_comp.value;

    }
}
}
