using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmControl : MonoBehaviour
{
    
    private AudioSource _bgmSource;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _bgmSource = GameObject.FindWithTag("Bgm").GetComponent<AudioSource>();
    }

    private void Update()
    {
        _bgmSource.volume = _slider.value;
    }
}
