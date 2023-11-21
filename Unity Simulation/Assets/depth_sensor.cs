//Mehmet Emre Çakal
/*
  Intentional delay and noise is removed due to further 
calibration of the simulation.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class depth_sensor : MonoBehaviour
{
    private Transform tf;
    public float depth;
    void Start()
    {
        tf = GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        depth = tf.position.y;
    }
}
