//Mehmet Emre Çakal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class body_buoyancy : MonoBehaviour
{
    private Rigidbody rb;
    private Transform buoyancyCenter;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        buoyancyCenter = GameObject.Find("center_of_buoyancy").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        rb.AddForceAtPosition(Vector3.up * rb.mass * 9.81f, buoyancyCenter.position);
    }
}
