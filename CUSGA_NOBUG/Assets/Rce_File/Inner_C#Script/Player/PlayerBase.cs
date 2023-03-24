using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

namespace Rce_File.Inner_C_Script.Player
{
   public class PlayerBase
   {
      private Rigidbody2D _playerRigid;
      private float _playerSpeed;
      private SpriteRenderer _playerSprite;
      
      private PlayableDirector _playerFront;
      private PlayableDirector _playerBack;
      private PlayableDirector _playerRight;
      private PlayableDirector _playerLeft;
      private List<Sprite> _playerSprites;
      private SpriteRenderer _spriteRenderer;
      public void Init_Rigid(Rigidbody2D rigidbody2D)=>this._playerRigid = rigidbody2D; 
      public void Init_Speed(float playerSpeed)=> this._playerSpeed = playerSpeed;
      
      public void Init_PlayerFront(PlayableDirector playableDirector) => this._playerFront = playableDirector;

      public void Init_PlayerBack(PlayableDirector playableDirector) => this._playerBack = playableDirector;
      
      public void Init_PlayerRight(PlayableDirector playableDirector) => this._playerRight = playableDirector;

      public void Init_PlayerLeft(PlayableDirector playableDirector) => this._playerLeft = playableDirector;

      public void Init_SpriteRenderer(SpriteRenderer playerSprite) => this._playerSprite = playerSprite;
      

      // ReSharper disable Unity.PerformanceAnalysis
      public virtual void PlayerMove()
      {
         float rightwithLeft = Input.GetAxis("Horizontal");
         float upwithDown = Input.GetAxis("Vertical");
         TimelinePlay();
         if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            upwithDown = 0;
         if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
            rightwithLeft = 0;
         _playerRigid.velocity = new Vector2(rightwithLeft, upwithDown)*_playerSpeed;
      }
      
      protected virtual void TimelinePlay()
      {
         if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.A)&&Input.GetKey(KeyCode.D))
         {
            return;
         }
         float rightwithLeft = Input.GetAxis("Horizontal");
         float upwithDown = Input.GetAxis("Vertical");
         
         if (Input.GetKey(KeyCode.S))
         {
            _playerBack.Stop();
            _playerRight.Stop();
            _playerLeft.Stop();
            _playerFront.Play();
            return;
         }
         
         if (Input.GetKeyUp(KeyCode.S))
         {
            _playerFront.Stop();
         }

         if (Input.GetKey(KeyCode.W))
         {
            _playerRight.Stop();
            _playerFront.Stop();
            _playerLeft.Stop();
            _playerBack.Play();
            return;
         }
         
         if (Input.GetKeyUp(KeyCode.W))
         {
            _playerBack.Stop();
         }

         if (Input.GetKey(KeyCode.D))
         {
            _playerFront.Stop();
            _playerBack.Stop();
            _playerLeft.Stop();
            _playerRight.Play();
            return;
         }
         
         if (Input.GetKeyUp(KeyCode.D))
         {
            _playerRight.Stop();
         }

         if (Input.GetKey(KeyCode.A))
         {
            _playerFront.Stop();
            _playerBack.Stop();
            _playerRight.Stop();
            _playerLeft.Play();
            return;
         }
         if (Input.GetKeyUp(KeyCode.A))
         {
            _playerLeft.Stop();
         }
      }
   }
}
