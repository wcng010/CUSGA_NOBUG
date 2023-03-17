using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using TMPro;
using UnityEngine.UI;
using Object = System.Object;

public class Object_UI : Base_UI<ObjectData>
{
    
    [NonSerialized] 
    public string Brush_composition;
    
    public void InitObject(ObjectData objectData,int Num)
    {
        ID = Num;
        if (objectData == null||objectData.ObjectNum<=0)//物品为空或数量为0，笔画框标记失活
        {
            IsActive = false;
            iteminPlaid.SetActive(false);
            return;
        }
        IsActive = true;
        Brush_composition = objectData.Brush_composition;
        Name_item = objectData.ObjectNames;
        plaid.sprite =objectData.ObjectUI_Bag;
        if (Num >= BagManager.Instance.boundaryInventory)
            return;
        NumText.text = objectData.ObjectNum.ToString();
    }
}
