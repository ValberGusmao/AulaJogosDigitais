using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private bool isGround = false;
    private bool right = true;
    private Vector2 lastMove;
    private int damage = 1;

    public static event Action<int, Vector2> onAtack;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isGround)
        {
            if (right)
            {
                rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y);
            }
            else
                rigidbody2d.velocity = new Vector2(-speed, rigidbody2d.velocity.y);
            lastMove = rigidbody2d.velocity;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            onAtack.Invoke(damage, lastMove);
            right = !right; 
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            right = !right;
        }
    }
}
