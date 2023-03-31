using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gramdma : Ohters<Gramdma>
{
    [Header("起来的状态图")]
    public Sprite sprite;

    private SpriteRenderer sprRen;

    public GameObject ShadowSprF;
    public GameObject ShadowSprC;

    public override void Start()
    {
        base.Start();
        sprRen = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if ( inter.index == 1 && !inter.chatFrame.activeInHierarchy)
        {
            inter.index++;
        }

        FindneedObject();
        
        inter.InteractionChat();
    }

    public override void FindneedObject()
    {
        base.FindneedObject();

        if (sprRen != null && succeed >= needStrings.Length && status)
        {
            status = false;
            StartCoroutine(UseObj());
            sprRen.sprite = sprite;
            ShadowSprF.SetActive(false);
            ShadowSprC.SetActive(true);
            inter.thisFace = sprite;
        }
            
    }
}
