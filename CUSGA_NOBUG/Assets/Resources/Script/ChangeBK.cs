using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ChangeBK : MonoBehaviour
{
    [SerializeField] 
    private float passTime;
    public void show()
    {
        if (SceneManager.GetActiveScene().name == "Prelude" || SceneManager.GetActiveScene().name == "EndingScene")
            gameManage.Instance.showChatFrame();
    }

   public void loadScene()
   {
       if (SceneManager.GetActiveScene().name == "Level4")
           SceneManager.LoadScene("Level3");
       else if(SceneManager.GetActiveScene().name != "EndingScene")
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       else
       {
           SceneManager.LoadScene(0);
       }
   }
   
   IEnumerator RceChangeEffect()
   {
       TimelineManager.Instance.PassTimeline.Play();
       yield return new WaitForSecondsRealtime(passTime);
       if (SceneManager.GetActiveScene().name == "Level4")
           SceneManager.LoadScene("Level3");
       else if(SceneManager.GetActiveScene().name != "EndingScene")
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       else
       {
           SceneManager.LoadScene(0);
       }
   }

   public void loadSceneNew()
   {
       StartCoroutine(RceChangeEffect());
   }


}
