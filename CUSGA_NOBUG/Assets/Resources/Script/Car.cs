using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed;
    public float Dis = 30;
    private BoxCollider2D col;

    private Vector2 startPos;
    private AudioSource carAudio;
    void Start()
    {
        carAudio = GetComponent<AudioSource>();
        col = GetComponent<BoxCollider2D>();
        startPos = transform.position;
    }

    void Update()
    {
        if(BgmControl.Instance.Source_slider.value != carAudio.volume)
            carAudio.volume = BgmControl.Instance.Source_slider.value;

        if (Physics2D.OverlapBox(transform.position + transform.up * 4.5f, new Vector2(col.size.x, 2f), 0, 1 << LayerMask.NameToLayer("Car")) == null)
        {
            if(!carAudio.isPlaying)
                carAudio.Play();

            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }            
        else
            carAudio.Stop();

        if(Vector2.Distance(startPos,transform.position) > Dis)
        {
            Destroy(this.gameObject);
        }
    }
}
