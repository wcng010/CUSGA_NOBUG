using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Obj
{
    pen,
    box,
    book,
}

public class SceneObj : MonoBehaviour
{
    public Obj obj;

    private void Start()
    {
        switch (obj)
        {
            case Obj.pen:
                if(PlayerPrefs.GetInt("pen",0) == 1)
                    Destroy(gameObject);
                break;
            case Obj.box:
                if (PlayerPrefs.GetInt("box", 0) == 1)
                    Destroy(gameObject);
                break;
            case Obj.book:
                if (PlayerPrefs.GetInt("book", 0) == 1)
                    Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        switch (obj)
        {
            case Obj.pen:
                PlayerPrefs.SetInt("pen", 1);
                break;
            case Obj.box:
                PlayerPrefs.SetInt("box", 1);
                break;
            case Obj.book:
                PlayerPrefs.SetInt("book", 1);
                break;
            default:
                break;
        }
    }
}
