using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother_3 : Ohters<Mother_3>
{
    // Update is called once per frame
    void Update()
    {
        FindneedObject();

        inter.InteractionChat();

        if(inter.index == 1 && status)
        {
            StartCoroutine(UseObj());
            status = false;
            //结束
        }
    }
}
