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
        //FindneedObject();
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
<<<<<<< HEAD

    protected override void useObject(string objectBag, string objectName)
    {
        if (string.Compare(objectName, this.gameObject.name, StringComparison.Ordinal) != 0
            || TimelineManager.Instance.doorTimeline.state != PlayState.Playing ||
            string.Compare(objectBag, "门", StringComparison.Ordinal) != 0)
            return;
        TimelineManager.Instance.doorTimeline.Stop();
        sprRen.sprite = sprite;
        base.useObject(objectBag,objectName);
    }
=======
>>>>>>> parent of eacc404 (Commit)
}
