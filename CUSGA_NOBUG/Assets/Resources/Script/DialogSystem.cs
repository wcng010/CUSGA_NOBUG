using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void GaussianBlurEvent();

public enum ObjType
{
    bridge,
    door,
    father,
    other,
}

public class DialogSystem : MonoBehaviour
{
    private static DialogSystem instance;
    public static DialogSystem Instance => instance;

    [Header("UI界面")]
    public Text textLabel;
    public Image faceImage;
    public Text textName;

    [Header("聊天内容出现间隔时间")]
    public float textSpeed = 0.1f;

    private int index;
    private bool textFinish = false;

    [HideInInspector]
    public Sprite otherFace;
    [HideInInspector]
    public string otherName;
    [Header("玩家头像")]
    public Sprite playerFace;
    [Header("玩家姓名")]
    public string playerName;

    private List<string> textList = new List<string>();
    Coroutine Co;
    bool DownF = false;

    public GaussianBlurEvent gaussianBlur;

    [HideInInspector]
    public ObjType objT = ObjType.other;
    
    private void Awake()
    {
        instance = this;
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Face();
        Co = StartCoroutine(SetTextUI());

        if (SceneManager.GetActiveScene().name == "Prelude")
            gaussianBlur +=GameObject.FindWithTag("MainCamera").GetComponent<GaussianBlur>().gaussianFunction;
    }

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.F) || DownF) && index != textList.Count)
        {
            DownF = false;
            Face();

            if (textFinish)
                Co = StartCoroutine(SetTextUI());
            else
            {
                StopCoroutine(Co);
                textLabel.text = textList[index].ToString();
                index++;
                textFinish = true;
            }
        }
        if(index == textList.Count) //聊天结束
        {
            if (SceneManager.GetActiveScene().name == "Prelude")
            {
                gameManage.Instance.changeBG = true;
                gameManage.Instance.index++;
                gaussianBlur.Invoke();
            }

            switch (objT)
            {
                case ObjType.bridge:
                    Bridge.Instance.spr.enabled = true;
                    TimelineManager.Instance.bridgeTimeline.Play();
                    break;
                case ObjType.door:
                    Door.Instance.spr.enabled = true;
                    TimelineManager.Instance.doorTimeline.Play();
                    break;
                case ObjType.father:
                    if (Father.Instance.inter.index == 0)
                    {
                        Father.Instance.GetNeedObject();
                        Father.Instance.inter.index++;
                    }                    
                    break;
                case ObjType.other:
                    break;
                default:
                    break;
            }

            objT = ObjType.other;
            this.gameObject.SetActive(false);

            return;
        }
    }

    public void GetTextFromFile(TextAsset file)
    {
        //清空聊天内容
        textList.Clear();
        index = 0;

        //分割聊天内容
        string[] lineData = file.text.Split('\n');

        foreach (string line in lineData)
        {
            //加入聊天列表中
            textList.Add(line);
        }
    }

    /// <summary>
    /// 选择面
    /// </summary>
    void Face()
    {
        switch (textList[index].Trim().ToString())
        {
            case "A":
                faceImage.sprite = otherFace;
                textName.text = otherName;
                index++;
                break;
            case "B":
                faceImage.sprite = playerFace;
                textName.text = playerName;
                index++;
                break;
            default:
                break;
        }
    }

    IEnumerator SetTextUI()
    {
        textFinish = false;
        textLabel.text = "";
        for (int i = 0; i < textList[index].Length; i++)
        {
            textLabel.text += textList[index][i].ToString();
            yield return new WaitForSeconds(textSpeed);
        }
        textFinish = true;
        index++;
    }

    public void MouseDown_F()
    {
        DownF = true;
    }
}
