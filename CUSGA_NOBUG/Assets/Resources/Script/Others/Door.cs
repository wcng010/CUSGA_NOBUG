using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : Ohters<Door>
{
    public override void Start()
    {
        base.Start();
        BagManager.Instance.useObject += useDoor;
    }

    void Update()
    {
        FindneedObject();
        inter.InteractionChat();
        if (inter.index == 1 && Input.GetKeyDown(KeyCode.F))
        {
            ShowObject();
        }
    }

    private void useDoor()
    {
        if (inter.sprite.enabled)
        {
            ShowObject();
            BagManager.Instance.UsedCount++;
        }
    }

}
