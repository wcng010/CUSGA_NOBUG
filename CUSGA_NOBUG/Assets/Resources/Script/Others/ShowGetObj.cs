using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGetObj : MonoBehaviour
{
    private Vector3 pos;
    void Start()
    {
        pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f , Screen.height / 2.0f,-5.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
