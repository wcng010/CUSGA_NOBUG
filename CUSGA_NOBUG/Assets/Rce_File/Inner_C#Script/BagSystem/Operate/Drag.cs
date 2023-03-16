using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public class Drag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerClickHandler
{
    public enum DragModel
    {
        BrushModel,
        ObjectModel
    }
    
    public DragModel dragModel;//拖动模式
    private Transform originalParent;//起始父Transform
    private int ClickItemID;//鼠标点击链表序号
    private int currentItemID;//OnBegin链表序号
    public ListData listClass;//DataClass链表
    private string ImageName;
    private string plaidName;
    private GameObject endGameObject;
    public Transform endObject;
    private int plaidID;
    private int objectID;

    /// <summary>
    /// 将合成台物品放回背包
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (dragModel == DragModel.BrushModel)
        {

            ClickItemID = transform.parent.GetComponent<Plaid_UI>().plaid_ID;
            if (ClickItemID <= BagManager.Instance.boundary_workbag)
                return;
            //数量为1情况
            if (listClass.BrushList[ClickItemID]._brushNum >= 1&&!BagManager.Instance.CorrectionFor_12B(listClass.BrushList[ClickItemID]._brushName))
            {
                for(int i=0;i<=BagManager.Instance.boundary_workbag;i++)
                    if (listClass.BrushList[i] == null)
                    {
                        listClass.BrushList[i] = listClass.BrushList[ClickItemID];
                        listClass.BrushList[ClickItemID] = null;
                        BagManager.Instance.RefreshBrush();
                        return;
                    }
            }
            listClass.BrushList[ClickItemID]._brushNum++;
            listClass.BrushList[ClickItemID] = null;
            BagManager.Instance.RefreshBrush();
        }
        
        
        
        else if (dragModel == DragModel.ObjectModel)
        {
            ClickItemID = transform.parent.GetComponent<Object_UI>().Object_ID;
            if (ClickItemID < BagManager.Instance.boundary_Inventory)
                return;
            if (ClickItemID > BagManager.Instance.boundary_Inventory && !BagManager.Instance.plaidGrid.activeSelf &&
                listClass.ObjectList[ClickItemID].CanUse != 1)
                return;
            if (ClickItemID > BagManager.Instance.boundary_Inventory && !BagManager.Instance.plaidGrid.activeSelf&&listClass.ObjectList[ClickItemID].CanUse==1)
            {
                print("ssssssss");
                BagManager.Instance.useObject.Invoke();
                if (BagManager.Instance.UsedCount == 0)
                {
                    BagManager.Instance.UsedCount = 0;
                    return;
                }
                BagManager.Instance.UsedCount = 0;
                //使用道具函数
                if (listClass.ObjectList[ClickItemID].ObjectNum >= 1&&!BagManager.Instance.CorrectionFor_12O(listClass.ObjectList[ClickItemID].ObjectNames))
                {
                    for(int i=0;i<BagManager.Instance.boundary_workbag;i++)
                        if (listClass.ObjectList[i] == null)
                        {
                            listClass.ObjectList[i] = listClass.ObjectList[ClickItemID];
                            listClass.ObjectList[ClickItemID].ObjectNum -= 1;
                            listClass.ObjectList[ClickItemID] = null;
                            BagManager.Instance.RefreshObject();
                            return;
                        }
                    
                }
                listClass.ObjectList[ClickItemID] = null;
                BagManager.Instance.RefreshObject();
                return;
            }
            
            if (listClass.ObjectList[ClickItemID].ObjectNum >= 1&&!BagManager.Instance.CorrectionFor_12O(listClass.ObjectList[ClickItemID].ObjectNames))
            {
                for(int i=0;i<BagManager.Instance.boundary_workbag;i++)
                    if (listClass.ObjectList[i] == null)
                    {
                        listClass.ObjectList[i] = listClass.ObjectList[ClickItemID];
                        listClass.ObjectList[ClickItemID] = null;
                        BagManager.Instance.RefreshObject();
                        return;
                    }
            }
            listClass.ObjectList[ClickItemID].ObjectNum++;
            listClass.ObjectList[ClickItemID] = null;
            BagManager.Instance.RefreshObject();
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        if (dragModel == DragModel.BrushModel)
        {
            currentItemID = originalParent.GetComponent<Plaid_UI>().plaid_ID;
            if (currentItemID > BagManager.Instance.boundary_workbag)
                return;
            ImageName = "brush_Image";
            plaidName = "plaid_Brush(Clone)";
        }
        else if (dragModel == DragModel.ObjectModel)
        {
            currentItemID = originalParent.GetComponent<Object_UI>().Object_ID;
            if (currentItemID >= BagManager.Instance.boundary_Inventory)
                return;
            ImageName = "object_Image";
            plaidName = "plaid_Object(Clone)";
        }
        transform.SetParent(endObject);
        transform.position = eventData.position;
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItemID > BagManager.Instance.boundary_workbag&&dragModel==DragModel.BrushModel)
            return;
        else if(currentItemID>=BagManager.Instance.boundary_Inventory&&dragModel==DragModel.ObjectModel)
            return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentItemID > BagManager.Instance.boundary_workbag&&dragModel==DragModel.BrushModel)
            return;
        else if(currentItemID>=BagManager.Instance.boundary_Inventory&&dragModel==DragModel.ObjectModel)
            return;
        endGameObject = eventData.pointerCurrentRaycast.gameObject;
        if (endGameObject != null)
        {
            if (endGameObject.name == ImageName)//格子不为空
            {
                if (dragModel == DragModel.BrushModel&& endGameObject.GetComponentInParent<Plaid_UI>().plaid_ID >
                    BagManager.Instance.boundary_workbag)//笔画拖到合成台
                {
                    transform.SetParent(originalParent);
                    transform.position = originalParent.position;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }
                
                if (dragModel == DragModel.ObjectModel&&endGameObject.GetComponentInParent<Object_UI>().Object_ID >=
                    BagManager.Instance.boundary_Inventory)//字拖到分解台
                {
                    transform.SetParent(originalParent);
                    transform.position = originalParent.position;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }
                
                transform.SetParent(endGameObject.transform.parent.parent);
                transform.position = endGameObject.transform.parent.parent.position;
                if (dragModel == DragModel.BrushModel)//交换笔画
                {
                    var temp = listClass.BrushList[currentItemID];
                    listClass.BrushList[currentItemID] = listClass.BrushList[endGameObject.GetComponentInParent<Plaid_UI>().plaid_ID];
                    listClass.BrushList[endGameObject.GetComponentInParent<Plaid_UI>().plaid_ID] = temp;
                    
                    endGameObject.transform.parent.position = originalParent.position;
                    endGameObject.transform.parent.SetParent(originalParent);
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    BagManager.Instance.RefreshBrush();
                }
                
                else if (dragModel == DragModel.ObjectModel)//交换物品
                {
                    var temp = listClass.ObjectList[currentItemID];
                    listClass.ObjectList[currentItemID] 
                        = listClass.ObjectList[endGameObject.GetComponentInParent<Object_UI>().Object_ID];
                    
                    listClass.ObjectList[endGameObject.GetComponentInParent<Object_UI>().Object_ID] = temp;
                    endGameObject.transform.parent.position = originalParent.position;
                    endGameObject.transform.parent.SetParent(originalParent);
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    BagManager.Instance.RefreshObject();
                }
                return;
            }
            
            else if (eventData.pointerCurrentRaycast.gameObject.name == plaidName)//格子为空
            {
                transform.SetParent(endGameObject.transform);
                transform.position = endGameObject.transform.position;
                if (dragModel == DragModel.BrushModel)//笔画拖到空格子里，笔画拖到合成台格子。
                {
                    plaidID = endGameObject.GetComponent<Plaid_UI>().plaid_ID;
                    if (listClass.BrushList[plaidID] == null)
                    {
                        listClass.BrushList[plaidID] = listClass.BrushList[currentItemID];
                        if (plaidID != currentItemID && plaidID <= BagManager.Instance.boundary_workbag)
                        {
                            listClass.BrushList[currentItemID] = null;
                        }
                        else if (plaidID != currentItemID && plaidID > BagManager.Instance.boundary_workbag)
                        {
                            if (plaidID > BagManager.Instance.boundary_exchange)
                            {
                                BagManager.Instance.ChangeCorrection();
                            }
                            if (listClass.BrushList[currentItemID]._brushNum >= 2)
                                listClass.BrushList[currentItemID]._brushNum--;
                            else 
                                listClass.BrushList[currentItemID] = null;
                        }
                        
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        BagManager.Instance.RefreshBrush();
                        return;
                    }
                    else 
                    {
                        (listClass.BrushList[currentItemID], listClass.BrushList[plaidID]) = (listClass.BrushList[plaidID], listClass.BrushList[currentItemID]);
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            BagManager.Instance.RefreshBrush();
                            return;
                    }
                }
                
                else if (dragModel == DragModel.ObjectModel)
                {
                    objectID = endGameObject.GetComponent<Object_UI>().Object_ID;
                    if (listClass.ObjectList[objectID] == null)
                    {
                            listClass.ObjectList[objectID] = listClass.ObjectList[currentItemID];
                        if (objectID != currentItemID && objectID < BagManager.Instance.boundary_Inventory)
                        {
                            listClass.ObjectList[currentItemID] = null;
                        }
                        else if (objectID != currentItemID && objectID >= BagManager.Instance.boundary_Inventory)
                        {
                            if (objectID > BagManager.Instance.boundary_Inventory)
                            {
                                BagManager.Instance.DeleteSameObject(objectID,BagManager.Instance.boundary_Inventory+1,listClass.ObjectList.Count);
                            }
                            if (listClass.ObjectList[currentItemID].ObjectNum >= 2)
                                listClass.ObjectList[currentItemID].ObjectNum--;
                            else
                                listClass.ObjectList[currentItemID] = null;
                        }
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        BagManager.Instance.RefreshObject();
                    }
                    else 
                    {
                        (listClass.ObjectList[currentItemID], listClass.ObjectList[objectID]) = (listClass.ObjectList[objectID], listClass.ObjectList[currentItemID]);
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        BagManager.Instance.RefreshObject();
                        
                    }
                }
                return;
            }
        }
        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    
    //处理sb的Canvas的方法，我也不知道为什么能行
    public void OnEnable()
    {
        originalParent = transform.parent;
        endObject = BagManager.Instance.EndTransform;
        transform.SetParent(endObject);
        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
