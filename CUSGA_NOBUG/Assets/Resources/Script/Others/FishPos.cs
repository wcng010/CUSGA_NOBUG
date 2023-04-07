using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPos : MonoBehaviour
{
    public GameObject fish;
    public float[] fishTime;

    private float time = 0;
    private bool status = true;
    void Update()
    {
        if(status)
        {
            time = Random.Range(fishTime[0], fishTime[1]);
            status = false;
            Instantiate(fish,transform.position,fish.transform.rotation);
        }
        else
        {
            time -= Time.deltaTime;
            if(time < 0)
            {
                status = true;
            }
        }
       
    }
}
