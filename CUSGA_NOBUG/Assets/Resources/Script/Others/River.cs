using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class River : Ohters<River>
{
    public Sprite Flsh;
    [Header("桶Transform")] [SerializeField]
    private Transform bucket;
    [Header("竿Transform")] [SerializeField] 
    private Transform fishingRod;

    public override void Start()
    {
        base.Start();
        BagManager.Instance.UseObject += UseBucket;
        BagManager.Instance.UseObject += UseFishingRod;
    }

    public void OnDisable()
    {
        BagManager.Instance.UseObject -= UseBucket;
        BagManager.Instance.UseObject -= UseFishingRod;
    }

    void Update()
    {
        //FindneedObject();

        inter.InteractionChat();
    }

    private void UseBucket(string objectBag, string objectName)
    {
        if (string.Compare(objectName, "Barrel", StringComparison.Ordinal) != 0
            || TimelineManager.Instance.bucketTimeline.state != PlayState.Playing ||
            string.Compare(objectBag, "桶", StringComparison.Ordinal) != 0) return;
        TimelineManager.Instance.bucketTimeline.Stop();
        bucket.gameObject.SetActive(false);//水桶失活，要做水桶进入背包就不用失活
        inter.index++;
        BagManager.Instance.UsedCount++;
    }

    private void UseFishingRod(string objectBag, string objectName)
    {
        if (string.Compare(objectName, "FishingRod", StringComparison.Ordinal) != 0
            || TimelineManager.Instance.fishingRodTimeline.state != PlayState.Playing ||
            string.Compare(objectBag, "竿", StringComparison.Ordinal) != 0) return;
        Debug.Log(1);
        TimelineManager.Instance.fishingRodTimeline.Stop();
        fishingRod.gameObject.SetActive(false);//水桶失活，要做水桶进入背包就不用失活
        inter.index++;
        BagManager.Instance.UsedCount++;
    }
}
