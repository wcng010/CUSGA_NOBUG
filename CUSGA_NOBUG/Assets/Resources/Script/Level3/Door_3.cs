using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_3 : MonoBehaviour
{
    public GameObject ToLevel4;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ToLevel4.SetActive(true);
    }
}
