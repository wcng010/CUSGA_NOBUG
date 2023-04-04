using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class Tree : Ohters<Tree>
{
    [SerializeField] [Header("叶子Transform")]
    private Transform leafTrans;

    public GameObject[] Shadow;
    public override void Start()
    {
        base.Start();
        BagManager.Instance.UseObject += UseLeaf;
    }

    public void OnDisable()
    {
        BagManager.Instance.UseObject -= UseLeaf;
    }

    void Update()
    {
        inter.InteractionChat();
    }


    private void UseLeaf(string objectBag, string objectName)
    {
        StartCoroutine(useLeaf(objectBag,objectName));
    }


    IEnumerator useLeaf(string objectBag, string objectName)
    {
        if (string.Compare(objectName, "Leaf", StringComparison.Ordinal) != 0
            || TimelineManager.Instance.leafTimeline.state != PlayState.Playing ||
            string.Compare(objectBag, "叶", StringComparison.Ordinal) != 0)
            yield break;
        BagManager.Instance.UsedCount++;
        TimelineManager.Instance.leafTimeline.Stop();
        leafTrans.gameObject.SetActive(false);
        TimelineManager.Instance.TreeTimeline.Play();
        yield return new WaitForSecondsRealtime(1f);
        spr.color = new Color(255, 255, 255, 0);
        inter.index++;
        Shadow[0].SetActive(false);
        Shadow[1].SetActive(true);
        yield return null;
    }
}
