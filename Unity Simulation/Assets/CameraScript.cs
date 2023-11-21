using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour

{
    static WebCamTexture backCam;
    // Start is called before the first frame update
    void Start()
    {

        backCam = new WebCamTexture();
        GetComponent<Renderer>().material.mainTexture = backCam;
        backCam.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
