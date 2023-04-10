using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;


public enum ObjType_Inter
{
    bridge,
    door,
    father,
    other,
    leaf,
    river,
    mother_3,
}

public class Interaction : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sprite;
    public GameObject chatFrame;
    [Header("该物品头像")]
    public Sprite thisFace;
    [Header("聊天名字")]
    public string thisName;
    [Header("获取的物品字")]
    public string ObjName;
    public bool IsObj;

    [Header("聊天内容")]
    public TextAsset[] textFile;

    [HideInInspector]
    public int index = 0;

    private List<ObjectData> dataList;

    public ObjType_Inter type_Inter = ObjType_Inter.other;

   // private Transform player;
   // private bool getObjbool;
    void Start()
    {
        index = 0;
        sprite = GetComponent<SpriteRenderer>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void Update()
    {
        InteractionObj();

        //if (getObjbool)
        //{
        //    StartCoroutine(GetObj());
        //    getObjbool = false;
        //}
            
    }

    void InteractionObj()
    {
        if (IsObj && ObjName != null)
        {
            if (sprite.enabled && Input.GetKeyDown(KeyCode.F))
            {
                dataList = BagManager.Instance.dataListClass.objectList;
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i] != null && dataList[i].ObjectNames == ObjName.ToString())
                    {
                        dataList[i].ObjectNum++;
                        if (transform.parent != null)
                        {
                            Destroy(transform.parent.gameObject);
                            Instantiate(Resources.Load<GameObject>("Prefab/GetObjEff"),transform.position,Quaternion.identity).GetComponent<SpriteRenderer>().sprite = thisFace;
                        }
                           
                        break;
                    }

                }
            }
        }
    }

    //IEnumerator GetObj()
    //{
    //    transform.parent.localScale -= Vector3.one;
    //    yield return null;
    //}
    

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
                DialogSystem.Instance.otherName = thisName;

                switch (type_Inter)
                {
                    case ObjType_Inter.bridge:
                        DialogSystem.Instance.objT = ObjType.bridge;
                        break;
                    case ObjType_Inter.door:
                        DialogSystem.Instance.objT = ObjType.door;
                        break;
                    case ObjType_Inter.father:
                        DialogSystem.Instance.objT = ObjType.father;
                        break;
                    case ObjType_Inter.leaf:
                        DialogSystem.Instance.objT = ObjType.leaf;
                        break;
                    case ObjType_Inter.river:
                        DialogSystem.Instance.objT = ObjType.river;
                        break;
                    case ObjType_Inter.mother_3:
                        DialogSystem.Instance.objT = ObjType.mother_3;
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
