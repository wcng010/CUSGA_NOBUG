using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject settingPanel;

    [Header("BeginScene")]
    [SerializeField]
    private Button startBut;
    [SerializeField]
    private Button settingBut;
    [SerializeField]
    private Button quitBut;

    [Header("otherScene")]
    [SerializeField]
    private Button ReBeginBut;
    [SerializeField]
    private Button ReStartBut;
    [SerializeField]
    private Button ReGameBut;

    [SerializeField]
    private GameObject settingEffect;
    [SerializeField]
    private PlayableDirector menuEffect;

    [SerializeField] private GameObject settingEffect1;

    public void MenuToLevel1()
    {
        StartCoroutine(MenuToBeginAtor());
    }

    IEnumerator MenuToBeginAtor()
    {
        settingEffect1.SetActive(true);

        startBut.interactable = false;
        settingBut.interactable = false;
        quitBut.interactable = false;

        yield return new WaitForSecondsRealtime(2f);
        settingEffect1.SetActive(false);
        settingPanel.SetActive(false);
        settingEffect.SetActive(true);
        TimelineManager.Instance.PassTimeline.Play();
        yield return new WaitForSecondsRealtime(4f);

        startBut.interactable = true;
        settingBut.interactable = true;
        quitBut.interactable = true;

        PlayerPrefs.SetInt("pen", 0);
        PlayerPrefs.SetInt("box", 0);
        PlayerPrefs.SetInt("book", 0);
        PlayerPrefs.SetInt("Drugstore", 0);
        SceneManager.LoadSceneAsync("Prelude");
    }


    public void OpenSetting()
    {
        StartCoroutine(OpenSettingAtor());
    }
    
    IEnumerator OpenSettingAtor()
    {
        startBut.interactable = false;
        quitBut.interactable = false;
        yield return new WaitForSecondsRealtime(2f);
        startBut.interactable = true;
        quitBut.interactable = true;
        settingPanel.SetActive(true);
        settingEffect.SetActive(false);
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameAtor());
    }

    IEnumerator ExitGameAtor()
    {
#if UNITY_EDITOR
        yield return new WaitForSecondsRealtime(2f);
        UnityEditor.EditorApplication.isPlaying = false;
#else
        yield return new WaitForSecondsRealtime(2f);
            Application.Quit();
#endif
    }

    public void SettingToMenu()
    {
        StartCoroutine(SettingToMenuAtor());
    }

    IEnumerator SettingToMenuAtor()
    {
        menuEffect.Play();

        if(SceneManager.GetActiveScene().name != "BeginScene")
        {
            ReBeginBut.interactable = false;
            ReStartBut.interactable = false;
            ReGameBut.interactable = false;
        }
       
        yield return new WaitForSecondsRealtime(2.1f);
        BgmControl.Instance.SaveMusic();

        if (SceneManager.GetActiveScene().name != "BeginScene")
        {
            ReBeginBut.interactable = true;
            ReStartBut.interactable = true;
            ReGameBut.interactable = true;
        }
       
        settingPanel.SetActive(false);
    }
    

    public void ToMenu()
    {
        StartCoroutine(ToMenuAtor());
    }

    IEnumerator ToMenuAtor()
    {
        menuEffect.Play();
        ReBeginBut.interactable = false;
        ReStartBut.interactable = false;
        ReGameBut.interactable = false;
        yield return new WaitForSecondsRealtime(2.1f);
        ReBeginBut.interactable = true;
        ReStartBut.interactable = true;
        ReGameBut.interactable = true;
        SceneManager.LoadSceneAsync("BeginScene");
    }


    public void ReStart()
    {
        StartCoroutine(ReStartAtor());
    }

    IEnumerator ReStartAtor()
    {
        menuEffect.Play();
        ReBeginBut.interactable = false;
        ReStartBut.interactable = false;
        ReGameBut.interactable = false;
        yield return new WaitForSecondsRealtime(2.1f);
        ReBeginBut.interactable = true;
        ReStartBut.interactable = true;
        ReGameBut.interactable = true;

        PlayerPrefs.SetInt("pen", 0);
        PlayerPrefs.SetInt("box", 0);
        PlayerPrefs.SetInt("book", 0);
        PlayerPrefs.SetInt("Drugstore", 0);

        if (SceneManager.GetActiveScene().name=="Level4")//第四关ReStart进入第三关
        {
            SceneManager.LoadScene("Level3");
            BagManager.Instance.LoadObjectData(BagManager.Instance.dataListClass.objectListBuffer3);
            BagManager.Instance.EnterScenes(4);
            yield break;
        }     

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

       
    }


}
