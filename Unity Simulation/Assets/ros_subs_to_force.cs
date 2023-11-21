//Mehmet Emre Çakal
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
public class ros_subs_to_force : MonoBehaviour

{   public float force_multip = 10;
    private StringSubscriber ss;
    private Rigidbody rb;
    public GameObject left_prop;
    public GameObject right_prop;
    private Vector3 normalized_joy_vector;

    private float left_prop_activation; 
    private float right_prop_activation; 

    public static Vector3 stringToVector3(string s){
        float x = 0;
        float y = 0;
        float z = 0;

        try {
            char[] charsToTrim = {'(', ')'};
            string result = s.Trim(charsToTrim);
            string[] subs = result.Split(',');
            x = float.Parse(subs[0]);
            y = float.Parse(subs[1]);
            z = float.Parse(subs[2]);
        }
        finally {
        }
    
        return new Vector3(x,y,z);
    }
    
    void Start()
    {
        ss = GetComponent<StringSubscriber>();
        rb = GetComponent<Rigidbody>();
        //left_prop = GameObject.Find("left_propeller");
        //right_prop = GameObject.Find("right_propeller");
    }

    void FixedUpdate()
    {
        if (ss.isMessageReceived) {
            normalized_joy_vector = (stringToVector3(ss.messageData) - new Vector3(0.5f, 0f, 0.5f)) * 2f;


            left_prop_activation = normalized_joy_vector.z + normalized_joy_vector.x / 2f;
            right_prop_activation = normalized_joy_vector.z - normalized_joy_vector.x / 2f;

            rb.AddForceAtPosition((left_prop.transform.up * left_prop_activation).normalized * force_multip, left_prop.transform.position);
            rb.AddForceAtPosition((right_prop.transform.up * right_prop_activation).normalized * force_multip, right_prop.transform.position);


        }
        if (Input.GetKey(KeyCode.D)) {
                rb.AddForceAtPosition(1f * left_prop.transform.up * force_multip, left_prop.transform.position);
            }

        if (Input.GetKey(KeyCode.A)) {
                rb.AddForceAtPosition(1f * right_prop.transform.up * force_multip, right_prop.transform.position);
            }
        
    }
}
}
