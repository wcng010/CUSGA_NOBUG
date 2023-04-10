using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGetObj : MonoBehaviour
{
    [Header("速度")]
    public float speed = 1.2f;

    [Header("缩放最终大小")]
    public Vector3 ScaleEnd;
    [Header("开始缩放大小")]
    public Vector3 ScaleStart;

    private Vector3 Midpos;
    private Vector3 MidBotpos;

    private Vector3 startPos;
    private Vector3 endPos;

    private Sprite Tex;

    [Header("展示时间")]
    public float showTime = 1f;
    private float time = 0;
    private float time_Trl = 0;
    void Start()
    {
        Midpos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f , Screen.height / 2.0f,10));
        MidBotpos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f + 100, -50,10));
        Tex = GetComponent<SpriteRenderer>().sprite;

        ScaleStart = Vector3.one * 100f / Tex.texture.width;
        if(Tex.texture.width >= 300)
        ScaleEnd = Vector3.one * 400f / Tex.texture.width;
        else
            ScaleEnd = Vector3.one * 200f / Tex.texture.width;

        transform.localScale = ScaleStart;
        startPos = transform.position;
      
    }

    // Update is called once per frame
    void Update()
    {
        if(time < showTime)
        {
            //Midpos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 10));
           
            if (Vector3.Distance(transform.position, Midpos) > 0.1f)
            {
                time_Trl += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, Midpos, time_Trl * speed);
                transform.localScale = Vector3.Lerp(ScaleStart, ScaleEnd, time_Trl * speed);
            }
            else
            {
                time += Time.deltaTime;
                endPos = transform.position;
                time_Trl = 0;
            }
        }
        else
        {
            MidBotpos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2.0f + 100, -50, 10));

            if (Vector3.Distance(transform.position, MidBotpos) > 1f)
            {
                time_Trl += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, MidBotpos, Time.deltaTime * speed * 1.2f);
                transform.localScale = Vector3.Lerp(ScaleEnd, ScaleStart, time_Trl * speed);
            }
            else
            {
               Destroy(gameObject);
            }
        }
       
    }
}
