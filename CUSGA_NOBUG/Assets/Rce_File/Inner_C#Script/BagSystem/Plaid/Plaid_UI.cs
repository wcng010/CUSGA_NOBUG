using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Pixeye.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Plaid_UI : MonoBehaviour
{
    
    [NonSerialized]
    public int plaid_ID;
    [NonSerialized]
    public string brushName_item;
    [NonSerialized]
    public bool IsActive;
    [Foldout("NeedDrag",true)]
    public Image brush_plaid;
    public GameObject iteminPlaid;
    public Text brushNum;
    
    public void InitPlaid(BrushData brushData)
    {
        if (plaid_ID > BagManager.Instance.boundary_exchange)
        {
            this.GetComponent<Image>().color = new Color(255, 0, 202, 255);
        }
        //this.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 1);
        if (brushData == null||brushData._brushNum<=0)
        {
            IsActive = false;
            iteminPlaid.SetActive(false);
            return;
        }
        IsActive = true;
        brushName_item = brushData._brushName;
        brush_plaid.sprite = brushData._brushSprite;
        if(plaid_ID<=BagManager.Instance.boundary_workbag)
        brushNum.text = brushData._brushNum.ToString();
    }
    
}
