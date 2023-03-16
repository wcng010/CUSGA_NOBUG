using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collision : MonoBehaviour
{
   private SpriteRenderer Sprite;

   protected void OnEnable()
   {
      Sprite = this.GetComponent<SpriteRenderer>();
   }

   private void OnTriggerEnter2D(Collider2D collider2D)
   {
      if (System.String.Compare(collider2D.tag, "Player", StringComparison.Ordinal) == 0)
      {
         Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, Sprite.color.a/2);
      }
   }

   private void OnTriggerExit2D(Collider2D collider2D)
   {
      if (System.String.Compare(collider2D.tag, "Player", StringComparison.Ordinal) == 0)
      {
         Sprite.color = new Color(Sprite.color.r, Sprite.color.g, Sprite.color.b, Sprite.color.a*2);
      }
   }
}
