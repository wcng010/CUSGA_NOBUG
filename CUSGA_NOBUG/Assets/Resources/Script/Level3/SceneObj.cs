using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Obj
{
    pen,
    box,
    book,
    flower,
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
            case Obj.flower:
                if (PlayerPrefs.GetInt("flower", 0) == 1)
                    Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    public void Get()
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
            case Obj.flower:
                PlayerPrefs.SetInt("flower", 1);
                break;
            default:
                break;
        }
    }
}
