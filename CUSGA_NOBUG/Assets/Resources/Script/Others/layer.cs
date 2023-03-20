using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class layer : MonoBehaviour
{
    public SpriteRenderer player;
    public SpriteRenderer other;

    public int playerLayer;
    public int otherLayer;

    private int startPlayer;
    private int startOther;

    private void Start()
    {
        startPlayer = player.sortingOrder;
        startOther = other.sortingOrder;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player.sortingOrder = playerLayer;
            other.sortingOrder = otherLayer;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.sortingOrder = startPlayer;
            other.sortingOrder = startOther;
        }
    }
}
