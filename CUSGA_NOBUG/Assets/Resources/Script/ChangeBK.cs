using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeBK : MonoBehaviour
{
    public void show()
    {
        if (SceneManager.GetActiveScene().name == "Prelude" || SceneManager.GetActiveScene().name == "EndingScene")
            gameManage.Instance.showChatFrame();
    }

   public void loadScene()
    {
        if(SceneManager.GetActiveScene().name != "EndingScene")
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
