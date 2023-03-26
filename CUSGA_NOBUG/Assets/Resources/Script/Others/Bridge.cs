using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;

public class Bridge : Ohters<Bridge>
{
    private float aphlaValue;
    public override void Start()
    {
        base.Start();
        BagManager.Instance.UseObject += useBridge;
    }

    void Update()
    {
        FindneedObject();

        inter.InteractionChat();
        
        if(inter.index == 1 && Input.GetKeyDown(KeyCode.F))
        {
            ShowObject();
        }
    }
    private void useBridge()
    {
        if (inter.sprite.enabled)
        {
            ShowObject();
            BagManager.Instance.UsedCount++;
        }
    }
}
