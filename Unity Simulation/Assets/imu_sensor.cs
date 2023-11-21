//Mehmet Emre Çakal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imu_sensor : MonoBehaviour
{
    private Transform tf;
    public float angle_of_attack;
    void Start()
    {
        tf = GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        angle_of_attack =  - (tf.eulerAngles.z - 270f); 
    }
}
