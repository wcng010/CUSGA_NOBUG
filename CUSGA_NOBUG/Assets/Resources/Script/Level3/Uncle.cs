using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uncle : Ohters<Uncle>
{
    public override void Start()
    {
        base.Start();
        if(PlayerPrefs.GetInt("uncle",0) == 2)
        {
            inter.index = 2;
            Student.Instance.evenIndex++;
        }

    }
    void Update()
    {
        if (!Student.Instance.gameObject.activeInHierarchy && Student.Instance.evenIndex == 2)
            Student.Instance.gameObject.SetActive(true);

        FindneedObject();

        inter.InteractionChat();

        if (inter.index == 1)
        {
            StartCoroutine(UseObj());
            inter.index++;
            PlayerPrefs.SetInt("uncle", 2);
            Student.Instance.evenIndex++;
        }
    }
}
