using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.EventCenter;
using UnityEngine;

public class MyEventTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            EventCenter.Publish(MyEventType.Level2End);
        }
    }
}
