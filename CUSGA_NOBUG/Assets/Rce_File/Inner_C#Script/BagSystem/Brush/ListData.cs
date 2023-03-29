using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "Data",menuName = "ListData")]
public class ListData : ScriptableObject
{
    private string _scenesName;
    public List<BrushData> brushList = new List<BrushData>();
    public List<ObjectData> objectList = new List<ObjectData>();
    public List<Envelop_Content> envelopList = new List<Envelop_Content>(); 
    public List<ObjectData> objectListBuffer1 = new List<ObjectData>();
    public List<ObjectData> objectListBuffer2 = new List<ObjectData>();
    public List<ObjectData> objectListBuffer3 = new List<ObjectData>();
    public void Start()
    {
        Debug.Log(1);
        _scenesName = SceneManager.GetActiveScene().name;
        switch (_scenesName)
        {
            case"Level1" : LoadObjectData(objectListBuffer1);break;
            case"Level2" : LoadObjectData(objectListBuffer2);break;
            case"Level3" : LoadObjectData(objectListBuffer3);break;
            default:Debug.LogError("Error");
                break;
        }
    }
    

    private void LoadObjectData(List<ObjectData> objectListBuffer)
    {
        for (int i = 0; i < objectListBuffer.Count; i++)
        {
            if (objectListBuffer[i])
            {
                objectList[i] = objectListBuffer[i];
            }
            else
            {
                Debug.LogError("Error");
            }
        }
    }


}
