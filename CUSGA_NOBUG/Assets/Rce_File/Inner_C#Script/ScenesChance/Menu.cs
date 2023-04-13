using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.BagSystem.Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject settingPanel;
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
        yield return new WaitForSecondsRealtime(2f);
        settingEffect1.SetActive(false);
        settingPanel.SetActive(false);
        settingEffect.SetActive(true);
        TimelineManager.Instance.PassTimeline.Play();
        yield return new WaitForSecondsRealtime(5f);
        PlayerPrefs.SetInt("pen", 0);
        PlayerPrefs.SetInt("box", 0);
        PlayerPrefs.SetInt("book", 0);
        SceneManager.LoadSceneAsync("Prelude");
    }


    public void OpenSetting()
    {
        StartCoroutine(OpenSettingAtor());
    }
    
    IEnumerator OpenSettingAtor()
    {
        yield return new WaitForSecondsRealtime(2f);
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
        yield return new WaitForSecondsRealtime(2.1f);
        settingPanel.SetActive(false);
    }
    

    public void ToMenu()
    {
        StartCoroutine(ToMenuAtor());
    }

    IEnumerator ToMenuAtor()
    {
        menuEffect.Play();
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadSceneAsync("BeginScene");
    }


    public void ReStart()
    {
        StartCoroutine(ReStartAtor());
    }

    IEnumerator ReStartAtor()
    {
        menuEffect.Play();
        yield return new WaitForSecondsRealtime(2);

        PlayerPrefs.SetInt("pen", 0);
        PlayerPrefs.SetInt("box", 0);
        PlayerPrefs.SetInt("book", 0);

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
