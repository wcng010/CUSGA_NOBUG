using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Van : Ohters<Van>
{
    public override void Start()
    {
        base.Start();
        Reserve = true;
    }
    void Update()
    {
        FindneedObject();

        inter.InteractionChat();

        if(inter.index == 1)
        {
            GetNeedObject();
            inter.index++;
        }
    }
}
