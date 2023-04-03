using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : Ohters<Kid>
{
    public GameObject showObj;

    void Update()
    {
        FindneedObject();

        inter.InteractionChat();

        if(inter.index == 1)
        {
            StartCoroutine(UseObj());
            inter.index++;
            showObj.SetActive(true);
        }

    }
}
