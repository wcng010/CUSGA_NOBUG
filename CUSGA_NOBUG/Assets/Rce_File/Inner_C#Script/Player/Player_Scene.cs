using System;
using System.Collections;
using System.Collections.Generic;
using Pixeye.Unity;
using Rce_File.Inner_C_Script.Player;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class Player_Scene : MonoBehaviour
{
    private Rigidbody2D _playerRigid;
    private PlayerBase _player;
    private SpriteRenderer _playerSpriteRenderer;
    public SpriteRenderer ShadowSpriteRenderer;
    [Foldout("PlayerTimeline",true)]
    public PlayableDirector PlayerFront;
    public PlayableDirector PlayerBack;
    public PlayableDirector PlayerRight;
    public PlayableDirector PlayerLeft;
    [FormerlySerializedAs("PlayerSprites")] [Header("角色贴图")]
    public Sprite[] playerSprites = new Sprite[4];
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
        SwitchStates();
        ShadowSpriteRenderer.sprite = _playerSpriteRenderer.sprite;
    }
    
    private void ComponentGet()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
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
        _player.Init_SpriteRenderer(_playerSpriteRenderer);
    }

    private void SwitchStates()
    {
        float rightwithLeft = Input.GetAxis("Horizontal");
        float upwithDown = Input.GetAxis("Vertical");
        if (Input.GetKeyUp(KeyCode.S)&&upwithDown<-0.4)
        {
            SetSprite(playerSprites[0]);
        }
        
        if (Input.GetKeyUp(KeyCode.W))
        {
            SetSprite(playerSprites[1]);
        }
        
        if (Input.GetKeyUp(KeyCode.D))
        {
            SetSprite(playerSprites[2]);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            SetSprite(playerSprites[3]);
        }
    }
    public virtual void SetSprite(Sprite playerSprite)
    {
        _playerSpriteRenderer.sprite=playerSprite;
    }
}

