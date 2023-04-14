using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    void Start()
    {
        if(PlayerPrefs.GetInt("Flower",0) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("Flower", 1);
    }
}
