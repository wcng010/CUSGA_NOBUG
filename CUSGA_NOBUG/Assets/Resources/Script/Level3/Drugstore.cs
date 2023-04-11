using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drugstore : Ohters<Drugstore>
{
    void Update()
    {
        if(inter.index == 1)
        FindneedObject();

        if (inter.index == 2)
            StartCoroutine(UseObj());

        inter.InteractionChat();
    }
}
