using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.EventCenter;
using UnityEngine;

public class Kid : Ohters<Kid>
{
    
    public GameObject showObj;

    [SerializeField] [Header("转场Transform")]
    private Transform endTransform;
    void Update()
    {
        FindneedObject();

        inter.InteractionChat();

        if(inter.index == 1)
        {
            StartCoroutine(UseObj());
            inter.index++;
            showObj.SetActive(true);
            Level2_Finish.Instance.ChatIndex++;
        }
    }
    private void OnEnable()
    {
        EventCenter.Subscribe(MyEventType.Level2End,Level2Exit1);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(MyEventType.Level2End,Level2Exit1);
    }

    private void Level2Exit1()
    {
        transform.position = endTransform.position;
    }
}
