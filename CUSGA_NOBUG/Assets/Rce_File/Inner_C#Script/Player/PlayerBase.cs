using UnityEngine;

namespace Rce_File.Inner_C_Script.Player
{
   public class PlayerBase
   {
      private Rigidbody2D _playerRigid;
      private float _playerSpeed;
      private Animator _playerAnimator;
     
      public PlayerBase()
      { }

      public void Init_Rigid(Rigidbody2D rigidbody2D)=>this._playerRigid = rigidbody2D; 
      public void Init_Speed(float playerSpeed)=> this._playerSpeed = playerSpeed;
      
      public void Init_Animator(Animator animator) => this._playerAnimator = animator;

      public virtual void PlayerMove()
      {
         float rightwithLeft = Input.GetAxis("Horizontal");
         float upwithDown = Input.GetAxis("Vertical");
         UpdateAnimatior(upwithDown,rightwithLeft);
         _playerRigid.velocity = new Vector2(rightwithLeft, upwithDown)*_playerSpeed;
      }

      public virtual void UpdateAnimatior(float upandDown=0,float rightandLeft=0)
      {
         if(upandDown!=0||rightandLeft!=0) _playerAnimator.SetTrigger("Move");
         
         _playerAnimator.SetFloat("UpAndDown",upandDown);
         _playerAnimator.SetFloat("LeftAndRight",rightandLeft);
      }
   }
}
