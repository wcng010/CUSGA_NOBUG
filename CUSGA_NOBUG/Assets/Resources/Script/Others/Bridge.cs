using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;

public class Bridge : Ohters<Bridge>
{
    private float aphlaValue;

    public SpriteRenderer spriteRenderer;
    public override void Start()
    {
        base.Start();
        BagManager.Instance.UseObject += useBridge;
    }

    void Update()
    {
        //FindneedObject();

        inter.InteractionChat();
        
        //if(inter.index == 1 && Input.GetKeyDown(KeyCode.F))
        //{
        //    ShowObject();
        //}
    }
    private void useBridge()
    {
<<<<<<< HEAD
        if (string.Compare(objectName, gameObject.name, StringComparison.Ordinal) != 0
            || TimelineManager.Instance.bridgeTimeline.state != PlayState.Playing ||
            string.Compare(objectBag, "桥", StringComparison.Ordinal) != 0) return;
        TimelineManager.Instance.bridgeTimeline.Stop();
        spriteRenderer.enabled = true;
        base.useObject(objectBag,objectName);
=======
        if (inter.sprite.enabled)
        {
            ShowObject();
            BagManager.Instance.UsedCount++;
        }
>>>>>>> parent of eacc404 (Commit)
    }
}
