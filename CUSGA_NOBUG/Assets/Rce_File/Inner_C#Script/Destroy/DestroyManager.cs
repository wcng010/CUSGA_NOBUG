using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class DestroyManager : BaseManager<DestroyManager>
{
    [SerializeField] 
    private int destroyLevel;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += TimeToDestroy;
    }

    private void TimeToDestroy(Scene arg0, Scene arg1)
    {
        if (SceneManager.GetActiveScene().buildIndex == destroyLevel)
        {
            Destroy(gameObject);
        }
    }
}
