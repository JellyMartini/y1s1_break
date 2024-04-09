using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver_Titanfall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cancel") > 0f) Application.Quit();
    }
}
