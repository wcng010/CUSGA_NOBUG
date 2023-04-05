using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ToLevel1()
    {
        SceneManager.LoadSceneAsync("Prelude");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void ToMenu()
    {
        SceneManager.LoadSceneAsync("BeginScene");
    }

    public void ReStart()
    {
        if (SceneManager.GetActiveScene().name=="Level4")//第四关ReStart进入第三关
        {
            SceneManager.LoadScene("Level3");
            BagManager.Instance.LoadObjectData(BagManager.Instance.dataListClass.objectListBuffer3);
            BagManager.Instance.EnterScenes(4);
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
