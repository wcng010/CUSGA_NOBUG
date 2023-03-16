using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;
using TMPro;
using UnityEngine.UI;
public class Object_UI : MonoBehaviour
{
    [NonSerialized]
    public int Object_ID;
    [NonSerialized]
    public string ObjectName_item;
    [NonSerialized] 
    public string Brush_composition;
    [NonSerialized]
    public bool IsActive;
    [Foldout("NeedDrag",true)]
    public Image object_plaid;
    public GameObject iteminPlaid;
    public Text objectNum;

    public void InitObject(ObjectData objectData,int Num)
    {
        //this.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 1);
        if (objectData == null||objectData.ObjectNum<=0)
        {
            IsActive = false;
            iteminPlaid.SetActive(false);
            return;
        }
        IsActive = true;
        Brush_composition = objectData.Brush_composition;
        ObjectName_item = objectData.ObjectNames;
        object_plaid.sprite =objectData.ObjectUI_Bag;
        if (Num >= BagManager.Instance.boundary_Inventory)
            return;
        objectNum.text = objectData.ObjectNum.ToString();
    }
}
