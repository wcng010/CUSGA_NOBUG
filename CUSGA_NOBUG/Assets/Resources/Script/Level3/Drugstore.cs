using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drugstore : Ohters<Drugstore>
{

    public override void Start()
    {
        base.Start();
        inter.index = PlayerPrefs.GetInt("Drugstore", 0);
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("Drugstore", 0) != inter.index)
            PlayerPrefs.SetInt("Drugstore", inter.index);

        if(inter.index == 1)
        FindneedObject();

        if (inter.index == 2)
            StartCoroutine(UseObj());

        inter.InteractionChat();
    }
}
