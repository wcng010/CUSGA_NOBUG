using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagControl : MonoBehaviour
{
    public GameObject Bag;
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        bagControl();
    }

    public virtual void bagControl()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Bag.SetActive(!Bag.activeSelf);
        }
    }
}
