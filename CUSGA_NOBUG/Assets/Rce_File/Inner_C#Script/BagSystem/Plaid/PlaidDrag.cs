using System;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rce_File.Inner_C_Script.BagSystem.Plaid
{
    public sealed class PlaidDrag : BaseDrag<BrushData> 
    {
        public override void OnEnable()
        {
            base.OnEnable();
            _datasList = listClass.BrushList;
        }
        
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            //获得当前点击的格子编号
            GetID<Plaid_UI>(ref FirstItemID,transform.parent);
            //背包内的格子没有点击事件
            if (FirstItemID <= BagManager.Instance.boundaryWorkbag)
                return;
            //当背包里没有对应格子Brush，找到一个空的格子，将当前Brush赋给它
            if (_datasList[FirstItemID]._brushNum >= 1 &&
                !BagManager.Instance.CorrectionFor_12B(_datasList[FirstItemID]._brushName))
            {
                for (int i = 0; i <= BagManager.Instance.boundaryWorkbag; i++)
                    if (!_datasList[i])
                    {
                        _datasList[i] = _datasList[FirstItemID];
                        _datasList[FirstItemID] = null;
                        BagManager.Instance.RefreshBrush();
                        return;
                    }
            }
            //将合成台和交换台的物品放回背包
            _datasList[FirstItemID]._brushNum++;
            _datasList[FirstItemID] = null;
            BagManager.Instance.RefreshBrush();
        }





        public override void OnBeginDrag(PointerEventData eventData)
        {
            
            OriginalParent = transform.parent;
            GetID<Plaid_UI>(ref FirstItemID,OriginalParent);
            //除背包外格子不能拖
            if (FirstItemID > BagManager.Instance.boundaryWorkbag)
                return;
            //分辨是空格子还是有物品
            ImageName = "brush_Image";
            PlaidName = "plaid_Brush(Clone)";
            Transform transformTemp;
            (transformTemp = transform).SetParent(endObject);
            transformTemp.position = eventData.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            //除Brush背包外格子不能拖
            if (FirstItemID > BagManager.Instance.boundaryWorkbag)
                return;
            transform.position = eventData.position;
        }
 
        public override void OnEndDrag(PointerEventData eventData)
        {
            
            //除Brush背包外格子不能拖
            if (FirstItemID > BagManager.Instance.boundaryWorkbag)
                return;
            //_endGameObject为后格子
            EndGameObject = eventData.pointerCurrentRaycast.gameObject;
            //在场景中拖动物品，不处理物品栏之间

            //背包内处理
            if (EndGameObject)
            {
                GetID<Plaid_UI>(ref EndItemID,EndGameObject.transform);
                //格子不为空
                if (String.Compare(EndGameObject.name,ImageName,StringComparison.Ordinal)==0)
                {
                    
                    //笔画拖到合成台
                    if ( EndItemID >= BagManager.Instance.boundaryWorkbag) 
                    {
                        Transform transformTemp;
                        (transformTemp = transform).SetParent(OriginalParent);
                        transformTemp.position = OriginalParent.position;
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        return;
                    }
                    
                    Transform transformTemp1;
                    var parent1 = EndGameObject.transform.parent;
                    var parent = parent1;
                    var parentTemp1 = parent.parent;
                    (transformTemp1 = transform).SetParent(parentTemp1);
                    transformTemp1.position = parentTemp1.position;
                    

                    (_datasList[FirstItemID], _datasList[EndItemID]) = (_datasList[EndItemID], _datasList[FirstItemID]);

                    var parentTemp = parent1;
                    parentTemp.position = OriginalParent.position;
                    parentTemp.SetParent(OriginalParent);
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    BagManager.Instance.RefreshBrush();
                        
                }

                
                else if (String.Compare(EndGameObject.name,PlaidName,StringComparison.Ordinal)==0) //格子为空
                {
                    Transform transformTemp;
                    (transformTemp = transform).SetParent(EndGameObject.transform);
                    transformTemp.position = EndGameObject.transform.position;
                    //笔画拖到空格子里，笔画拖到合成台格子。
                    if (!_datasList[EndItemID])//空格子
                    {
                        //将结束格子=开始格子
                        _datasList[EndItemID] = _datasList[FirstItemID];
                        //背包内部笔画格子交换
                        if (EndItemID != FirstItemID && EndItemID <= BagManager.Instance.boundaryWorkbag)
                        {
                            _datasList[FirstItemID] = null;
                        }
                        //移到背包外格子，笔画数量-1
                        else if (EndItemID != FirstItemID && EndItemID > BagManager.Instance.boundaryWorkbag)
                        {
                            if (EndItemID > BagManager.Instance.boundaryExchange)
                            {
                                BagManager.Instance.ChangeCorrection();
                            }

                            if (_datasList[FirstItemID]._brushNum >= 2)
                                _datasList[FirstItemID]._brushNum--;
                            else
                                _datasList[FirstItemID] = null;
                        }
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        BagManager.Instance.RefreshBrush();
                        return;
                    }
                    //数量为0
                    (_datasList[FirstItemID], _datasList[EndItemID]) = (
                        _datasList[EndItemID], _datasList[FirstItemID]);
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    BagManager.Instance.RefreshBrush();
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
