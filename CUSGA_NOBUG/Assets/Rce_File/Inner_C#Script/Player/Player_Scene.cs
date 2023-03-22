using System;
using System.Collections;
using System.Collections.Generic;
using Rce_File.Inner_C_Script.Player;
using UnityEngine;

public class Player_Scene : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody2D _playerRigid;
    private PlayerBase _player;
    private Animator _playerAnimator;
    private SpriteRenderer _playerRenderer;
    public List<Sprite> SpriteList = new List<Sprite>(); 
    protected virtual void OnEnable()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerRenderer = GetComponent<SpriteRenderer>();
    }
    
    protected virtual void Start()
    {
        _player = new PlayerBase();
        _player.Init_Rigid(_playerRigid);
        _player.Init_Speed(playerSpeed);
        _player.Init_Animator(_playerAnimator);
    }
    
    protected void Update()
    {
        _player.PlayerMove();
    }
    public virtual void SpriteToForward()
    {
        _playerRenderer.sprite = SpriteList[0];
    }
    public virtual void SpriteToBack()
    {
        _playerRenderer.sprite = SpriteList[3];
    }

    public virtual void SpriteToRight()
    {
        _playerRenderer.sprite = SpriteList[2];
    }
}
