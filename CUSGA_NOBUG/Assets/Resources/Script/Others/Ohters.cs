using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;
using UnityEngine.Playables;

public class Ohters<T> : MonoBehaviour where T : class
{
    private static T instance;
    public static T Instance => instance;

    public string[] needStrings;

    public GameObject[] useGameObj;
    public Sprite[] useSpriteObj;

    private Collider2D coll;
    [HideInInspector]
    public SpriteRenderer spr;
    [HideInInspector]
    public Interaction inter;

    protected int succeed;
    protected bool close = false;
    protected bool status = true;
    protected List<ObjectData> dataList;

    protected virtual void Awake()
    {
        instance = this as T;
    }

    public virtual void Start()
    {
        succeed = 0;
        inter = GetComponentInChildren<Interaction>();
        if(transform.parent != null)
            coll = transform.parent.GetComponent<Collider2D>();
        else
            coll = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
    }
    public virtual void FindneedObject()
    {
        if (inter.sprite.enabled && !close && !inter.chatFrame.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                dataList = BagManager.Instance.dataListClass.objectList;
                for (int i = 0; i < needStrings.Length; i++)
                {
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        if (dataList[j] != null && dataList[j].ObjectNames == needStrings[i].ToString())
                        {
                            if (dataList[j].ObjectNum > 0)
                            {
                                succeed++;                             
                                break;
                            }
                        }

                    }

                }
            }

            if (succeed >= needStrings.Length)
            {
                for (int i = 0; i < needStrings.Length; i++)
                {
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        if (dataList[j] != null && dataList[j].ObjectNames == needStrings[i].ToString())
                        {
                            if (dataList[j].ObjectNum > 0)
                            {
                                dataList[j].ObjectNum--;
                                break;
                            }
                        }

                    }

                }

                inter.index++;
                close = true;
            }

        }

        
    }

    public virtual void ShowObject()
    {
        coll.isTrigger = true;
        spr.enabled = true;
        inter.index++;
    }
    
    protected virtual void useObject(string objectBag, string objectName)
    {
        coll.isTrigger = true;
        spr.color = new Color(255, 255, 255, 1f);
        transform.localScale = new Vector3(1, 1, 1);
        inter.index++;
        BagManager.Instance.UsedCount++;
    }

    public IEnumerator UseObj()
    {
        for(int i = 0;i< needStrings.Length;i++)
        {
            useGameObj[i].SetActive(true);
            useGameObj[i].GetComponent<SpriteRenderer>().sprite = useSpriteObj[i];
            yield return new WaitForSeconds(0.5f);
        }
    }
}
