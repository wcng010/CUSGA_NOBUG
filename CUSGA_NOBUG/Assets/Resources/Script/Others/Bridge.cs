using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Ohters<Bridge>
{
    public override void Start()
    {
        base.Start();
        BagManager.Instance.useObject += useBridge;
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
