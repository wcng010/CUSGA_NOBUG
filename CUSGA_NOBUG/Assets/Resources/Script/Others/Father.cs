using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.EventCenter;
using UnityEngine;

public class Father : Ohters<Father>
{
    [SerializeField] [Header("转场Transform")]
    private Transform endTransform;

    private void OnEnable()
    {
        EventCenter.Subscribe(MyEventType.Level2End,Level2Exit3);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(MyEventType.Level2End,Level2Exit3);
    }


    void Update()
    {
        inter.InteractionChat();
    }
    private void Level2Exit3()
    {
        transform.position = endTransform.position;
    }
}
