using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase
{
   private Rigidbody2D _playerRigid;
   private float _playerSpeed;
   public PlayerBase()
   {
      
   }

   public void Init_Rigid(Rigidbody2D rigidbody2D)=>this._playerRigid = rigidbody2D;
   

   public void Init_Speed(float playerSpeed)=> this._playerSpeed = playerSpeed;
   
   

   public virtual void PlayerMove()
   {
      float UpwithDown = Input.GetAxis("Horizontal");
      float RightwithLeft = Input.GetAxis("Vertical");
      _playerRigid.velocity = new Vector2(UpwithDown, RightwithLeft)*_playerSpeed;
   }
   
   
}
