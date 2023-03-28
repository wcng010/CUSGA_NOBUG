using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseDrag<R> : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler where R :BaseData 
{
    protected Transform OriginalParent; //起始父Transform
    protected int FirstItemID; //OnBegin链表序号
    protected int EndItemID;
    protected string ImageName;
    protected string PlaidName;
    protected GameObject EndGameObject;
    public Transform endObject;
    public ListData listClass; //DataClass链表
    
    protected List<R> _datasList;
    
    public virtual void OnEnable()
    {
        OriginalParent = transform.parent;
        endObject = BagManager.Instance.endTransform;
        transform.SetParent(endObject);
        Transform transformTemp;
        (transformTemp = transform).SetParent(OriginalParent);
        transformTemp.position = OriginalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    
    protected void GetID<T>(ref int id,Transform transTemp)where T:Base_UI
    {
        id = transTemp.GetComponent<T>().ID;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }



    protected void SetDataList<T>(T baseData) where T : BaseData
    {
        
    }
}
