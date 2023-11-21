//Mehmet Emre Çakal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

public class syringeForce : MonoBehaviour
{
    private GameObject rear_syringe_go;
    private GameObject front_syringe_go;
    private Rigidbody rb;

    private front_syringe_sub front_Syringe_Subscriber;
    private rearSyringeSub rear_Syringe_Subscriber; 


    private float rear_syringe_occupancy = 0.5f;
    private float front_syringe_occupancy = 0.5f; 
    
    
    public float max_water_mass_capacity = 0.2f; //kg, per syringe
    public float syringe_distance = 0.2f; //m
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        front_Syringe_Subscriber = GetComponent<front_syringe_sub>();
        rear_Syringe_Subscriber = GetComponent<rearSyringeSub>();

        
        rear_syringe_go = GameObject.Find("rear_syringe");
        front_syringe_go = GameObject.Find("front_syringe");
        
    }

    float calc_syringe_force(float syringe_occupancy) {
        //  Total buoyancy force of the vehicle is calculated and calibrated when 
        // the syringes are half full. 
        return max_water_mass_capacity * (syringe_occupancy - 0.5f) * -9.81f;
    }

    void FixedUpdate()
    {
        front_syringe_occupancy = front_Syringe_Subscriber.front_syringe_occupancy_from_matlab;
        rear_syringe_occupancy = rear_Syringe_Subscriber.rear_syringe_occupancy_from_matlab;

        rb.AddForceAtPosition(Vector3.up * calc_syringe_force(rear_syringe_occupancy), rear_syringe_go.transform.position);
        rb.AddForceAtPosition(Vector3.up * calc_syringe_force(front_syringe_occupancy), front_syringe_go.transform.position);
    
        //rb.AddTorque(Vector3.forward * calc_syringe_force(rear_syringe_occupancy) * syringe_distance);
        //rb.AddTorque(Vector3.forward * calc_syringe_force(front_syringe_occupancy) * syringe_distance);
    }
}
