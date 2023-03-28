using System;
using System.Collections.Generic;
using Pixeye.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Rce_File.Inner_C_Script.BagSystem.Manager
{
    public delegate void UseObject(string objectBag ,string objectName);
    public class BagManager : BaseManager<BagManager>
    {
        [Header("信息Data数组")]
        public ListData dataListClass;
    
        [Foldout("Grid",true)]
        [Header("背包格子Grid")]
        public GameObject plaidGrid; 
        [Header("合成台格子Grid")]
        public Transform syntheticGrid; 
        [Header("分解台格子Grid")]
        public Transform decomposeGrid;
        [Header("交换台格子Grid")]
        public Transform exchangeGrid; 
        [Header("物品栏格子Grid")] 
        public Transform inventoryGrid;
    
    
        [Foldout("预设体",true)]
        [Header("格子Brush预设体")]
        public GameObject plaid; 
        [Header("格子Object预设体")] 
        public GameObject plaidObject;
    
    
        [Foldout("数组",true)]
        [Header("格子笔画数组")]
        public List<GameObject> brushList=new List<GameObject>(); 
        [Header("格子道具数组")]
        public List<GameObject> objectList = new List<GameObject>();
    
        
        [Foldout("分界线",true)]
        [Header("背包笔画合成台分界线")] 
        public int boundaryWorkbag; 
        [Header("背包兑换台分界线")] 
        public int boundaryExchange; 
        [Header("背包物品栏分界线")] 
        public int boundaryInventory;
    
    
        [Header("合成检测数组")] 
        private string[] _checkStrings = new string[20];
        [Header("合成检测String")]
        private String _checkString;
    
        
        [Foldout("其他",true)]
        [Header("交换台格子颜色")] 
        public Color exchangeColor;
        [Header("开始时需要添加的笔画")] 
        public string[]beginBrushs;
         [Header("防遮罩坐标")] 
        public Transform endTransform;
    
        [Header("委托计数")]
        public UseObject UseObject;
        [NonSerialized]
        public int UsedCount=0;

        public Transform PlayerTrans;
        
        public void RefreshBrush()
        {
            BagClear();
            brushList.Clear();
            for (int i = 0; i<dataListClass.BrushList.Count; i++)
            {
                brushList.Add(Instantiate(plaid));
                brushList[i].GetComponent<Plaid_UI>().InitPlaid(dataListClass.BrushList[i],i);
                if (i <= boundaryWorkbag)
                {
                    brushList[i].transform.SetParent(plaidGrid.transform);
                }
                else if(i<=boundaryExchange)
                {
                    brushList[i].transform.SetParent(syntheticGrid);
                }
                else
                {
                    brushList[i].GetComponent<Image>().color = exchangeColor;
                    brushList[i].transform.SetParent(exchangeGrid);
                }
            }
        }
    

        public void RefreshObject()
        {
            BagClear();
            objectList.Clear();
            for (int i = 0; i<dataListClass.ObjectList.Count; i++)
            {
            
                objectList.Add(Instantiate(plaidObject));
                objectList[i].GetComponent<Object_UI>().InitObject(dataListClass.ObjectList[i],i);
                if(i<boundaryInventory)
                {
                    objectList[i].transform.SetParent(plaidGrid.transform);
                }
                else if (i == boundaryInventory)
                {
                    objectList[i].transform.localScale = new Vector3(2, 2, 2);
                    objectList[i].transform.SetParent(decomposeGrid);
                }
                else if (i > boundaryInventory)
                {
                    objectList[i].transform.SetParent(inventoryGrid);
                }
            }
        }

        private void BagClear()
        {
            for (int i = 0; i < syntheticGrid.childCount; i++)
            {
                Destroy(syntheticGrid.GetChild(i).gameObject);
            }
            for (int i = 0; i < decomposeGrid.childCount; i++)
            {
                Destroy(decomposeGrid.GetChild(i).gameObject);
            }

            for (int i = 0; i < plaidGrid.transform.childCount; i++) //遍历所有格子
            {
                Destroy(plaidGrid.transform.GetChild(i).gameObject);
            }
        
            for (int i = 0; i < exchangeGrid.childCount; i++) //遍历所有格子
            {
                Destroy(exchangeGrid.GetChild(i).gameObject);
            }

            for (int i = 0; i < inventoryGrid.childCount; i++)
            {
                Destroy(inventoryGrid.GetChild(i).gameObject);
            }
        }

        /* public void ExitClear()
    {
        BagClear();
        brushList.Clear();
        ObjectList.Clear();
    }*/
        /// <summary>
        /// 合成函数
        /// 遍历所有data道具，使得匹配道具数量加1.
        /// 存在问题：一开始没有某个道具。
        /// </summary>
        public void Synthesis_Brush()
        {
            for (int i = boundaryWorkbag+1,j=0; i < boundaryExchange+1&&brushList[i].GetComponent<Plaid_UI>().IsActive; i++,j++)//将合成台上所有激活的字加到检测String数组中
            {
                if (!brushList[i]) break;
                _checkStrings[j] = brushList[i].GetComponent<Plaid_UI>().Name_item;
            }

            _checkString = String.Concat(_checkStrings);//将检测数组组合成一个String变量，对应加数的字
            foreach (var item in dataListClass.ObjectList)//遍历字数组，对应字Num++
            {
                if (item  && string.Compare(item.Brush_composition,_checkString,StringComparison.Ordinal)==0)
                {
                    item.ObjectNum++;
                    CorrectionFor_01B(boundaryWorkbag+1,boundaryExchange+1,boundaryExchange+1,boundaryExchange+4);
                    RefreshBrush();
                    _checkStrings = new string[20];
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
            if (objectList[boundaryInventory].GetComponent<Object_UI>().IsActive)
            {
                var index = 0;
                for (; index < dataListClass.ObjectList[boundaryInventory].Decomposition.Length; index++)
                {
                    var t1 = dataListClass.ObjectList[boundaryInventory].Decomposition[index];
                    foreach (var t in dataListClass.BrushList)
                    {
                        if (t != null &&
                            string.Compare(t1,
                                t._brushName, StringComparison.Ordinal) == 0)
                        {
                            t._brushNum++;
                            break;
                        }
                    }
                }

                if (dataListClass.ObjectList[boundaryInventory].ObjectNum == 1&&!CorrectionFor_12O(dataListClass.ObjectList[boundaryInventory].ObjectNames))
                {
                    for (int i = 0; i <boundaryWorkbag ; i++)
                    {
                        if (dataListClass.ObjectList[i] == null)
                        {
                            dataListClass.ObjectList[i] = dataListClass.ObjectList[boundaryInventory];
                            dataListClass.ObjectList[i].ObjectNum--;
                            break;
                        }
                    }
                }
                dataListClass.ObjectList[boundaryInventory] = null;
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
            if (brushList[boundaryExchange + 1].GetComponent<Plaid_UI>().IsActive  &&
                brushList[boundaryExchange + 2].GetComponent<Plaid_UI>().IsActive  &&
                brushList[boundaryExchange + 3].GetComponent<Plaid_UI>().IsActive )
            {
                //当交换台的笔画的数为1，但合成台仍有笔画时
                /*int Num1;
                dataListClass.BrushList[boundaryExchange+3]._brushNum++;
                if (dataListClass.BrushList[boundaryExchange + 1]._brushNum == 1
                    &&(Num1=FindSameBrush(boundaryWorkbag + 1, boundaryExchange + 1,
                        dataListClass.BrushList[boundaryExchange + 1]._brushName))!=0)
                {
                    CorrectionFor_01B(boundaryExchange+2,boundaryExchange+3);
                    dataListClass.BrushList[boundaryExchange + 1]=null;
                    RefreshBrush();
                    return;
                }

                int Num2;
                if (dataListClass.BrushList[boundaryExchange + 2]._brushNum == 1
                    && (Num2 = FindSameBrush(boundaryWorkbag + 1, boundaryExchange + 1,
                        dataListClass.BrushList[boundaryExchange + 1]._brushName)) != 0)
                {
                    CorrectionFor_01B(boundaryExchange+1,boundaryExchange+3,boundaryExchange+2);
                    dataListClass.BrushList[boundaryExchange + 2] = null;
                    RefreshBrush();
                    return;
                }*/
                dataListClass.BrushList[boundaryExchange+3]._brushNum++;
                CorrectionFor_01B(boundaryExchange+1,boundaryExchange+3,boundaryWorkbag+1,boundaryExchange+1);
                RefreshBrush();
            }
            else
            {
                Debug.Log("兑换失败");
            }
        }

        private int FindSameBrush(int firstIndex,int endIndex,string findName)
        {
            for (int i=firstIndex;i<endIndex;i++)
            {
                if (dataListClass.BrushList[i]&&
                    string.Compare(dataListClass.BrushList[i]._brushName,findName,StringComparison.Ordinal)==0)
                {
                    return i;
                }
            }
            return 0;
        }


        /// <summary>
        /// 数量0，1的处理
        /// 遍历firstIndex到endIndex数组
        /// 当数组中有元素满足：笔画数为1，背包中无此画，firstIndex2到endIndex2也无此笔画，将该元素移到背包数组中，且数量--
        /// 否则，将此元素设为null。
        /// </summary>
        /// <param name="firstIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="firstIndex2"></param>
        /// <param name="endIndex2"></param>
        /// <param name="avoidIndex"></param>
        private void CorrectionFor_01B(int firstIndex,int endIndex,int firstIndex2,int endIndex2,int avoidIndex=10000)
        {
            
            for (int i = firstIndex; i < endIndex&&brushList[i].GetComponent<Plaid_UI>().IsActive&&i!=avoidIndex; i++)//背包中没有该笔画，且当前笔画数为1，但另一个地方有这个笔画
            {
                if (dataListClass.BrushList[i]._brushNum == 1 //当前笔画数为1
                    && !CorrectionFor_12B(dataListClass.BrushList[i]._brushName)//背包中没有笔画
                    && FindSameBrush(firstIndex2, endIndex2, dataListClass.BrushList[i]._brushName) != 0)//firstIndex2到endIndex2中没有笔画
                {
                    dataListClass.BrushList[i] = null;
                    continue;
                }

                if (dataListClass.BrushList[i]._brushNum == 1&&!CorrectionFor_12B(dataListClass.BrushList[i]._brushName))//背包中没有该笔画，且当前笔画数为1.将这个笔画空格与背包笔画空格移位。
                {
                    for (int j = 0; j <= boundaryWorkbag; j++)
                    {
                        if (dataListClass.BrushList[j] == null)
                        {
                            dataListClass.BrushList[j] = dataListClass.BrushList[i];
                            break;
                        }
                    }
                    dataListClass.BrushList[i]._brushNum--;
                }
                dataListClass.BrushList[i] = null;
            }
            //RefreshBrush();
        }

        /*public void CorrectionFor_01O(int ClickItemID)
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
    }*/


        /// <summary>
        /// true为数量为2的情况，false为数量为1的情况
        /// </summary>
        /// <param name="tempName"></param>
        /// <returns></returns>
        public bool CorrectionFor_12B(string tempName)
        {
            for (int i = 0; i <= boundaryWorkbag; i++)
            {
                if (dataListClass.BrushList[i]&&tempName == dataListClass.BrushList[i]._brushName)
                {
                    return true;
                }
            }
            return false;
        }


        public bool CorrectionFor_12O(string tempName)
        {
            for (int i = 0; i < boundaryInventory; i++)
            {
                if (dataListClass.ObjectList[i] != null && tempName == dataListClass.ObjectList[i].ObjectNames)
                {
                    return true;
                }
            }

            return false;
        }

        //删除从FirstIndex到EndIndex与MyID相同的元素
        public void DeleteSameObject(int myID,int firstIndex,int endIndex)
        {
            for (int i = firstIndex; i < endIndex; i++)
            {
                if (dataListClass.ObjectList[i] != null &&
                    dataListClass.ObjectList[i].ObjectNames == dataListClass.ObjectList[myID].ObjectNames&&i!=myID)
                {
                    dataListClass.ObjectList[i].ObjectNum++;
                    dataListClass.ObjectList[i] = null;
                }
            }
        }

        public void ChangeCorrection()
        {
            if (dataListClass.BrushList[boundaryExchange+1]&&
                dataListClass.BrushList[boundaryExchange+3]&&
                (dataListClass.BrushList[boundaryExchange + 1]._brushName ==
                 dataListClass.BrushList[boundaryExchange + 3]._brushName))
            {
                dataListClass.BrushList[boundaryExchange + 3]._brushNum++;
                dataListClass.BrushList[boundaryExchange + 3] = null;
            }

            if (dataListClass.BrushList[boundaryExchange + 2]  &&
                dataListClass.BrushList[boundaryExchange + 3]  &&
                (dataListClass.BrushList[boundaryExchange + 2]._brushName ==
                 dataListClass.BrushList[boundaryExchange + 3]._brushName))
            {
                dataListClass.BrushList[boundaryExchange + 3]._brushNum++;
                dataListClass.BrushList[boundaryExchange + 3] = null;
            }
        }

        //开局删除所有笔画和物品
        private void EnterScenes()
        {
            foreach (var brush in dataListClass.BrushList)
            {
                if (brush != null)
                    brush._brushNum = 0;
            }

            foreach (var item in dataListClass.ObjectList)
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
            foreach (var t in beginBrushs)
            foreach (var t1 in dataListClass.BrushList)
            {
                if (t == null) return;
                if (t1 != null &&
                    String.Compare(t1._brushName, t, StringComparison.Ordinal) == 0)
                {
                    t1._brushNum++;
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
            for (int i = 0; i <= boundaryWorkbag; i++)
            {
                if (!dataListClass.BrushList[i])//找到空格子，向后方找非空格子，将非空格子与空格子换位，若后无非空格子则结束
                {
                    if (!FindOtherBrush(i, boundaryWorkbag+1))
                    {
                        break;
                    }
                }
            }
        
            for (int i = 0; i <= boundaryWorkbag; i++)
            {
                if (dataListClass.BrushList[i]&&dataListClass.BrushList[i]._brushNum==0)//在上面基础上继续排序，01格子
                {
                    if (!FindFullBrush(i, boundaryWorkbag+1))
                    {
                        break;
                    }
                }
            }

        }

        private bool FindOtherBrush(int firstIndex,int endIndex)
        {
            for (int i = firstIndex+1; i < endIndex; i++)
            {
                if (dataListClass.BrushList[i])
                {
                    dataListClass.BrushList[firstIndex] = dataListClass.BrushList[i];
                    dataListClass.BrushList[i] = null;
                    return true;
                }
            }
            return false;
        }
        private bool FindFullBrush(int firstIndex,int endIndex)
        {
            for (int i = firstIndex+1; i < endIndex; i++)
            {
                if (dataListClass.BrushList[i]&&dataListClass.BrushList[i]._brushNum>0)
                {
                    (dataListClass.BrushList[firstIndex], dataListClass.BrushList[i]) = (dataListClass.BrushList[i], dataListClass.BrushList[firstIndex]);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteSameBrush()
        {
            for (int i = 0; i <= boundaryWorkbag; i++)
            {
                if(!dataListClass.BrushList[i])
                    continue;
                for (int j = 0; j <= boundaryWorkbag; j++)
                {
                    if (!dataListClass.BrushList[j])
                        continue;
                    if (dataListClass.BrushList[i]&&dataListClass.BrushList[j]&&
                        String.Compare(dataListClass.BrushList[i]._brushName, dataListClass.BrushList[j]._brushName, StringComparison.Ordinal) == 0
                        &&i!=j)
                    {
                        dataListClass.BrushList[j] = null;
                    }
                }
            }
        }




        /// <summary>
        /// 
        /// </summary>
        private void EnterScenesAndClearObject()
        {
            for (int i = 0; i <dataListClass.ObjectList.Count ; i++)
            {
                if (!dataListClass.ObjectList[i])
                {
                    if (!FindOtherObject(i, dataListClass.ObjectList.Count-1))
                    {
                        break;
                    }
                }
            }
        
            for (int i = 0; i <dataListClass.ObjectList.Count; i++)
            {
                if (dataListClass.ObjectList[i]&&dataListClass.ObjectList[i].ObjectNum==0)
                {
                    if (!FindFullObject(i, dataListClass.ObjectList.Count-1))
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 由于物品栏不能做排序
        /// </summary>
        public void ClickBagAndClearObject()
        {
            for (int i = 0; i <=boundaryInventory ; i++)
            {
                if (dataListClass.ObjectList[i] == null)
                {
                    if (!FindOtherObject(i, boundaryInventory))
                    {
                        break;
                    }
                }
            }
        
            for (int i = 0; i <=boundaryInventory; i++)
            {
                if (dataListClass.ObjectList[i]!=null&&dataListClass.ObjectList[i].ObjectNum==0)
                {
                    if (!FindFullObject(i, boundaryInventory))
                    {
                        break;
                    }
                }
            }

        }
    

        private bool FindOtherObject(int firstIndex,int endIndex)
        {
            for (int i = firstIndex+1; i <=endIndex; i++)
            {
                if (dataListClass.ObjectList[i] != null)
                {
                    dataListClass.ObjectList[firstIndex] = dataListClass.ObjectList[i];
                    dataListClass.ObjectList[i] = null;
                    return true;
                }
            }

            return false;
        }
        private bool FindFullObject(int firstIndex,int endIndex)
        {
            for (int i = firstIndex+1; i <=endIndex; i++)
            {
                if (dataListClass.ObjectList[i]!=null&&dataListClass.ObjectList[i].ObjectNum>0)
                {
                    (dataListClass.ObjectList[firstIndex], dataListClass.ObjectList[i]) = (dataListClass.ObjectList[i], dataListClass.ObjectList[firstIndex]);
                    return true;
                }
            }

            return false;
        }

        public void ToBrushTarget()
        {
            syntheticGrid.GetComponent<Image>().raycastTarget = true;
            decomposeGrid.GetComponent<Image>().raycastTarget = false;
        }
        public void ToObjectTarget()
        {
            syntheticGrid.GetComponent<Image>().raycastTarget = false;
            decomposeGrid.GetComponent<Image>().raycastTarget = true;
        }


        private void Start()
        {   RefreshBrush();
            RefreshObject();
            EnterScenes();
            RefreshObject();
        }
    }
}