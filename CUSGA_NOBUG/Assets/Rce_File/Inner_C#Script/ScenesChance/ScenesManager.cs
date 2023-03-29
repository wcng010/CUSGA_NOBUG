using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager :BaseManager<ScenesManager>
{
    // Start is called before the first frame update
    private string sceneName;

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(SceneManager.GetActiveScene().name=="Level1")
            SceneManager.LoadSceneAsync("Level2");
            else
            {
                SceneManager.LoadSceneAsync("Level3");
            }
        }
    }
    


    /// <summary>
    /// 用key来存数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    public void Save(object data,string key)
    { 
        var jsonData=JsonUtility.ToJson(data,true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName,SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// 通过key来取数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    
    public void Load(object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        { JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data); }
    }
}
