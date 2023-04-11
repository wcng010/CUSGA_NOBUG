using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aunt : Ohters<Aunt>
{
    void Update()
    {
        if (!Student.Instance.gameObject.activeInHierarchy && Student.Instance.evenIndex == 2)
            Student.Instance.gameObject.SetActive(true);

        FindneedObject();

        inter.InteractionChat();

        if(inter.index == 1)
        {
            StartCoroutine(UseObj());
            inter.index++;
            Student.Instance.evenIndex++;
        }
    }
}
