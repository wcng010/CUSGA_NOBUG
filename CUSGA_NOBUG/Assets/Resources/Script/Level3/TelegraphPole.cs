using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class TelegraphPole : Ohters<TelegraphPole>
{
    [SerializeField]
    private Sprite fullPoleSpr;

    public GameObject shadow;
    public GameObject shadow1;
    private void OnEnable()
    {
        BagManager.Instance.UseObject += useObject;
    }

    private void OnDisable()
    {
        BagManager.Instance.UseObject -= useObject;
    }

    public override void Start()
    {
        base.Start();

        if(PlayerPrefs.GetString("TelegraphPole") == "true")
        {
            TimelineManager.Instance.PoleTimeline.Play();
        }

        if(PlayerPrefs.GetInt("TelegraphPole", 0) == 1)
        {
            inter.index = 1;
            spr.sprite = fullPoleSpr;
            spr.color = new Color(255, 255, 255, 1f);
            transform.localScale = new Vector3(1, 1, 1);
            shadow.SetActive(false);
            shadow1.SetActive(true);
            TimelineManager.Instance.PoleTimeline.Stop();
        }
            
    }

    void Update()
    {
        inter.InteractionChat();
    }

    protected override void useObject(string objectBag ,string objectName)
    {
        if (string.Compare(objectName, gameObject.name, StringComparison.Ordinal) != 0
            || TimelineManager.Instance.PoleTimeline.state != PlayState.Playing ||
            string.Compare(objectBag, "杆", StringComparison.Ordinal) != 0) return;
        Debug.Log(1);
        spr.sprite = fullPoleSpr;
        spr.color = new Color(255, 255, 255, 1f);
        transform.localScale = new Vector3(1, 1, 1);
        inter.index++;
        PlayerPrefs.SetInt("TelegraphPole", 1);
        shadow.SetActive(false);
        shadow1.SetActive(true);
        Drugstore.Instance.inter.index++;
        PlayerPrefs.SetInt("Drugstore", 1);
        BagManager.Instance.UsedCount++;
        TimelineManager.Instance.PoleTimeline.Stop();
    }

}
