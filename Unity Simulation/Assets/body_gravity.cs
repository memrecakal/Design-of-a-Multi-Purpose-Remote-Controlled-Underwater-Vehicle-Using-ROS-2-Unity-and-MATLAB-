//Mehmet Emre Çakal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class body_gravity : MonoBehaviour
{
    private Rigidbody rb;
    private Transform massCenter;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        massCenter = GameObject.Find("center_of_mass").GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        rb.AddForceAtPosition(- Vector3.up * rb.mass * 9.81f, massCenter.position);
    }
}
