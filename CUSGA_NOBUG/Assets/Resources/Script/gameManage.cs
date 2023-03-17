using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class gameManage : MonoBehaviour
{
    private static gameManage instance;
    public static gameManage Instance => instance;

    public GameObject chatFrame;
    [Header("对话数据文本")]
    public TextAsset[] textFile;
    [Header("其他人头像")]
    public Sprite[] otherFace;
    [Header("背景图片")]
    public GameObject[] background;

    public GameObject End;

    [HideInInspector]
    public int index = 0;
    [HideInInspector]
    public bool changeBG;

    bool status = false;

    private void Awake()
    {
        instance = this;
    }

     void Update()
    {     
        if (index >= textFile.Length)
        {
            End.SetActive(true);
            return;
        }

        if(GaussianBlur.Instance.GaussionTimeline.state == PlayState.Playing && !status)
            status = true;

        if(GaussianBlur.Instance.GaussionTimeline.state == PlayState.Paused && status)
        {
            status = false;
            showChatFrame();
        }

        if(GaussianBlur.Instance.iterations > 3.5f)
        {
            Change_BK();
        }
    }

    public void Change_BK()
    {
        if (!background[index].activeInHierarchy)
        {
            background[index - 1].SetActive(false);
            background[index].SetActive(true);
        }        
    }

    public void showChatFrame()
    {
        changeBG = false;
        DialogSystem.Instance.otherFace = otherFace[index];
        DialogSystem.Instance.GetTextFromFile(textFile[index]);
        chatFrame.SetActive(true);
    }
}
