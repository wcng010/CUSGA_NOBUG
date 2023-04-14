using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Ohters<Student>
{
    [HideInInspector]
    public int evenIndex = 0;

    public override void Start()
    {
        base.Start();
        gameObject.SetActive(false);

        if(PlayerPrefs.GetInt("student",0) == 1)
        {
            inter.index = 1;
        }
    }

    void Update()
    {
        inter.InteractionChat();
    }
}
