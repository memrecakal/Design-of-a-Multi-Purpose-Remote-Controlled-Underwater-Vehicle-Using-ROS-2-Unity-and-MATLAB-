using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
public class ros_subs_to_text : MonoBehaviour

{
    private StringSubscriber ss;
    
    // Start is called before the first frame update
    void Start()
    {
        ss = GetComponent<StringSubscriber>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(ss.messageData);
    }
}
}
