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
    leaf,
    river,
    mother_3,
    student,
    drugstore,
    telegraphPole,
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

    public SpriteRenderer[] level2UseObj;
    private void Awake()
    {
        objT = ObjType.other;
        instance = this;
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (faceImage.color == new Color(1, 1, 1, 0))
            faceImage.color = new Color(1, 1, 1, 1);

        Face();
        Co = StartCoroutine(SetTextUI());

        if (SceneManager.GetActiveScene().name == "Prelude" || SceneManager.GetActiveScene().name == "EndingScene")
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
            if (SceneManager.GetActiveScene().name == "EndingScene")
            {
                gameManage.Instance.changeBG = true;
                gameManage.Instance.index++;
                gaussianBlur.Invoke();
            }
            if (SceneManager.GetActiveScene().name == "Level2")
            {
                if(Level2_Finish.Instance.gameObject.activeInHierarchy == true)
                {
                    Level2_Finish.Instance.ChatStatu = true;
                }
            }

            switch (objT)
            {
                case ObjType.bridge:
                    Bridge.Instance.spr.enabled = true;
                    TimelineManager.Instance.bridgeTimeline.Play();
                    break;
                case ObjType.telegraphPole:
                    TimelineManager.Instance.PoleTimeline.Play();
                    break;
                case ObjType.door:
                    Door.Instance.spr.enabled = true;
                    TimelineManager.Instance.doorTimeline.Play();
                    break;
                case ObjType.leaf:
                    level2UseObj[2].enabled = true;
                    TimelineManager.Instance.leafTimeline.Play();
                    break;
                case ObjType.river:
                    if(River.Instance.inter.index == 0)
                    {
                        level2UseObj[0].enabled = true;
                        TimelineManager.Instance.fishingRodTimeline.Play();
                    }
                    else if(River.Instance.inter.index == 1)
                    {
                        level2UseObj[1].enabled = true;
                        TimelineManager.Instance.bucketTimeline.Play();
                    }                   
                    break;
                case ObjType.father:
                    if (Father.Instance.inter.index == 1)
                    {
                        Father.Instance.GetNeedObject(Father.Instance.transform.position);
                        Father.Instance.inter.index++;
                    }                    
                    break;
                case ObjType.student:
                    if (Student.Instance.inter.index == 0)
                    {
                        Student.Instance.GetNeedObject(Student.Instance.transform.position);
                        Student.Instance.inter.index++;
                    }
                    break;
                case ObjType.drugstore:
                    if (Drugstore.Instance.inter.index == 2)
                    {
                        Drugstore.Instance.GetNeedObject(Drugstore.Instance.transform.position);
                        Drugstore.Instance.inter.index++;
                    }
                    break;
                case ObjType.mother_3:
                    if (Mother_3.Instance.inter.index == 1)
                    {
                        //跳场景动画
                        //跳场景
                        SceneManager.LoadScene("EndingScene");
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
                if(faceImage != null)
                    faceImage.sprite = otherFace;
                if(faceImage != null)
                    textName.text = otherName;
                index++;
                break;
            case "B":
                if (faceImage != null)
                    faceImage.sprite = playerFace;
                if (faceImage != null)
                    textName.text = playerName;
                index++;
                break;
            case "N":
                faceImage.sprite = playerFace;
                faceImage.color = new Color(1, 1, 1, 0);
                textName.text = "";
                index++;
                break;
            case "2F":
                faceImage.sprite = Level2_Finish.Instance.otherFace[0];
                textName.text = Level2_Finish.Instance.otherName[0];
                index++;
                break;
            case "2M":
                faceImage.sprite = Level2_Finish.Instance.otherFace[1];
                textName.text = Level2_Finish.Instance.otherName[1];
                index++;
                break;
            case "2S":
                faceImage.sprite = Level2_Finish.Instance.otherFace[2];
                textName.text = Level2_Finish.Instance.otherName[2];
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
