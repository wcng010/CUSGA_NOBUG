using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rce_File.Inner_C_Script.BagSystem.Operate
{
    public sealed class Drag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerClickHandler
    {
        public enum DragModel
        {
            BrushModel,
            ObjectModel
        }
    
        public DragModel dragModel;//拖动模式
        private Transform _originalParent;//起始父Transform
        private int _clickItemID;//鼠标点击链表序号
        private int _currentItemID;//OnBegin链表序号
        public ListData listClass;//DataClass链表
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
        public void OnPointerClick(PointerEventData eventData)
        {
            if (dragModel == DragModel.BrushModel)
            {

                _clickItemID = transform.parent.GetComponent<Plaid_UI>().ID;
                if (_clickItemID <= BagManager.Instance.boundaryWorkbag)
                    return;
                //数量为1情况
                if (listClass.BrushList[_clickItemID]._brushNum >= 1&&!BagManager.Instance.CorrectionFor_12B(listClass.BrushList[_clickItemID]._brushName))
                {
                    for(int i=0;i<=BagManager.Instance.boundaryWorkbag;i++)
                        if (listClass.BrushList[i] == null)
                        {
                            listClass.BrushList[i] = listClass.BrushList[_clickItemID];
                            listClass.BrushList[_clickItemID] = null;
                            BagManager.Instance.RefreshBrush();
                            return;
                        }
                }
                listClass.BrushList[_clickItemID]._brushNum++;
                listClass.BrushList[_clickItemID] = null;
                BagManager.Instance.RefreshBrush();
            }
        
        
        
            else if (dragModel == DragModel.ObjectModel)
            {
                Debug.Log(BagManager.Instance.plaidGrid.activeInHierarchy);
                _clickItemID = transform.parent.GetComponent<Object_UI>().ID;//获得当前点击的格子编号
                if (_clickItemID < BagManager.Instance.boundaryInventory)//背包内
                    return;
                if (_clickItemID > BagManager.Instance.boundaryInventory && !BagManager.Instance.plaidGrid.activeInHierarchy &&
                    listClass.ObjectList[_clickItemID].CanUse != 1)//物品栏但不可用
                    return;
                if (_clickItemID > BagManager.Instance.boundaryInventory && !BagManager.Instance.plaidGrid.activeInHierarchy&&listClass.ObjectList[_clickItemID].CanUse==1)//物品栏
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
                    return;
                }
            
                if (listClass.ObjectList[_clickItemID].ObjectNum >= 1&&!BagManager.Instance.CorrectionFor_12O(listClass.ObjectList[_clickItemID].ObjectNames))
                {
                    for(int i=0;i<BagManager.Instance.boundaryWorkbag;i++)
                        if (listClass.ObjectList[i] == null)
                        {
                            listClass.ObjectList[i] = listClass.ObjectList[_clickItemID];
                            listClass.ObjectList[_clickItemID] = null;
                            BagManager.Instance.RefreshObject();
                            return;
                        }
                }
                listClass.ObjectList[_clickItemID].ObjectNum++;
                listClass.ObjectList[_clickItemID] = null;
                BagManager.Instance.RefreshObject();
            }
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            if (dragModel == DragModel.BrushModel)
            {
                _currentItemID = _originalParent.GetComponent<Plaid_UI>().ID;
                if (_currentItemID > BagManager.Instance.boundaryWorkbag)
                    return;
                _imageName = "brush_Image";
                _plaidName = "plaid_Brush(Clone)";
            }
            else if (dragModel == DragModel.ObjectModel)
            {
                _currentItemID = _originalParent.GetComponent<Object_UI>().ID;
                if (_currentItemID >= BagManager.Instance.boundaryInventory)
                    return;
                _imageName = "object_Image";
                _plaidName = "plaid_Object(Clone)";
            }

            Transform transformTemp;
            (transformTemp = transform).SetParent(endObject);
            transformTemp.position = eventData.position;
            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_currentItemID > BagManager.Instance.boundaryWorkbag&&dragModel==DragModel.BrushModel)
                return;
            else if(_currentItemID>=BagManager.Instance.boundaryInventory&&dragModel==DragModel.ObjectModel)
                return;
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_currentItemID > BagManager.Instance.boundaryWorkbag&&dragModel==DragModel.BrushModel)
                return;
            else if(_currentItemID>=BagManager.Instance.boundaryInventory&&dragModel==DragModel.ObjectModel)
                return;
            _endGameObject = eventData.pointerCurrentRaycast.gameObject;
            if (_endGameObject != null)
            {
                if (_endGameObject.name == _imageName)//格子不为空
                {
                    if (dragModel == DragModel.BrushModel&& _endGameObject.GetComponentInParent<Plaid_UI>().ID >
                        BagManager.Instance.boundaryWorkbag)//笔画拖到合成台
                    {
                        Transform transformTemp;
                        (transformTemp = transform).SetParent(_originalParent);
                        transformTemp.position = _originalParent.position;
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        return;
                    }
                
                    if (dragModel == DragModel.ObjectModel&&_endGameObject.GetComponentInParent<Object_UI>().ID >=
                        BagManager.Instance.boundaryInventory)//字拖到分解台
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
                    if (dragModel == DragModel.BrushModel)//交换笔画
                    {
                        var temp = listClass.BrushList[_currentItemID];
                        listClass.BrushList[_currentItemID] = listClass.BrushList[_endGameObject.GetComponentInParent<Plaid_UI>().ID];
                        listClass.BrushList[_endGameObject.GetComponentInParent<Plaid_UI>().ID] = temp;

                        var parentTemp = _endGameObject.transform.parent;
                        parentTemp.position = _originalParent.position;
                        parentTemp.SetParent(_originalParent);
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        BagManager.Instance.RefreshBrush();
                    }
                
                    else if (dragModel == DragModel.ObjectModel)//交换物品
                    {
                        var temp = listClass.ObjectList[_currentItemID];
                        listClass.ObjectList[_currentItemID] 
                            = listClass.ObjectList[_endGameObject.GetComponentInParent<Object_UI>().ID];
                    
                        listClass.ObjectList[_endGameObject.GetComponentInParent<Object_UI>().ID] = temp;
                        var parentTemp2 = _endGameObject.transform.parent;
                        parentTemp2.position = _originalParent.position;
                        parentTemp2.SetParent(_originalParent);
                        GetComponent<CanvasGroup>().blocksRaycasts = true;
                        BagManager.Instance.RefreshObject();
                    }
                    return;
                }
            
                else if (eventData.pointerCurrentRaycast.gameObject.name == _plaidName)//格子为空
                {
                    Transform transformTemp;
                    (transformTemp = transform).SetParent(_endGameObject.transform);
                    transformTemp.position = _endGameObject.transform.position;
                    if (dragModel == DragModel.BrushModel)//笔画拖到空格子里，笔画拖到合成台格子。
                    {
                        _plaidID = _endGameObject.GetComponent<Plaid_UI>().ID;
                        if (listClass.BrushList[_plaidID] == null)
                        {
                            listClass.BrushList[_plaidID] = listClass.BrushList[_currentItemID];
                            if (_plaidID != _currentItemID && _plaidID <= BagManager.Instance.boundaryWorkbag)
                            {
                                listClass.BrushList[_currentItemID] = null;
                            }
                            else if (_plaidID != _currentItemID && _plaidID > BagManager.Instance.boundaryWorkbag)
                            {
                                if (_plaidID > BagManager.Instance.boundaryExchange)
                                {
                                    BagManager.Instance.ChangeCorrection();
                                }
                                if (listClass.BrushList[_currentItemID]._brushNum >= 2)
                                    listClass.BrushList[_currentItemID]._brushNum--;
                                else 
                                    listClass.BrushList[_currentItemID] = null;
                            }
                        
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            BagManager.Instance.RefreshBrush();
                            return;
                        }
                        else 
                        {
                            (listClass.BrushList[_currentItemID], listClass.BrushList[_plaidID]) = (listClass.BrushList[_plaidID], listClass.BrushList[_currentItemID]);
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            BagManager.Instance.RefreshBrush();
                            return;
                        }
                    }
                
                    else if (dragModel == DragModel.ObjectModel)
                    {
                        _objectID = _endGameObject.GetComponent<Object_UI>().ID;
                        if (listClass.ObjectList[_objectID] == null)
                        {
                            listClass.ObjectList[_objectID] = listClass.ObjectList[_currentItemID];
                            if (_objectID != _currentItemID && _objectID < BagManager.Instance.boundaryInventory)
                            {
                                listClass.ObjectList[_currentItemID] = null;
                            }
                            else if (_objectID != _currentItemID && _objectID >= BagManager.Instance.boundaryInventory)
                            {
                                if (_objectID > BagManager.Instance.boundaryInventory)
                                {
                                    BagManager.Instance.DeleteSameObject(_objectID,BagManager.Instance.boundaryInventory+1,listClass.ObjectList.Count);
                                }
                                if (listClass.ObjectList[_currentItemID].ObjectNum >= 2)
                                    listClass.ObjectList[_currentItemID].ObjectNum--;
                                else
                                    listClass.ObjectList[_currentItemID] = null;
                            }
                            GetComponent<CanvasGroup>().blocksRaycasts = true;
                            BagManager.Instance.RefreshObject();
                        }
                        else 
                        {
                            (listClass.ObjectList[_currentItemID], listClass.ObjectList[_objectID]) = (listClass.ObjectList[_objectID], listClass.ObjectList[_currentItemID]);
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
            (transformTemp= transform).SetParent(_originalParent);
            transformTemp.position = _originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
