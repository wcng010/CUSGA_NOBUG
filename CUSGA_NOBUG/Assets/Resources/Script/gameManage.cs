using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManage : MonoBehaviour
{
    private static gameManage instance;
    public static gameManage Instance => instance;

    public GameObject chatFrame;
    [Header("对话数据文本")]
    public TextAsset[] textFile;
    [Header("其他人头像")]
    public Sprite[] otherFace;

    [HideInInspector]
    public int index = 0;
    [HideInInspector]
    public bool changeBG;

    private void Awake()
    {
        instance = this;
    }

     private void Update()
    {
        if (index >= textFile.Length)
            SceneManager.LoadScene("Level1");
    }

    //public void Change_BK()
    //{
    //    background[index-1].SetActive(false);
    //    background[index].SetActive(true);
    //}

    public void showChatFrame()
    {
        changeBG = false;
        DialogSystem.Instance.otherFace = otherFace[index];
        DialogSystem.Instance.GetTextFromFile(textFile[index]);
        chatFrame.SetActive(true);
    }
}
