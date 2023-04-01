using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using Pixeye.Unity;
using Rce_File.Inner_C_Script.EventCenter;
using Rce_File.Inner_C_Script.Player;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class Player_Scene : MonoBehaviour
{
    private Rigidbody2D _playerRigid;
    private PlayerBase _player;
    private SpriteRenderer _playerSpriteRenderer;
     public SpriteRenderer shadowSpriteRenderer;
     [Foldout("PlayerTimeline",true)]
    public PlayableDirector playerFront;
    public PlayableDirector playerBack;
    public PlayableDirector playerRight;
    public PlayableDirector playerLeft;
    public PlayableDirector levelExit1;
    public PlayableDirector levelExit2;
    
    [FormerlySerializedAs("PlayerSprites")] [Header("角色贴图")]
    public Sprite[] playerSprites = new Sprite[4];
    [Header("玩家速度")]
    public float playerSpeed;
    private Animator _animator;
    private PlayableGraph _playableGraph;
    public AnimationClip level2Move;
    private bool _animationState=false;
    
    protected virtual void OnEnable()
    {
        ComponentGet();
        DontDestroyOnLoad(this);
        EventCenter.Subscribe(MyEventType.Level2End,Level2Exit);
    }

    protected void OnDisable()
    {
        EventCenter.Unsubscribe(MyEventType.Level2End,Level2Exit);
    }

    protected virtual void Start()
    {
        PlayerInit();
        _animator = GetComponent<Animator>();
    }
    
    protected void Update()
    {
        if (_animationState) return;
        _player.PlayerMove();
        SwitchStates();
        shadowSpriteRenderer.sprite = _playerSpriteRenderer.sprite;
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
        _player.Init_PlayerFront(playerFront);
        _player.Init_PlayerBack(playerBack);
        _player.Init_PlayerRight(playerRight);
        _player.Init_PlayerLeft(playerLeft);
        _player.Init_SpriteRenderer(_playerSpriteRenderer);
    }

    private void SwitchStates()
    {
        float rightwithLeft = Input.GetAxis("Horizontal");
        float upwithDown = Input.GetAxis("Vertical");
        if (Input.GetKeyUp(KeyCode.S))
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

    private void Level2Exit()
    {
        StartCoroutine(level2Exit());
        _animationState = true;
        levelExit1.Play();
        AnimationPlayableUtilities.PlayClip(_animator, level2Move, out _playableGraph);
    }

    IEnumerator level2Exit()
    {
        yield return new WaitForSecondsRealtime(5.5f);
        Debug.Log(1);
        SetSprite(playerSprites[0]);
        yield return new WaitForSecondsRealtime(3f);
        levelExit2.Play();
        _playableGraph.Destroy();
        yield return null;
    }
}

