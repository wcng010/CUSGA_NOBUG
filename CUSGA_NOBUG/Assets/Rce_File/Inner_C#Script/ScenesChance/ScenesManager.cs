using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager :BaseManager<ScenesManager>
{
    // Start is called before the first frame update
    private int sceneNum=3;
    //TODO:NeedToDelete
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(sceneNum++);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("Level3");
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("Level1");
    }


}
