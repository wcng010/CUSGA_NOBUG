using System;
using System.Collections.Generic;
using Pixeye.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
        public string[] beginBrushs1;

        public string[] beginBrushs2;

        public string[] beginBrushs3;
         [Header("防遮罩坐标")] 
        public Transform endTransform;
    
        [Header("委托计数")]
        public UseObject UseObject;
        [NonSerialized]
        public int UsedCount=0;

        [Header("当前场景号")] 
        private int _sceneNumNow;
        [Header("前一个场景号")]
        private int _sceneNumPast;
        [Header("书")] [SerializeField] 
        private Button bookImage;
        #region RefreshFunction
        public void RefreshBrush()
        {
            BagClear();
            brushList.Clear();
            for (int i = 0; i<dataListClass.brushList.Count; i++)
            {
                brushList.Add(Instantiate(plaid));
                brushList[i].GetComponent<Plaid_UI>().InitPlaid(dataListClass.brushList[i],i);
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
            for (int i = 0; i<dataListClass.objectList.Count; i++)
            {
                objectList.Add(Instantiate(plaidObject));
                objectList[i].GetComponent<Object_UI>().InitObject(dataListClass.objectList[i],i);
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
        #endregion
        

        #region BagFunction
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
            foreach (var item in dataListClass.objectList)//遍历字数组，对应字Num++
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
            if (String.CompareOrdinal(dataListClass.objectList[boundaryInventory].ObjectNames, "书") == 0)
            {
                bookImage.gameObject.SetActive(true);
            }
            if (objectList[boundaryInventory].GetComponent<Object_UI>().IsActive)
            {
                var index = 0;
                for (; index < dataListClass.objectList[boundaryInventory].Decomposition.Length; index++)
                {
                    var t1 = dataListClass.objectList[boundaryInventory].Decomposition[index];
                    foreach (var t in dataListClass.brushList)
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

                if (dataListClass.objectList[boundaryInventory].ObjectNum == 1&&!CorrectionFor_12O(dataListClass.objectList[boundaryInventory].ObjectNames))
                {
                    for (int i = 0; i <boundaryWorkbag ; i++)
                    {
                        if (dataListClass.objectList[i] == null)
                        {
                            dataListClass.objectList[i] = dataListClass.objectList[boundaryInventory];
                            dataListClass.objectList[i].ObjectNum--;
                            break;
                        }
                    }
                }
                dataListClass.objectList[boundaryInventory] = null;
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
                dataListClass.brushList[boundaryExchange+3]._brushNum++;
                CorrectionFor_01B(boundaryExchange+1,boundaryExchange+3,boundaryWorkbag+1,boundaryExchange+1);
                RefreshBrush();
            }
            else
            {
                Debug.Log("兑换失败");
            }
        }
        #endregion
        

        #region CorrectionFunction
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
                if (dataListClass.brushList[i]._brushNum == 1 //当前笔画数为1
                    && !CorrectionFor_12B(dataListClass.brushList[i]._brushName)//背包中没有笔画
                    && FindSameBrush(firstIndex2, endIndex2, dataListClass.brushList[i]._brushName) != 0)//firstIndex2到endIndex2中没有笔画
                {
                    dataListClass.brushList[i] = null;
                    continue;
                }

                if (dataListClass.brushList[i]._brushNum == 1&&!CorrectionFor_12B(dataListClass.brushList[i]._brushName))//背包中没有该笔画，且当前笔画数为1.将这个笔画空格与背包笔画空格移位。
                {
                    for (int j = 0; j <= boundaryWorkbag; j++)
                    {
                        if (dataListClass.brushList[j] == null)
                        {
                            dataListClass.brushList[j] = dataListClass.brushList[i];
                            break;
                        }
                    }
                    dataListClass.brushList[i]._brushNum--;
                }
                dataListClass.brushList[i] = null;
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
                if (dataListClass.brushList[i]&&tempName == dataListClass.brushList[i]._brushName)
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
                if (dataListClass.objectList[i] != null && tempName == dataListClass.objectList[i].ObjectNames)
                {
                    return true;
                }
            }

            return false;
        }
        
        public void ChangeCorrection()
        {
            if (dataListClass.brushList[boundaryExchange+1]&&
                dataListClass.brushList[boundaryExchange+3]&&
                (dataListClass.brushList[boundaryExchange + 1]._brushName ==
                 dataListClass.brushList[boundaryExchange + 3]._brushName))
            {
                dataListClass.brushList[boundaryExchange + 3]._brushNum++;
                dataListClass.brushList[boundaryExchange + 3] = null;
            }

            if (dataListClass.brushList[boundaryExchange + 2]  &&
                dataListClass.brushList[boundaryExchange + 3]  &&
                (dataListClass.brushList[boundaryExchange + 2]._brushName ==
                 dataListClass.brushList[boundaryExchange + 3]._brushName))
            {
                dataListClass.brushList[boundaryExchange + 3]._brushNum++;
                dataListClass.brushList[boundaryExchange + 3] = null;
            }
        }

        #endregion

        #region BagSort
        /// <summary>
        /// 对所有笔画格子进行排序，
        /// 将空格子移到后方
        /// 再将笔画为0的格子移到后方
        /// </summary>
        public void EnterScenesAndClearBrush()
        {
            for (int i = 0; i <= boundaryWorkbag; i++)
            {
                if (!dataListClass.brushList[i])//找到空格子，向后方找非空格子，将非空格子与空格子换位，若后无非空格子则结束
                {
                    if (!FindOtherBrush(i, boundaryWorkbag+1))
                    {
                        break;
                    }
                }
            }
        
            for (int i = 0; i <= boundaryWorkbag; i++)
            {
                if (dataListClass.brushList[i]&&dataListClass.brushList[i]._brushNum==0)//在上面基础上继续排序，01格子
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
                if (dataListClass.brushList[i])
                {
                    dataListClass.brushList[firstIndex] = dataListClass.brushList[i];
                    dataListClass.brushList[i] = null;
                    return true;
                }
            }
            return false;
        }
        private bool FindFullBrush(int firstIndex,int endIndex)
        {
            for (int i = firstIndex+1; i < endIndex; i++)
            {
                if (dataListClass.brushList[i]&&dataListClass.brushList[i]._brushNum>0)
                {
                    (dataListClass.brushList[firstIndex], dataListClass.brushList[i]) = (dataListClass.brushList[i], dataListClass.brushList[firstIndex]);
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// 对所有物品格子进行排序
        /// </summary>
        private void EnterScenesAndClearObject()
        {
            for (int i = 0; i <dataListClass.objectList.Count ; i++)
            {
                if (!dataListClass.objectList[i])
                {
                    if (!FindOtherObject(i, dataListClass.objectList.Count-1))
                    {
                        break;
                    }
                }
            }
        
            for (int i = 0; i <dataListClass.objectList.Count; i++)
            {
                if (dataListClass.objectList[i]&&dataListClass.objectList[i].ObjectNum==0)
                {
                    if (!FindFullObject(i, dataListClass.objectList.Count-1))
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
                if (dataListClass.objectList[i] != null)
                {
                    dataListClass.objectList[firstIndex] = dataListClass.objectList[i];
                    dataListClass.objectList[i] = null;
                    return true;
                }
            }

            return false;
        }
        
        private bool FindFullObject(int firstIndex,int endIndex)
        {
            for (int i = firstIndex+1; i <=endIndex; i++)
            {
                if (dataListClass.objectList[i]!=null&&dataListClass.objectList[i].ObjectNum>0)
                {
                    (dataListClass.objectList[firstIndex], dataListClass.objectList[i]) = (dataListClass.objectList[i], dataListClass.objectList[firstIndex]);
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// 由于物品栏不能做排序
        /// </summary>
        public void ClickBagAndClearObject()
        {
            for (int i = 0; i <=boundaryInventory ; i++)
            {
                if (dataListClass.objectList[i] == null)
                {
                    if (!FindOtherObject(i, boundaryInventory))
                    {
                        break;
                    }
                }
            }
        
            for (int i = 0; i <=boundaryInventory; i++)
            {
                if (dataListClass.objectList[i]!=null&&dataListClass.objectList[i].ObjectNum==0)
                {
                    if (!FindFullObject(i, boundaryInventory))
                    {
                        break;
                    }
                }
            }

        }
        #endregion
        
        #region ChangeSceneFunction
        /// <summary>
        /// 转场景时调用，换上新的字，初始化笔画。
        /// </summary>
        /// <param name="currentScene"></param>
        /// <param name="nextScene"></param>
        private void ChangeScene(Scene currentScene,Scene nextScene)
        {
            _sceneNumPast = _sceneNumNow;
            _sceneNumNow = SceneManager.GetActiveScene().buildIndex;
            ClearObjectData(dataListClass.objectList);
            if(_sceneNumPast==3&&_sceneNumNow==4//第二关进入第三关
               ||_sceneNumNow==4&&_sceneNumPast==4)//第三关内按Restart
            {   
                LoadObjectData(dataListClass.objectListBuffer3); 
                EnterScenes(4);
                return;
            }
            switch (_sceneNumNow)
            {
                case 0 : Destroy(this.gameObject); break;
                case 1: Destroy(this.gameObject); break;
                case 6: Destroy(this.gameObject); break;
                case 2: LoadObjectData(dataListClass.objectListBuffer1);EnterScenes(2);break;
                case 3: LoadObjectData(dataListClass.objectListBuffer2);EnterScenes(3);break;
            }
        }

        /// <summary>
        /// 将缓冲区data加载到List
        /// </summary>
        /// <param name="objectListBuffer"></param>
        public void LoadObjectData(List<ObjectData> objectListBuffer)
        {
            for (int i = 0; i < objectListBuffer.Count; i++)
            {
                if (objectListBuffer[i])
                {
                    dataListClass.objectList[i] = objectListBuffer[i];
                }
                else
                {
                    Debug.LogError("Error");
                }
            }
        }

        /// <summary>
        /// 清空物品背包，全为null
        /// </summary>
        /// <param name="objectDataList"></param>
        private void ClearObjectData(List<ObjectData> objectDataList)
        {
            for (int i = 0; i < objectDataList.Count; i++)
            {
                if (!objectDataList[i])
                {
                    objectDataList[i] = null;
                }
            }

        }
        
        //开局删除所有笔画和物品
        public void EnterScenes(int levelNum)
        {
            foreach (var brush in dataListClass.brushList)
            {
                if (brush != null)
                    brush._brushNum = 0;
            }
            foreach (var item in dataListClass.objectList)
            {
                if (item != null)
                    item.ObjectNum = 0;
            }

            switch (levelNum)
            {
                case 2:AddBrushOnBegin(beginBrushs1); break;
                case 3:AddBrushOnBegin(beginBrushs2); break;
                case 4:AddBrushOnBegin(beginBrushs3); break;
            }
            EnterScenesAndClearBrush();
            EnterScenesAndClearObject();
            RefreshObject();
        }
        /// <summary>
        /// 开局添加笔画
        /// </summary>
        /// <param name="beginBrushs"></param>
        private void AddBrushOnBegin(string[] beginBrushs)
        {
            foreach (var t in beginBrushs)
            foreach (var t1 in dataListClass.brushList)
            {
                if (t == null) return;
                if (t1 != null &&
                    String.Compare(t1._brushName, t, StringComparison.Ordinal) == 0)
                {
                    t1._brushNum++;
                }
            }
        }
        
        #endregion
        /// <summary>
        /// 查找firstIndex到endIndex范围内名字相同的笔画
        /// </summary>
        /// <param name="firstIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="findName"></param>
        /// <returns></returns>
        private int FindSameBrush(int firstIndex,int endIndex,string findName)
        {
            for (int i=firstIndex;i<endIndex;i++)
            {
                if (dataListClass.brushList[i]&&
                    string.Compare(dataListClass.brushList[i]._brushName,findName,StringComparison.Ordinal)==0)
                {
                    return i;
                }
            }
            return 0;
        }
        
        //删除从FirstIndex到EndIndex与MyID相同的物品
        public void DeleteSameObject(int myID,int firstIndex,int endIndex)
        {
            for (int i = firstIndex; i < endIndex; i++)
            {
                if (dataListClass.objectList[i] != null &&
                    dataListClass.objectList[i].ObjectNames == dataListClass.objectList[myID].ObjectNames&&i!=myID)
                {
                    dataListClass.objectList[i].ObjectNum++;
                    dataListClass.objectList[i] = null;
                }
            }
        }
        
        /// <summary>
        /// 删除背包内相同的笔画
        /// </summary>
        public void DeleteSameBrush()
        {
            for (int i = 0; i <= boundaryWorkbag; i++)
            {
                if(!dataListClass.brushList[i])
                    continue;
                for (int j = 0; j <= boundaryWorkbag; j++)
                {
                    if (!dataListClass.brushList[j])
                        continue;
                    if (dataListClass.brushList[i]&&dataListClass.brushList[j]&&
                        String.Compare(dataListClass.brushList[i]._brushName, dataListClass.brushList[j]._brushName, StringComparison.Ordinal) == 0
                        &&i!=j)
                    {
                        dataListClass.brushList[j] = null;
                    }
                }
            }
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
        
        
        
        private void OnEnable()
        {
            DontDestroyOnLoad(this);
            SceneManager.activeSceneChanged += ChangeScene;//转场调用
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= ChangeScene;
        }
    }
}