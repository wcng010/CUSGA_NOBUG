using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.EventCenter;
using UnityEngine;
using UnityEngine.Serialization;

public class FirePile : MonoBehaviour
{
    private SpriteRenderer _sprite; 
    [SerializeField]
    private GameObject fireParticle;
    private void OnEnable()
    {
        _sprite = this.GetComponent<SpriteRenderer>();
        EventCenter.Subscribe(MyEventType.Level2End,Level2Exit4);
    }

    private void OnDisable()
    {
        EventCenter.Unsubscribe(MyEventType.Level2End,Level2Exit4);
    }

    private void Level2Exit4()
    {
        _sprite.enabled = true;
        fireParticle.SetActive(true);
    }
}
