using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Data",menuName = "ObjectData")]
public class ObjectData : ScriptableObject
{
    public string ObjectNames;
    public Sprite ObjectUI_Bag;
    public Sprite ObjectUI_Scenes;
    public string Brush_composition;
    public int ObjectNum;
    public string[] Decomposition;
    public int CanUse;//0不可用，1可用
}
