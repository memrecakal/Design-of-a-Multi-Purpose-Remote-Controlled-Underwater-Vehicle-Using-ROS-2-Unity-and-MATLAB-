using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RosSharp.RosBridgeClient
{
public class connection_status : MonoBehaviour
{
    private RosConnector rosConnector;
    private Text statusText;
    void Start()
    {
        rosConnector = GetComponent<RosConnector>();
        statusText = GameObject.Find("statusinfo_text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rosConnector.IsConnected.WaitOne(1)) {
            statusText.text = "Connected";
            statusText.color = Color.green;
        }
        else {
            statusText.text = "Disconnected";
            statusText.color = Color.red;
        }
    }
}
}
