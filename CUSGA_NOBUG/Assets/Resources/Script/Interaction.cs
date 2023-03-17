using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sprite;
    public GameObject chatFrame;
    [Header("ͷ��")]
    public Sprite thisFace;
    [Header("��ȡ����")]
    public string ObjName;
    public bool IsObj;

    [Header("�Ի������ı�")]
    public TextAsset[] textFile;

    [HideInInspector]
    public int index = 0;

    private List<ObjectData> dataList;
    
    void Start()
    {
        index = 0;
        sprite = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        InteractionObj();
    }

    void InteractionObj()
    {
        if (IsObj && ObjName != null)
        {
            if (sprite.enabled && Input.GetKeyDown(KeyCode.F))
            {
                dataList = BagManager.Instance.dataListClass.ObjectList;
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i] != null && dataList[i].ObjectNames == ObjName.ToString())
                    {
                        dataList[i].ObjectNum++;
                        if (transform.parent != null)
                            Destroy(transform.parent.gameObject);
                        break;
                    }

                }
            }
        }
    }

    public void InteractionChat()
    {
        //�����ｻ��
        if (sprite != null && !IsObj)
        {
            //û�м��������£�ͬʱ��ҽӴ���R
            if (sprite.enabled && Input.GetKeyDown(KeyCode.F) && !chatFrame.activeInHierarchy)
            {
                if (index >= textFile.Length)
                    return;

                DialogSystem.Instance.otherFace = thisFace;

                DialogSystem.Instance.GetTextFromFile(textFile[index]);//��ȡ�ļ�����
                chatFrame.SetActive(true);//��ʾ�Ի���
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsObj)
        {
            if (collision.tag == "Player")
            {
                sprite.enabled = true;
            }

        }
        else if (collision.tag == "Player" && index < textFile.Length)
        {
            sprite.enabled = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            sprite.enabled = false;
        }
    }
}
