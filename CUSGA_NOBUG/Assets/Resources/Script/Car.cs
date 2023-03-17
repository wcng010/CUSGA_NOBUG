using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed;
    public float Dis = 30;
    private BoxCollider2D col;

    private Vector2 startPos;
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        startPos = transform.position;
    }

    void Update()
    {
        if (Physics2D.OverlapBox(transform.position + transform.up * 4.5f,new Vector2(col.size.x,2.5f),0,1<<LayerMask.NameToLayer("Car")) == null)
       transform.Translate(Vector3.up * speed * Time.deltaTime);

        if(Vector2.Distance(startPos,transform.position) > Dis)
        {
            Destroy(this.gameObject);
        }
    }
}
