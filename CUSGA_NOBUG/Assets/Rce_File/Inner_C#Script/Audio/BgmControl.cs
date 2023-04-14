using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmControl : MonoBehaviour
{
    private static BgmControl instance;
    public static BgmControl Instance => instance;

    private AudioSource _bgmMusic;
    public AudioSource[] _Source;

    public Slider Music_slider;
    public Slider Source_slider;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _bgmMusic = GameObject.FindGameObjectWithTag("Bgm").GetComponent<AudioSource>();
        _bgmMusic.volume = PlayerPrefs.GetFloat("Music",1);

        if(Music_slider != null)
        Music_slider.value = PlayerPrefs.GetFloat("Music", 1);

        foreach (AudioSource source in _Source)
        {
            source.volume = PlayerPrefs.GetFloat("Source", 1);
        }

        if(Source_slider != null)
        Source_slider.value = PlayerPrefs.GetFloat("Source", 1);
    }

    private void Update()
    {
        if (Music_slider != null && _bgmMusic.volume != Music_slider.value)
        _bgmMusic.volume = Music_slider.value;

        if (_Source.Length != 0 && _Source[0].volume != Source_slider.value)
            foreach (AudioSource source in _Source)
            {
                source.volume = Source_slider.value;
            }
    }


    public void setVolume()
    {
        _bgmMusic.volume = PlayerPrefs.GetFloat("Music", 1);
        Music_slider.value = PlayerPrefs.GetFloat("Music", 1);
    }

    public void SaveMusic()
    {
        PlayerPrefs.SetFloat("Music", Music_slider.value);
        PlayerPrefs.SetFloat("Source", Source_slider.value);
    }
}
