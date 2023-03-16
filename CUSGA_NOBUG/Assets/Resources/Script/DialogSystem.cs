using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void GaussianBlurEvent();
public class DialogSystem : MonoBehaviour
{
    private static DialogSystem instance;
    public static DialogSystem Instance => instance;

    [Header("UI���")]
    public TextMeshProUGUI textLabel;
    public Image faceImage;

    [Header("����ʾ���ʱ��")]
    public float textSpeed = 0.1f;

    private int index;
    private bool textFinish = false;

    [HideInInspector]
    public Sprite otherFace;
    [Header("���ͷ��")]
    public Sprite playerFace;

    private List<string> textList = new List<string>();
    Coroutine Co;
    bool DownF = false;

    public GaussianBlurEvent gaussianBlur;
    
    private void Awake()
    {
        instance = this;
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Face();
        Co = StartCoroutine(SetTextUI());
        gaussianBlur+=GameObject.FindWithTag("MainCamera").GetComponent<GaussianBlur>().gaussianFunction;
    }

    void Update()
    {
        //�Ի�û�����
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
        if(index == textList.Count) //�Ի�����
        {
            if (SceneManager.GetActiveScene().name == "Prelude")
            {
                gameManage.Instance.changeBG = true;
                gameManage.Instance.index++;
                gaussianBlur.Invoke();
            }
                
            this.gameObject.SetActive(false);

            return;
        }
    }

    public void GetTextFromFile(TextAsset file)
    {
        //�����
        textList.Clear();
        index = 0;

        //�����и�
        string[] lineData = file.text.Split('\n');

        foreach (string line in lineData)
        {
            //��ȡ�ļ�
            textList.Add(line);
        }
    }

    /// <summary>
    /// ��ȡͷ��
    /// </summary>
    void Face()
    {
        switch (textList[index].Trim().ToString())
        {
            case "A":
                faceImage.sprite = otherFace;
                index++;
                break;
            case "B":
                faceImage.sprite = playerFace;
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
