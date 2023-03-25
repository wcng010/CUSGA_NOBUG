using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;
using TMPro;
using UnityEngine.UI;
using Object = System.Object;
public class Base_UI : MonoBehaviour
{
    [NonSerialized]
    public int ID;
    [NonSerialized]
    public string Name_item;
    [NonSerialized]
    public bool IsActive;
    [Foldout("NeedDrag",true)]
    public Image plaid;
    public GameObject iteminPlaid;
    public Text NumText;
}
