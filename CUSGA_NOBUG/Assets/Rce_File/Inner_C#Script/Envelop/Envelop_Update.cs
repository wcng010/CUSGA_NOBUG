using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Envelop_Update : MonoBehaviour
{
    private string _sceneName;
    public Text synopsisText;
    public Text brushText;
    private void Start()
    {
        StartCoroutine(nameof(UpdateEnvelop));
    }

    IEnumerator  UpdateEnvelop()
    {
        _sceneName = SceneManager.GetActiveScene().name;
        foreach (var level in BagManager.Instance.DataListClass.EnvelopList)
        {
            if (level &&  String.Compare(level.LevelName, _sceneName, StringComparison.Ordinal)==0)
            {
                synopsisText.text = level.StoryContent;
                brushText.text = level.BrushOrder;
                break;
            }
        }
        yield return null;
    }


}
