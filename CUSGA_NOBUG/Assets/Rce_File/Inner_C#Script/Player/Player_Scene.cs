using System;
using System.Collections;
using System.Collections.Generic;
using Pixeye.Unity;
using Rce_File.Inner_C_Script.Player;
using UnityEngine;
using UnityEngine.Playables;

public class Player_Scene : MonoBehaviour
{
    private Rigidbody2D _playerRigid;
    private PlayerBase _player;
    [Foldout("PlayerTimeline",true)]
    public PlayableDirector PlayerFront;
    public PlayableDirector PlayerBack;
    public PlayableDirector PlayerRight;
    public PlayableDirector PlayerLeft;
    [Header("玩家速度")]
    public float playerSpeed;
    protected virtual void OnEnable()
    {
        ComponentGet();
    }
    
    protected virtual void Start()
    {
            PlayerInit();
    }
    
    protected void Update()
    {
        _player.PlayerMove();
    }



    private void ComponentGet()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
    }

    private void PlayerInit()
    {
        _player = new PlayerBase();
        _player.Init_Rigid(_playerRigid);
        _player.Init_Speed(playerSpeed);
        _player.Init_PlayerFront(PlayerFront);
        _player.Init_PlayerBack(PlayerBack);
        _player.Init_PlayerRight(PlayerRight);
        _player.Init_PlayerLeft(PlayerLeft);
    }
}
