using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;


public enum ObjType_Inter
{
    bridge,
    door,
    other,
}

public class Interaction : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sprite;
    public GameObject chatFrame;
    [Header("该物品头像")]
    public Sprite thisFace;
    [Header("获取的物品名字")]
    public string ObjName;
    public bool IsObj;

    [Header("聊天内容")]
    public TextAsset[] textFile;

    [HideInInspector]
    public int index = 0;

    private List<ObjectData> dataList;

    public ObjType_Inter type_Inter = ObjType_Inter.other;

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
        //存在聊天系统
        if (sprite != null && !IsObj)
        {
            if (sprite.enabled && Input.GetKeyDown(KeyCode.F) && !chatFrame.activeInHierarchy)
            {
                if (index >= textFile.Length)
                    return;

                DialogSystem.Instance.otherFace = thisFace;

                switch (type_Inter)
                {
                    case ObjType_Inter.bridge:
                        DialogSystem.Instance.objT = ObjType.bridge;
                        break;
                    case ObjType_Inter.door:
                        DialogSystem.Instance.objT = ObjType.door;
                        break;
                    case ObjType_Inter.other:
                        DialogSystem.Instance.objT = ObjType.other;
                        break;
                    default:
                        break;
                }

                DialogSystem.Instance.GetTextFromFile(textFile[index]);
                chatFrame.SetActive(true);
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
