using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeBK : MonoBehaviour
{
    public void show()
    {
        if (SceneManager.GetActiveScene().name == "Prelude")
            gameManage.Instance.showChatFrame();
    }

   public void loadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
