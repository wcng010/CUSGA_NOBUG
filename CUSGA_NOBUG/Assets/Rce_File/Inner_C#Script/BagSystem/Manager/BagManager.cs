using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public delegate void UseObject();
public class BagManager : BaseManager<BagManager>
{
    [Header("信息Data数组")]
    public ListData DataListClass;
    [Header("背包格子Grid")]
    public GameObject plaidGrid;
    [Header("合成台格子Grid")]
    public GameObject WorkGrid;
    [Header("交换台格子Grid")]
    public GameObject exchangeGrid;
    [Header("物品栏格子Grid")] 
    public GameObject InventoryGrid;
    [Header("格子Brush预设体")]
    public GameObject plaid;
    [Header("格子笔画数组")]
    public List<GameObject> brushList=new List<GameObject>();
    [Header("格子Object预设体")] 
    public GameObject plaid_Object;
    [Header("格子道具数组")]
    public List<GameObject> ObjectList = new List<GameObject>();
    [Header("防遮罩坐标")] 
    public Transform EndTransform;
    [Header("背包笔画合成台分界线")] 
    public int boundary_workbag;
    [Header("背包兑换台分界线")] 
    public int boundary_exchange;
    [Header("背包物品栏分界线")] 
    public int boundary_Inventory;
    [Header("合成检测数组")] 
    private string[] CheckStrings = new string[20];
    [Header("合成检测String")]
    private String CheckString;
    public UseObject useObject;
    [NonSerialized]
    public int UsedCount=0;
    [Header("开始时需要添加的笔画")] 
    public string[]BeginBrushs;
    public void RefreshBrush()
    {
       BagClear();
       brushList.Clear();
       for (int i = 0; i<DataListClass.BrushList.Count; i++)
        {
            brushList.Add(Instantiate(plaid));
            brushList[i].GetComponent<Plaid_UI>().plaid_ID = i;
            brushList[i].GetComponent<Plaid_UI>().InitPlaid(DataListClass.BrushList[i]);
            if (i <= boundary_workbag)
            {
                brushList[i].transform.SetParent(plaidGrid.transform);
            }
            else if(i<=boundary_exchange)
            {
                brushList[i].transform.SetParent(WorkGrid.transform);
            }
            else
            {
                brushList[i].transform.SetParent(exchangeGrid.transform);
            }
        }
    }
    

    public void RefreshObject()
    {
        BagClear();
        ObjectList.Clear();
        for (int i = 0; i<DataListClass.ObjectList.Count; i++)
        {
            
            ObjectList.Add(Instantiate(plaid_Object));
            ObjectList[i].GetComponent<Object_UI>().Object_ID = i;
            ObjectList[i].GetComponent<Object_UI>().InitObject(DataListClass.ObjectList[i],i);
            if(i<boundary_Inventory)
            {
                ObjectList[i].transform.SetParent(plaidGrid.transform);
            }
            else if (i == boundary_Inventory)
            {
                ObjectList[i].transform.SetParent(WorkGrid.transform);
            }
            else if (i > boundary_Inventory)
            {
                ObjectList[i].transform.SetParent(InventoryGrid.transform);
            }
        }
    }
    
    public void BagClear()
    {
        for (int i = 0; i < WorkGrid.transform.childCount; i++)
        {
            Destroy(WorkGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < plaidGrid.transform.childCount; i++) //遍历所有格子
        {
            Destroy(plaidGrid.transform.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < exchangeGrid.transform.childCount; i++) //遍历所有格子
        {
            Destroy(exchangeGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < InventoryGrid.transform.childCount; i++)
        {
            Destroy(InventoryGrid.transform.GetChild(i).gameObject);
        }

        Debug.Log("背包清空");
    }

    public void ExitClear()
    {
        BagClear();
        brushList.Clear();
        ObjectList.Clear();
    }
    /// <summary>
    /// 合成函数
    /// 遍历所有data道具，使得匹配道具数量加1.
    /// 存在问题：一开始没有某个道具。
    /// </summary>
    public void Synthesis_Brush()
    {
        for (int i = boundary_workbag+1,j=0; i < boundary_exchange+1&&brushList[i].GetComponent<Plaid_UI>().IsActive; i++,j++)
        {
            CheckStrings[j] = brushList[i].GetComponent<Plaid_UI>().brushName_item;
        }

        CheckString = String.Concat(CheckStrings);
        foreach (var item in DataListClass.ObjectList)
        {
            if (item != null && item.Brush_composition == CheckString)
            {
                item.ObjectNum++;
                CorrectionFor_01B(boundary_workbag+1,boundary_exchange+1);
                RefreshBrush();
                CheckStrings = new string[20];
                return;
            }
        }
        Debug.Log("合成失败");
    }
    /// <summary>
    /// 分解函数
    /// 遍历所有data笔画让每个笔画数加1
    /// </summary>
    public void Decompose()
    {
        if (ObjectList[boundary_Inventory].GetComponent<Object_UI>().IsActive)
        {
            for (int i = 0;i<DataListClass.ObjectList[boundary_Inventory].Decomposition.Length; i++)
            {
                for (int j = 0; j < DataListClass.BrushList.Count; j++)
                {
                    if (DataListClass.BrushList[j]!=null&&DataListClass.ObjectList[boundary_Inventory].Decomposition[i] ==
                        DataListClass.BrushList[j]._brushName)
                    {
                        Debug.Log(DataListClass.BrushList[j]._brushName+"数量加一");
                        DataListClass.BrushList[j]._brushNum++;
                        break;
                    }
                }
            }
            
            if (DataListClass.ObjectList[boundary_Inventory].ObjectNum == 1&&!CorrectionFor_12O(DataListClass.ObjectList[boundary_Inventory].ObjectNames))
            {
                for (int i = 0; i <boundary_workbag ; i++)
                {
                    if (DataListClass.ObjectList[i] == null)
                    {
                        DataListClass.ObjectList[i] = DataListClass.ObjectList[boundary_Inventory];
                        DataListClass.ObjectList[i].ObjectNum--;
                        break;
                    }
                }
            }
            DataListClass.ObjectList[boundary_Inventory] = null;
            RefreshObject();
        }
        else//分解失败
        {
            Debug.Log("分解失败");
        }
    }
    
    /// <summary>
    /// 将两个笔画交换为一个笔画
    /// </summary>
    public void Exchange()
    {
        if (brushList[boundary_exchange + 1].GetComponent<Plaid_UI>().IsActive == true &&
            brushList[boundary_exchange + 2].GetComponent<Plaid_UI>().IsActive == true &&
            brushList[boundary_exchange + 3].GetComponent<Plaid_UI>().IsActive == true)
        {
            DataListClass.BrushList[boundary_exchange+3]._brushNum++;
            CorrectionFor_01B(boundary_exchange+1,boundary_exchange+3);
            RefreshBrush();
        }
        else
        {
            Debug.Log("兑换失败");
        }
        
    }
    
    /// <summary>
    /// 数量0，1的处理
    /// </summary>
    /// <param name="FirstIndex"></param>
    /// <param name="EndIndex"></param>
    public void CorrectionFor_01B(int FirstIndex,int EndIndex)
    {
        for (int i = FirstIndex; i < EndIndex&&brushList[i].GetComponent<Plaid_UI>().IsActive; i++)
        {
            if (DataListClass.BrushList[i]._brushNum == 1&&!CorrectionFor_12B(DataListClass.BrushList[i]._brushName))
            {
                for (int j = 0; j <= boundary_workbag; j++)
                {
                    if (DataListClass.BrushList[j] == null)
                    {
                        DataListClass.BrushList[j] = DataListClass.BrushList[i];
                        break;
                    }
                }
                DataListClass.BrushList[i]._brushNum--;
            }
            DataListClass.BrushList[i] = null;
        }
        RefreshBrush();
    }

    public void CorrectionFor_01O(int ClickItemID)
    {
        if (DataListClass.ObjectList[ClickItemID].ObjectNum == 1&&!CorrectionFor_12O(DataListClass.ObjectList[ClickItemID].ObjectNames))
        {
            for(int i=0;i<boundary_workbag;i++)
                if (DataListClass.ObjectList[i] == null)
                {
                    DataListClass.ObjectList[i] = DataListClass.ObjectList[ClickItemID];
                    DataListClass.ObjectList[ClickItemID] = null;
                    RefreshObject();
                    return;
                }
        }
    }


    /// <summary>
    /// true为数量为2的情况，false为数量为1的情况
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CorrectionFor_12B(string name)
    {
        for (int i = 0; i <= boundary_workbag; i++)
        {
            if (DataListClass.BrushList[i]!=null&&name == DataListClass.BrushList[i]._brushName)
            {
                return true;
            }
        }
        return false;
    }


    public bool CorrectionFor_12O(string name)
    {
        for (int i = 0; i < boundary_Inventory; i++)
        {
            if (DataListClass.ObjectList[i] != null && name == DataListClass.ObjectList[i].ObjectNames)
            {
                return true;
            }
        }

        return false;
    }

    public void DeleteSameObject(int MyID,int FirstIndex,int EndIndex)
    {
        for (int i = FirstIndex; i < EndIndex; i++)
        {
            if (DataListClass.ObjectList[i] != null &&
                DataListClass.ObjectList[i].ObjectNames == DataListClass.ObjectList[MyID].ObjectNames&&i!=MyID)
            {
                DataListClass.ObjectList[i].ObjectNum++;
                DataListClass.ObjectList[i] = null;
            }
        }
    }

    public void ChangeCorrection()
    {
        if (DataListClass.BrushList[boundary_exchange+1]!=null&&
            DataListClass.BrushList[boundary_exchange+3]!=null&&
            (DataListClass.BrushList[boundary_exchange + 1]._brushName ==
             DataListClass.BrushList[boundary_exchange + 3]._brushName))
        {
            DataListClass.BrushList[boundary_exchange + 3]._brushNum++;
            DataListClass.BrushList[boundary_exchange + 3] = null;
        }

        if (DataListClass.BrushList[boundary_exchange + 2] != null &&
            DataListClass.BrushList[boundary_exchange + 3] != null &&
            (DataListClass.BrushList[boundary_exchange + 2]._brushName ==
             DataListClass.BrushList[boundary_exchange + 3]._brushName))
        {
            DataListClass.BrushList[boundary_exchange + 3]._brushNum++;
            DataListClass.BrushList[boundary_exchange + 3] = null;
        }
    }

    public void EnterScenes()
    {
        foreach (var brush in DataListClass.BrushList)
        {
            if (brush != null)
                brush._brushNum = 0;
        }

        foreach (var item in DataListClass.ObjectList)
        {
            if (item != null)
                item.ObjectNum = 0;
        }
        AddBrushOnBegin();
        EnterScenesAndClearBrush();
        EnterScenesAndClearObject();
    }

    private void AddBrushOnBegin()
    {
        for(int i=0;i<BeginBrushs.Length;i++)
        for (int j = 0; j < DataListClass.BrushList.Count; j++)
        {
            if (BeginBrushs[i] == null) return;
            if (DataListClass.BrushList[j] != null &&
                DataListClass.BrushList[j]._brushName.CompareTo(BeginBrushs[i]) == 0)
            {
                DataListClass.BrushList[j]._brushNum++;
            }
        }
    }

    /// <summary>
    /// 对所有笔画格子进行排序，
    /// 将空格子移到后方
    /// 再将笔画为0的格子移到后方
    /// </summary>
    public void EnterScenesAndClearBrush()
    {
        for (int i = 0; i < DataListClass.BrushList.Count; i++)
        {
            if (DataListClass.BrushList[i] == null)
            {
                if (!FindOtherBrush(i, DataListClass.BrushList.Count))
                {
                    break;
                }
            }
        }
        
        for (int i = 0; i < DataListClass.BrushList.Count; i++)
        {
            if (DataListClass.BrushList[i]!=null&&DataListClass.BrushList[i]._brushNum==0)
            {
                if (!FindFullBrush(i, DataListClass.BrushList.Count))
                {
                    break;
                }
            }
        }

    }

    private bool FindOtherBrush(int FirstIndex,int EndIndex)
    {
        for (int i = FirstIndex+1; i < EndIndex; i++)
        {
            if (DataListClass.BrushList[i] != null)
            {
                DataListClass.BrushList[FirstIndex] = DataListClass.BrushList[i];
                DataListClass.BrushList[i] = null;
                return true;
            }
        }

        return false;
    }
    private bool FindFullBrush(int FirstIndex,int EndIndex)
    {
        for (int i = FirstIndex+1; i < EndIndex; i++)
        {
            if (DataListClass.BrushList[i]!=null&&DataListClass.BrushList[i]._brushNum>0)
            {
                (DataListClass.BrushList[FirstIndex], DataListClass.BrushList[i]) = (DataListClass.BrushList[i], DataListClass.BrushList[FirstIndex]);
                return true;
            }
        }

        return false;
    }

    ///
    ///  
    public void DeleteSameBrush()
    {
        for (int i = 0; i <= boundary_workbag; i++)
        {
            if(DataListClass.BrushList[i]==null)
                continue;
            for (int j = 0; j <= boundary_workbag; j++)
            {
                if (DataListClass.BrushList[j] == null)
                    continue;
                if (DataListClass.BrushList[i]!=null&&DataListClass.BrushList[j]!=null&&
                    DataListClass.BrushList[i]._brushName.CompareTo(DataListClass.BrushList[j]._brushName) == 0
                    &&i!=j)
                {
                    DataListClass.BrushList[j] = null;
                }
            }
        }
    }




    /// <summary>
    /// 
    /// </summary>
    private void EnterScenesAndClearObject()
    {
        for (int i = 0; i <DataListClass.ObjectList.Count ; i++)
        {
            if (DataListClass.ObjectList[i] == null)
            {
                if (!FindOtherObject(i, DataListClass.ObjectList.Count-1))
                {
                    break;
                }
            }
        }
        
        for (int i = 0; i <DataListClass.ObjectList.Count; i++)
        {
            if (DataListClass.ObjectList[i]!=null&&DataListClass.ObjectList[i].ObjectNum==0)
            {
                if (!FindFullObject(i, DataListClass.ObjectList.Count-1))
                {
                    break;
                }
            }
        }
    }

    public void ClickBagAndClearObject()
    {
        for (int i = 0; i <=boundary_Inventory ; i++)
        {
            if (DataListClass.ObjectList[i] == null)
            {
                if (!FindOtherObject(i, boundary_Inventory))
                {
                    break;
                }
            }
        }
        
        for (int i = 0; i <=boundary_Inventory; i++)
        {
            if (DataListClass.ObjectList[i]!=null&&DataListClass.ObjectList[i].ObjectNum==0)
            {
                if (!FindFullObject(i, boundary_Inventory))
                {
                    break;
                }
            }
        }

    }
    

    private bool FindOtherObject(int FirstIndex,int EndIndex)
    {
        for (int i = FirstIndex+1; i <=EndIndex; i++)
        {
            if (DataListClass.ObjectList[i] != null)
            {
                DataListClass.ObjectList[FirstIndex] = DataListClass.ObjectList[i];
                DataListClass.ObjectList[i] = null;
                return true;
            }
        }

        return false;
    }
    private bool FindFullObject(int FirstIndex,int EndIndex)
    {
        for (int i = FirstIndex+1; i <=EndIndex; i++)
        {
            if (DataListClass.ObjectList[i]!=null&&DataListClass.ObjectList[i].ObjectNum>0)
            {
                (DataListClass.ObjectList[FirstIndex], DataListClass.ObjectList[i]) = (DataListClass.ObjectList[i], DataListClass.ObjectList[FirstIndex]);
                return true;
            }
        }

        return false;
    }
    
    
    private void Start()
    {   RefreshBrush();
        RefreshObject();
        EnterScenes();
        RefreshObject();
    }
}
