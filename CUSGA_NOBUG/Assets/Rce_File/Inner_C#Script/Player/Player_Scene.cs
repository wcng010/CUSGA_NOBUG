using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Scene : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody2D _playerRigid;
    private PlayerBase _player;
    
    
    protected virtual void OnEnable()
    {
        _playerRigid = this.GetComponent<Rigidbody2D>();
    }
    
    protected virtual void Start()
    {
       
        _player = new PlayerBase();
        _player.Init_Rigid(_playerRigid);
        _player.Init_Speed(playerSpeed);
    }
    
    protected void Update()
    {
        _player.PlayerMove();
    }
}
