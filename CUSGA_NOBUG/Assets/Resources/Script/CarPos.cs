using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPos : MonoBehaviour
{
    public GameObject car;

    [Header("随机出车时间")]
    public float minTime;
    public float maxTime;

    private float time = 0;
    private float CD;
    void Start()
    {
        CD = Random.Range(minTime, maxTime);
    }
    void Update()
    {
        if (Physics2D.OverlapBox(transform.position + transform.up, new Vector2(0.2f, 1), 0, 1 << LayerMask.NameToLayer("Car")) != null)
            return;

            time += Time.deltaTime;
        if(time > CD)
        {
            Instantiate(car,transform.position,transform.rotation);
            time = 0;
            CD = Random.Range(minTime, maxTime);
        }
    }
}
