using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Pixeye.Unity;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class  Plaid_UI : Base_UI
{
    public void InitPlaid(BrushData brushData,int Num)
    {
        ID = Num;
        if (brushData == null||brushData._brushNum<=0)//笔画为空或数量为0，笔画框标记失活
        {
            IsActive = false;
            iteminPlaid.SetActive(false);
            return;
        }
        IsActive = true;
        Name_item = brushData._brushName;
        plaid.sprite = brushData._brushSprite;
        if(ID<=BagManager.Instance.boundaryWorkbag)
        NumText.text = brushData._brushNum.ToString();
    }
}

