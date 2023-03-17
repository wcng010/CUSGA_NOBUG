using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum carDir
{
    type1,
    type2,
    type3,
    type4
}

public class CarPos : MonoBehaviour
{
    public GameObject car;
    public carDir dir;

    [Header("随机出车时间")]
    public float minTime;
    public float maxTime;

    private float time = 0;
    private float CD;

    private GameObject Car;
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
            switch (dir)
            {
                case carDir.type1:
                    
                    break;
                case carDir.type2:
                    break;
                case carDir.type3:
                    Instantiate(car, transform.position, transform.rotation);
                    break;
                case carDir.type4:
                    Car = Instantiate(car, transform.position, transform.rotation);
                    Car.transform.localScale = new Vector3(-0.5f,0.5f,0.5f);
                    break;
                default:
                    break;
            }
            
            time = 0;
            CD = Random.Range(minTime, maxTime);
        }
    }
}
