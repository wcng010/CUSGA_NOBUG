using System;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using Rce_File.Inner_C_Script.BagSystem.Operate;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rce_File.Inner_C_Script.BagSystem.Object
{
    public sealed class ObjectDrag : BaseDrag<ObjectData>
    {
        public override void OnEnable()
        {
            base.OnEnable();
            _datasList = listClass.objectList;
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            //获得当前点击的格子编号
            GetID<Object_UI>(ref FirstItemID,transform.parent);
            //背包内的格子没有点击事件
            if (FirstItemID < BagManager.Instance.boundaryInventory) 
                return;
            //当背包关闭时，物品栏格子不能回到背包
            if (FirstItemID > BagManager.Instance.boundaryInventory &&
                !BagManager.Instance.plaidGrid.activeInHierarchy ) 
                return;
            //当背包里没有对应格子Object，找到一个空的格子，将当前Object赋给它
            if (_datasList[FirstItemID].ObjectNum >= 1 &&
                !BagManager.Instance.CorrectionFor_12O(_datasList[FirstItemID].ObjectNames))
            {
                for (int i = 0; i < BagManager.Instance.boundaryWorkbag; i++)
                    if (_datasList[i] == null)
                    {
                        _datasList[i] = _datasList[FirstItemID];
                        _datasList[FirstItemID] = null;
                        BagManager.Instance.RefreshObject();
                        return;
                    }
            }
            //将分解台和物品栏的物品放回背包
            _datasList[FirstItemID].ObjectNum++;
            _datasList[FirstItemID] = null;
            BagManager.Instance.RefreshObject();
        }
        


        public override void OnBeginDrag(PointerEventData eventData)
        {
            
                OriginalParent = transform.parent;
                //_currentItemID为原格子ID
                GetID<Object_UI>(ref FirstItemID, OriginalParent);
                //合成台格子不能拖
                if (FirstItemID == BagManager.Instance.boundaryInventory) 
                    return;
                //背包外，拖物品栏可用物品，改变图片，改变大小
                if (FirstItemID > BagManager.Instance.boundaryInventory 
                    && !BagManager.Instance.plaidGrid.activeInHierarchy
                    && _datasList[FirstItemID].CanUse == 1) //3.21
                {
                    if (_datasList[FirstItemID] && BagManager.Instance.objectList[FirstItemID])
                    {
                        BagManager.Instance.objectList[FirstItemID].GetComponent<Object_UI>().plaid.sprite =
                            _datasList[FirstItemID].ObjectUI_Scenes;
                        this.transform.localScale = new(4, 4, 0);
                    }
                }
                ImageName = "object_Image";
                PlaidName = "plaid_Object(Clone)";
                
            Transform transformTemp;
            (transformTemp = transform).SetParent(endObject);
            transformTemp.position = eventData.position;
            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            //合成台格子不能拖
            if (FirstItemID == BagManager.Instance.boundaryInventory ) //3.21
                return;
            transform.position = eventData.position;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            
            //else if(_currentItemID>=BagManager.Instance.boundaryInventory&&dragModel==DragModel.ObjectModel)
            //合成台格子不能拖
            if (FirstItemID == BagManager.Instance.boundaryInventory ) //3.21
                return;
            //_endGameObject为后格子
            EndGameObject = eventData.pointerCurrentRaycast.gameObject;
            //在场景中拖动物品，不处理物品栏之间
            if (!EndGameObject&&FirstItemID > BagManager.Instance.boundaryInventory 
                               &&!BagManager.Instance.plaidGrid.activeInHierarchy
                               &&_datasList[FirstItemID].CanUse==1)//3.21
            {
                //设置为原大小和图片
                BagManager.Instance.objectList[FirstItemID].GetComponent<Object_UI>().plaid.sprite = _datasList[FirstItemID].ObjectUI_Bag;
                this.transform.localScale = new(0.6f, 0.6f, 0);
               
               
                //射线检测，通过判断拖到的位置，判断是否可使用道具
                if (Camera.main != null)
                {
                    Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y,
                        Input.mousePosition.z - transform.position.z));
                    RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, 100);
                    if (hit)
                    {
                        BagManager.Instance.UseObject.Invoke(listClass.objectList[EndItemID].ObjectNames,hit.collider.name);
                        if (BagManager.Instance.UsedCount == 0)//没有接触到物品
                        {
                            BagManager.Instance.UsedCount = 0;
                            Transform transformTemp3;
                            (transformTemp3 = transform).SetParent(OriginalParent);
                            transformTemp3.position = OriginalParent.position;
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            return;
                        }
                        BagManager.Instance.UsedCount = 0;
                        //使用道具后，数量减少函数
                        if (_datasList[FirstItemID].ObjectNum >= 1&&!BagManager.Instance.CorrectionFor_12O(_datasList[FirstItemID].ObjectNames))
                        {
                            for(int i=0;i<BagManager.Instance.boundaryWorkbag;i++)
                                if (_datasList[i] == null)
                                {
                                    _datasList[i] = _datasList[FirstItemID];
                                    _datasList[FirstItemID].ObjectNum -= 1;
                                    _datasList[FirstItemID] = null;
                                    BagManager.Instance.UsedCount = 0;
                                    Transform transformTemp3;
                                    (transformTemp3 = transform).SetParent(OriginalParent);
                                    transformTemp3.position = OriginalParent.position;
                                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                                    BagManager.Instance.RefreshObject();
                                    return;
                                }
                        }
                        _datasList[FirstItemID] = null;
                        BagManager.Instance.RefreshObject();
                    }
                }
            } //3.21

            //背包内处理
            if (EndGameObject)
            {
                //处理物品栏之间，拖动物体还原大小和图片
                if (FirstItemID>BagManager.Instance.boundaryInventory)
                {
                    BagManager.Instance.objectList[FirstItemID].GetComponent<Object_UI>().plaid.sprite = _datasList[FirstItemID].ObjectUI_Bag;
                    this.transform.localScale = new(0.6f, 0.6f, 0);
                }
                GetID<Object_UI>(ref EndItemID,EndGameObject.transform);
                //格子不为空
                if (String.Compare(EndGameObject.name,ImageName,StringComparison.Ordinal)==0)
                {
                    //字拖到分解台
                    if ( EndItemID == BagManager.Instance.boundaryInventory)
                    {
                        Transform transformTemp;
                        (transformTemp = transform).SetParent(OriginalParent);
                        transformTemp.position = OriginalParent.position;
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        return;
                    }

                    
                    Transform transformTemp1;
                    var parent = EndGameObject.transform.parent;
                    var parentTemp1 = parent.parent;
                    (transformTemp1 = transform).SetParent(parentTemp1);
                    transformTemp1.position = parentTemp1.position;
                    
                    //背包内交换物品+物品栏交换物品
                    if ((FirstItemID<BagManager.Instance.boundaryInventory&&EndItemID<BagManager.Instance.boundaryInventory)
                        ||(FirstItemID>BagManager.Instance.boundaryInventory&&EndItemID>BagManager.Instance.boundaryInventory)) //改3.22
                    {
                        (_datasList[FirstItemID], _datasList[EndItemID]) = (_datasList[EndItemID], _datasList[FirstItemID]);
                        var parentTemp2 = EndGameObject.transform.parent;
                        parentTemp2.position = OriginalParent.position;
                        parentTemp2.SetParent(OriginalParent);
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        BagManager.Instance.RefreshObject();
                    }
                    
                    Transform transformTemp3;
                    (transformTemp3 = transform).SetParent(OriginalParent);
                    transformTemp3.position = OriginalParent.position;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }

                
                else if (String.Compare(EndGameObject.name,PlaidName,StringComparison.Ordinal)==0) //格子为空
                {
                    Transform transformTemp;
                    (transformTemp = transform).SetParent(EndGameObject.transform);
                    transformTemp.position = EndGameObject.transform.position;
                    //笔画拖到空格子里，笔画拖到合成台格子。

                    if (FirstItemID > BagManager.Instance.boundaryInventory &&
                        EndItemID <= BagManager.Instance.boundaryInventory)
                        {
                            Transform transformTemp3;
                            (transformTemp3 = transform).SetParent(OriginalParent);
                            transformTemp3.position = OriginalParent.position;
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            return;
                        }


                        //物品拖到空格子里。
                        if (!_datasList[EndItemID])
                        {
                            //将结束格子=开始格子
                            _datasList[EndItemID] = _datasList[FirstItemID];
                            //背包内部笔画格子交换
                            if (EndItemID != FirstItemID && EndItemID < BagManager.Instance.boundaryInventory)
                            {
                                _datasList[FirstItemID] = null;
                            }
                            //背包中的物品拖到分解台和物品栏，数量减1
                            else if (FirstItemID<BagManager.Instance.boundaryInventory&&EndItemID != FirstItemID && EndItemID >= BagManager.Instance.boundaryInventory)
                            {
                                if (EndItemID > BagManager.Instance.boundaryInventory)
                                {
                                    BagManager.Instance.DeleteSameObject(EndItemID,
                                        BagManager.Instance.boundaryInventory + 1, _datasList.Count);
                                }
                                
                                if (_datasList[FirstItemID].ObjectNum >= 2)
                                    _datasList[FirstItemID].ObjectNum--;
                                else
                                    _datasList[FirstItemID] = null;
                            }
                            //物品栏范围拖拽
                            else if (FirstItemID > BagManager.Instance.boundaryInventory &&
                                     EndItemID != FirstItemID && EndItemID > BagManager.Instance.boundaryInventory)
                            {
                                _datasList[FirstItemID] = null;
                            }
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            BagManager.Instance.RefreshObject();
                        }
                        //数量为0
                        else
                        {
                            (_datasList[FirstItemID], _datasList[EndItemID]) = (
                                _datasList[EndItemID], _datasList[FirstItemID]);
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            BagManager.Instance.RefreshObject();
                        }
                        return;
                }
            }
            Transform transformTemp2;
            (transformTemp2 = transform).SetParent(OriginalParent);
            transformTemp2.position = OriginalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}

