using System;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rce_File.Inner_C_Script.BagSystem.Operate
{
    public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public enum DragModel
        {
            BrushModel,
            ObjectModel
        }

        public DragModel dragModel; //拖动模式
        private Transform _originalParent; //起始父Transform
        private int _clickItemID; //鼠标点击链表序号
        private int _currentItemID; //OnBegin链表序号
        public ListData listClass; //DataClass链表
        private string _imageName;
        private string _plaidName;
        private GameObject _endGameObject;
        public Transform endObject;
        private int _plaidID;
        private int _objectID;

        /// <summary>
        /// 将合成台物品放回背包
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (dragModel == DragModel.BrushModel)
            {

                //获得当前点击的格子编号
                _clickItemID = transform.parent.GetComponent<Plaid_UI>().ID;
                //背包内的格子没有点击事件
                if (_clickItemID <= BagManager.Instance.boundaryWorkbag)
                    return;
                //当背包里没有对应格子Brush，找到一个空的格子，将当前Brush赋给它
                if (listClass.brushList[_clickItemID]._brushNum >= 1 &&
                    !BagManager.Instance.CorrectionFor_12B(listClass.brushList[_clickItemID]._brushName))
                {
                    for (int i = 0; i <= BagManager.Instance.boundaryWorkbag; i++)
                        if (listClass.brushList[i] == null)
                        {
                            listClass.brushList[i] = listClass.brushList[_clickItemID];
                            listClass.brushList[_clickItemID] = null;
                            BagManager.Instance.RefreshBrush();
                            return;
                        }
                }
                //将合成台和交换台的物品放回背包
                listClass.brushList[_clickItemID]._brushNum++;
                listClass.brushList[_clickItemID] = null;
                BagManager.Instance.RefreshBrush();
            }



            else if (dragModel == DragModel.ObjectModel)
            {
                //获得当前点击的格子编号
                _clickItemID = transform.parent.GetComponent<Object_UI>().ID; 
                //背包内的格子没有点击事件
                if (_clickItemID < BagManager.Instance.boundaryInventory) 
                    return;
                //当背包关闭时，物品栏格子不能回到背包
                if (_clickItemID > BagManager.Instance.boundaryInventory &&
                    !BagManager.Instance.plaidGrid.activeInHierarchy ) 
                    return;
                /*if (_clickItemID > BagManager.Instance.boundaryInventory && //3.21
                    !BagManager.Instance.plaidGrid.activeInHierarchy &&
                    listClass.ObjectList[_clickItemID].CanUse == 1) //3.21
                    return;*/
                /*if (_clickItemID > BagManager.Instance.boundaryInventory && !BagManager.Instance.plaidGrid.activeInHierarchy&&listClass.ObjectList[_clickItemID].CanUse==1)//3.21
                {
                    BagManager.Instance.UseObject.Invoke();
                    if (BagManager.Instance.UsedCount == 0)
                    {
                        BagManager.Instance.UsedCount = 0;
                        return;
                    }
                    BagManager.Instance.UsedCount = 0;
                    //使用道具函数
                    if (listClass.ObjectList[_clickItemID].ObjectNum >= 1&&!BagManager.Instance.CorrectionFor_12O(listClass.ObjectList[_clickItemID].ObjectNames))
                    {
                        for(int i=0;i<BagManager.Instance.boundaryWorkbag;i++)
                            if (listClass.ObjectList[i] == null)
                            {
                                listClass.ObjectList[i] = listClass.ObjectList[_clickItemID];
                                listClass.ObjectList[_clickItemID].ObjectNum -= 1;
                                listClass.ObjectList[_clickItemID] = null;
                                BagManager.Instance.RefreshObject();
                                return;
                            }
                    
                    }
                    listClass.ObjectList[_clickItemID] = null;
                    BagManager.Instance.RefreshObject();
                    return;//3.21
                }*/

                //当背包里没有对应格子Object，找到一个空的格子，将当前Object赋给它
                if (listClass.objectList[_clickItemID].ObjectNum >= 1 &&
                    !BagManager.Instance.CorrectionFor_12O(listClass.objectList[_clickItemID].ObjectNames))
                {
                    for (int i = 0; i < BagManager.Instance.boundaryWorkbag; i++)
                        if (listClass.objectList[i] == null)
                        {
                            listClass.objectList[i] = listClass.objectList[_clickItemID];
                            listClass.objectList[_clickItemID] = null;
                            BagManager.Instance.RefreshObject();
                            return;
                        }
                }
                //将分解台和物品栏的物品放回背包
                listClass.objectList[_clickItemID].ObjectNum++;
                listClass.objectList[_clickItemID] = null;
                BagManager.Instance.RefreshObject();
            }
        }


        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            
            _originalParent = transform.parent;
            //笔画模式
            if (dragModel == DragModel.BrushModel)
            {
                //_currentItemID为原格子ID
                _currentItemID = _originalParent.GetComponent<Plaid_UI>().ID;
                //除背包外格子不能拖
                if (_currentItemID > BagManager.Instance.boundaryWorkbag)
                    return;
                //分辨是空格子还是有物品
                _imageName = "brush_Image";
                _plaidName = "plaid_Brush(Clone)";
            }
            //物品模式
            else if (dragModel == DragModel.ObjectModel)
            {
                //_currentItemID为原格子ID
                _currentItemID = _originalParent.GetComponent<Object_UI>().ID;
                //if (_currentItemID >= BagManager.Instance.boundaryInventory)
                //合成台格子不能拖
                if (_currentItemID == BagManager.Instance.boundaryInventory) //3.21
                    return;
                //背包外，拖物品栏可用物品，改变图片，改变大小
                if (_currentItemID > BagManager.Instance.boundaryInventory
                    && dragModel == DragModel.ObjectModel && !BagManager.Instance.plaidGrid.activeInHierarchy
                    && listClass.objectList[_currentItemID].CanUse == 1) //3.21
                {
                    if (listClass.objectList[_currentItemID] && BagManager.Instance.objectList[_currentItemID])
                    {
                        BagManager.Instance.objectList[_currentItemID].GetComponent<Object_UI>().plaid.sprite =
                            listClass.objectList[_currentItemID].ObjectUI_Scenes;
                        this.transform.localScale = new(4, 4, 0);
                    }
                    
                }
                _imageName = "object_Image";
                _plaidName = "plaid_Object(Clone)";
            }

            Transform transformTemp;
            (transformTemp = transform).SetParent(endObject);
            transformTemp.position = eventData.position;
            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            //除Brush背包外格子不能拖
            if (_currentItemID > BagManager.Instance.boundaryWorkbag && dragModel == DragModel.BrushModel)
                return;
            //else if(_currentItemID>=BagManager.Instance.boundaryInventory&&dragModel==DragModel.ObjectModel)
            //合成台格子不能拖
            else if (_currentItemID == BagManager.Instance.boundaryInventory && dragModel == DragModel.ObjectModel) //3.21
                return;
            transform.position = eventData.position;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            
            //除Brush背包外格子不能拖
            if (_currentItemID > BagManager.Instance.boundaryWorkbag && dragModel == DragModel.BrushModel)
                return;
            //else if(_currentItemID>=BagManager.Instance.boundaryInventory&&dragModel==DragModel.ObjectModel)
            //合成台格子不能拖
            else if (_currentItemID == BagManager.Instance.boundaryInventory && dragModel == DragModel.ObjectModel) //3.21
                return;
            //_endGameObject为后格子
            _endGameObject = eventData.pointerCurrentRaycast.gameObject;
            //在场景中拖动物品，不处理物品栏之间
            if (!_endGameObject&&_currentItemID > BagManager.Instance.boundaryInventory 
                               && dragModel == DragModel.ObjectModel&&!BagManager.Instance.plaidGrid.activeInHierarchy
                               &&listClass.objectList[_currentItemID].CanUse==1)//3.21
           {
               //设置为原大小和图片
               BagManager.Instance.objectList[_currentItemID].GetComponent<Object_UI>().plaid.sprite = listClass.objectList[_currentItemID].ObjectUI_Bag;
               this.transform.localScale = new(0.6f, 0.6f, 0);
               
               
               //射线检测，通过判断拖到的位置，判断是否可使用道具
               if (Camera.main != null)
               {
                   
                   Vector3 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                   //Vector3 screenPos2D = new Vector3(screenPos3D.x, screenPos3D.y, -Camera.main.transform.position.z);
                   //Vector2 screenPos = Camera.main.ScreenToWorldPoint(screenPos2D);
                   /*Vector2 playerPos = BagManager.Instance.PlayerTrans.position;
                   float distance = (screenPos - playerPos).magnitude;
                   RaycastHit2D hit = Physics2D.Raycast(playerPos,screenPos, distance,(1<<9));
                   Debug.DrawLine(playerPos,screenPos,Color.red,distance);*/
                   Collider2D coll = Physics2D.OverlapCircle(screenPos, 2f, (1 << 9));
                   if (coll)
                   {
                       Debug.Log(coll.name);
                       BagManager.Instance.UseObject.Invoke(listClass.objectList[_currentItemID].ObjectNames,coll.name);
                       if (BagManager.Instance.UsedCount == 0)//没有接触到物品
                       {
                           BagManager.Instance.UsedCount = 0;
                           Transform transformTemp3;
                           (transformTemp3 = transform).SetParent(_originalParent);
                           transformTemp3.position = _originalParent.position;
                           GetComponent<CanvasGroup>().blocksRaycasts = true;
                           return;
                       }
                       BagManager.Instance.UsedCount = 0;
                       //使用道具后，数量减少函数
                       if (listClass.objectList[_currentItemID].ObjectNum >= 1&&!BagManager.Instance.CorrectionFor_12O(listClass.objectList[_currentItemID].ObjectNames))
                       {
                           for(int i=0;i<BagManager.Instance.boundaryWorkbag;i++)
                               if (listClass.objectList[i] == null)
                               {
                                   listClass.objectList[i] = listClass.objectList[_currentItemID];
                                   if(listClass.objectList[_currentItemID].ObjectNames!="竿"&&
                                      listClass.objectList[_currentItemID].ObjectNames!="桶")
                                   listClass.objectList[_currentItemID].ObjectNum -= 1;
                                   listClass.objectList[_currentItemID] = null;
                                   BagManager.Instance.UsedCount = 0;
                                   Transform transformTemp3;
                                   (transformTemp3 = transform).SetParent(_originalParent);
                                   transformTemp3.position = _originalParent.position;
                                   GetComponent<CanvasGroup>().blocksRaycasts = true;
                                   BagManager.Instance.RefreshObject();
                                   return;
                               }
                       }

                       if (listClass.objectList[_currentItemID].ObjectNames == "竿" ||
                           listClass.objectList[_currentItemID].ObjectNames == "桶")
                           listClass.objectList[_clickItemID].ObjectNum+= 1;
                       listClass.objectList[_currentItemID] = null;
                       BagManager.Instance.RefreshObject();
                   }
               }
           } //3.21

            //背包内处理
            if (_endGameObject)
            {
                //处理物品栏之间，拖动物体还原大小和图片
                if (dragModel == DragModel.ObjectModel&&_currentItemID>BagManager.Instance.boundaryInventory)
                {
                    BagManager.Instance.objectList[_currentItemID].GetComponent<Object_UI>().plaid.sprite = listClass.objectList[_currentItemID].ObjectUI_Bag;
                    this.transform.localScale = new Vector3(0.6f, 0.6f, 0);
                }
                
                //格子不为空
                if (String.Compare(_endGameObject.name,_imageName,StringComparison.Ordinal)==0)
                {
                    GetEndObjectID();
                    //笔画拖到合成台
                    if (dragModel == DragModel.BrushModel && _plaidID >=
                        BagManager.Instance.boundaryWorkbag) 
                    {
                        Transform transformTemp;
                        (transformTemp = transform).SetParent(_originalParent);
                        transformTemp.position = _originalParent.position;
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        return;
                    }

                    //字拖到分解台
                    if (dragModel == DragModel.ObjectModel && _objectID ==
                        BagManager.Instance.boundaryInventory)
                    {
                        Transform transformTemp;
                        (transformTemp = transform).SetParent(_originalParent);
                        transformTemp.position = _originalParent.position;
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        return;
                    }

                    
                    Transform transformTemp1;
                    var parent = _endGameObject.transform.parent;
                    var parentTemp1 = parent.parent;
                    (transformTemp1 = transform).SetParent(parentTemp1);
                    transformTemp1.position = parentTemp1.position;
                    
                    //背包内，交换笔画
                    if (dragModel == DragModel.BrushModel) 
                    {
                        (listClass.brushList[_currentItemID], listClass.brushList[_plaidID]) = (listClass.brushList[_plaidID], listClass.brushList[_currentItemID]);

                        var parentTemp = _endGameObject.transform.parent;
                        parentTemp.position = _originalParent.position;
                        parentTemp.SetParent(_originalParent);
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        BagManager.Instance.RefreshBrush();
                    }
                    
                    //背包内交换物品+物品栏交换物品
                    else if (dragModel == DragModel.ObjectModel
                             &&(_currentItemID<BagManager.Instance.boundaryInventory&&_objectID<BagManager.Instance.boundaryInventory)
                             ||(_currentItemID>BagManager.Instance.boundaryInventory&&_objectID>BagManager.Instance.boundaryInventory)) //改3.22
                    {
                        (listClass.objectList[_currentItemID], listClass.objectList[_objectID]) = (listClass.objectList[_objectID], listClass.objectList[_currentItemID]);
                        var parentTemp2 = _endGameObject.transform.parent;
                        parentTemp2.position = _originalParent.position;
                        parentTemp2.SetParent(_originalParent);
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        BagManager.Instance.RefreshObject();
                    }
                    Transform transformTemp3;
                    (transformTemp3 = transform).SetParent(_originalParent);
                    transformTemp3.position = _originalParent.position;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }

                
                else if (eventData.pointerCurrentRaycast.gameObject.name == _plaidName) //格子为空
                {
                   GetEndObjectID();
                   Transform transformTemp;
                    (transformTemp = transform).SetParent(_endGameObject.transform);
                    transformTemp.position = _endGameObject.transform.position;
                    //笔画拖到空格子里，笔画拖到合成台格子。
                    if (dragModel == DragModel.BrushModel) 
                    {
                        if (!listClass.brushList[_plaidID])//空格子
                        {
                            //将结束格子=开始格子
                            listClass.brushList[_plaidID] = listClass.brushList[_currentItemID];
                            //背包内部笔画格子交换
                            if (_plaidID != _currentItemID && _plaidID <= BagManager.Instance.boundaryWorkbag)
                            {
                                listClass.brushList[_currentItemID] = null;
                            }
                            //移到背包外格子，笔画数量-1
                            else if (_plaidID != _currentItemID && _plaidID > BagManager.Instance.boundaryWorkbag)
                            {
                                if (_plaidID > BagManager.Instance.boundaryExchange)
                                {
                                    BagManager.Instance.ChangeCorrection();
                                }

                                if (listClass.brushList[_currentItemID]._brushNum >= 2)
                                    listClass.brushList[_currentItemID]._brushNum--;
                                else
                                    listClass.brushList[_currentItemID] = null;
                            }
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            BagManager.Instance.RefreshBrush();
                            return;
                        }
                        //数量为0
                        else
                        {
                            (listClass.brushList[_currentItemID], listClass.brushList[_plaidID]) = (
                                listClass.brushList[_plaidID], listClass.brushList[_currentItemID]);
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            BagManager.Instance.RefreshBrush();
                            return;
                        }
                    }

                    else if (dragModel == DragModel.ObjectModel)
                    {

                        if (_currentItemID > BagManager.Instance.boundaryInventory &&
                            _objectID <= BagManager.Instance.boundaryInventory)
                        {
                            Transform transformTemp3;
                            (transformTemp3 = transform).SetParent(_originalParent);
                            transformTemp3.position = _originalParent.position;
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            return;
                        }


                        //物品拖到空格子里。
                        if (!listClass.objectList[_objectID])
                        {
                            //将结束格子=开始格子
                            listClass.objectList[_objectID] = listClass.objectList[_currentItemID];
                            //背包内部笔画格子交换
                            if (_objectID != _currentItemID && _objectID < BagManager.Instance.boundaryInventory)
                            {
                                listClass.objectList[_currentItemID] = null;
                            }
                            //背包中的物品拖到分解台和物品栏，数量减1
                            else if (_currentItemID<BagManager.Instance.boundaryInventory&&_objectID != _currentItemID && _objectID >= BagManager.Instance.boundaryInventory)
                            {
                                if (_objectID > BagManager.Instance.boundaryInventory)
                                {
                                    BagManager.Instance.DeleteSameObject(_objectID,
                                        BagManager.Instance.boundaryInventory + 1, listClass.objectList.Count);
                                }
                                
                                if (listClass.objectList[_currentItemID].ObjectNum >= 2)
                                    listClass.objectList[_currentItemID].ObjectNum--;
                                else
                                    listClass.objectList[_currentItemID] = null;
                            }
                            //物品栏范围拖拽
                            else if (_currentItemID > BagManager.Instance.boundaryInventory &&
                                _objectID != _currentItemID && _objectID > BagManager.Instance.boundaryInventory)
                            {
                                listClass.objectList[_currentItemID] = null;
                            }
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            BagManager.Instance.RefreshObject();
                        }
                        //数量为0
                        else
                        {
                            (listClass.objectList[_currentItemID], listClass.objectList[_objectID]) = (
                                listClass.objectList[_objectID], listClass.objectList[_currentItemID]);
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            BagManager.Instance.RefreshObject();
                        }
                    }
                    return;
                }
            }
            Transform transformTemp2;
            (transformTemp2 = transform).SetParent(_originalParent);
            transformTemp2.position = _originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        //处理sb的Canvas的方法，我也不知道为什么能行
        public void OnEnable()
        {
            _originalParent = transform.parent;
            endObject = BagManager.Instance.endTransform;
            transform.SetParent(endObject);
            Transform transformTemp;
            (transformTemp = transform).SetParent(_originalParent);
            transformTemp.position = _originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        
        private void GetEndObjectID()
        {
            if(dragModel==DragModel.BrushModel)
                _plaidID =_endGameObject.GetComponentInParent<Plaid_UI>().ID;
            else if(dragModel == DragModel.ObjectModel)
                _objectID =_endGameObject.GetComponentInParent<Object_UI>().ID;
        }
        
    }
}