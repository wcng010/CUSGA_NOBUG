using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using Unity.VisualScripting;
using UnityEngine;

public class Door : Ohters<Door>
{
    public SpriteRenderer sprRen;

    public Sprite sprite;
    public override void Start()
    {
        base.Start();
        BagManager.Instance.UseObject += useDoor;
    }

    void Update()
    {
        FindneedObject();
        inter.InteractionChat();
        //if (inter.index == 1 && Input.GetKeyDown(KeyCode.F))
        //{            
        //    ShowObject();
        //}
    }

    private void useDoor()
    {
        if (inter.sprite.enabled)
        {
            ShowObject();
            BagManager.Instance.UsedCount++;
        }
    }

    public override void ShowObject()
    {
        sprRen.sprite = sprite;
        base.ShowObject();
    }
}
