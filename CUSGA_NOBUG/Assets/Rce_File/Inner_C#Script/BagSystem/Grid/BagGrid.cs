using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;

public class BagGrid : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        BagManager.Instance.RefreshBrush();
    }
}
