using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.EventCenter;
using UnityEngine;

public class Mother : Ohters<Mother>
{
    [SerializeField] [Header("转场Transform")]
    private Transform endTransform;
    void Update()
    {
        inter.InteractionChat();
    }
    private void OnEnable()
    {
        EventCenter.Subscribe(MyEventType.Level2End,Level2Exit2);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(MyEventType.Level2End,Level2Exit2);
    }
    
    private void Level2Exit2()
    {
        transform.position = endTransform.position;
    }
}
