using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rce_File.Inner_C_Script.EventCenter;

public class Level2_Finish : MonoBehaviour
{
    private static Level2_Finish instance;
    public static Level2_Finish Instance => instance;

    public GameObject chatFrame;
    [Header("对话数据文本")]
    public TextAsset[] textFile;
    [Header("其他人头像")]
    public Sprite[] otherFace;
    [Header("其他人名字")]
    public string[] otherName;

    public GameObject End;

    [HideInInspector]
    public int ChatIndex = 0;
    [HideInInspector]
    public bool ChatStatu = false;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
    void Start()
    {
        StartCoroutine(levelEnd());
    }

    void Update()
    {
        if(ChatStatu)
        {
            StartCoroutine(levelExit());
            ChatStatu = false;
        }
    }

    IEnumerator levelEnd()
    {
        EventCenter.Publish(MyEventType.Level2End);
        yield return new WaitForSeconds(5.5f);
        DialogSystem.Instance.GetTextFromFile(textFile[ChatIndex]);
        chatFrame.SetActive(true);
    }

    IEnumerator levelExit()
    {
        TimelineManager.Instance.Level2Exit2.Play();
        yield return new WaitForSeconds(3.5f);
        End.SetActive(true);
    }
}
