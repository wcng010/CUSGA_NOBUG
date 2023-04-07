using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float[] speedMidMax;
    private Vector3 statrPos;
    private float speed;
    private bool stop;
    private bool status = true;
    private float time;
    void Start()
    {
        statrPos = transform.position;
        speed = Random.Range(speedMidMax[0], speedMidMax[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(1, 100) > 72 && status)
        {
            time = 0;
            stop = true;
            status = false;
        }
        else if(status)
        {
            status = false;
            stop = false;
        }
           

        if(!stop)
        {
            transform.Translate(new Vector3(-4, 3f, 0) * speed * Time.deltaTime);
            time += Time.deltaTime;
            if(time > speed)
            {
                status = true;
                time = 0;
            }
                
        }      
        else
        {
            transform.Translate(new Vector3(-4, 3f, 0) * -1 / 5 * speed * Time.deltaTime);
            time += Time.deltaTime;
            if (time > speed * 2/3)
            {
                stop = false;
                status = true;
                time = 0;
            }
               
        }
            

        if (Vector3.Distance(statrPos,transform.position) > 70)
            Destroy(gameObject);
    }
}
