using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data",menuName = "ListData")]
public class ListData : ScriptableObject
{
    public List<BrushData> BrushList = new List<BrushData>();
    public List<ObjectData> ObjectList = new List<ObjectData>();
    public List<Envelop_Content> EnvelopList = new List<Envelop_Content>();
}
