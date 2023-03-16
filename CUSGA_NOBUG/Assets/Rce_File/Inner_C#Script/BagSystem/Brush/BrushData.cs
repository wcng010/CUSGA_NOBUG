using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Data",menuName = "BrushData")]
public class BrushData:ScriptableObject
{
   public string _brushName;
   public Sprite _brushSprite;
   public int _brushNum;
}
