using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : Ohters<TrafficLight>
{
    public GameObject[] stopCar;

    public Sprite sprite;

    [HideInInspector]
    public bool IsOk = false;

    private SpriteRenderer sprRen;

    public override void Start()
    {
        base.Start();
        sprRen = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(IsOk)
        {
            FindneedObject();

            inter.InteractionChat();

            if (inter.index == 1 && Input.GetKeyDown(KeyCode.F))
            {
                sprRen.sprite = sprite;
                stopCar[0].layer = LayerMask.NameToLayer("Car");
                stopCar[0].GetComponent<Collider2D>().isTrigger = true;

                stopCar[1].layer = LayerMask.NameToLayer("Car");
                stopCar[1].GetComponent<Collider2D>().isTrigger = true;

                StartCoroutine(UseObj());
                inter.index++;
                //结束
            }
        }       
    }
}
