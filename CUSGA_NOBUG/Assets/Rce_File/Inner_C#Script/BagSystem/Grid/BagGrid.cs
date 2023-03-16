using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagGrid : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        BagManager.Instance.RefreshBrush();
    }
}
