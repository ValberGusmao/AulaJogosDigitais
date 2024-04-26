using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

public class Jogador : MonoBehaviour
{
    public enum CharacterStates
    {
        Idle,
        Walking,
        Run,
        Jumping,
        Dead,
        None
    }

    private SpriteRenderer spriteRender;
    private Rigidbody2D rigidbody2D;
    private Animator anime;

    private CharacterStates state = CharacterStates.Idle;

    public CameraController camera;
    public int jumpForce = 200;
    public int speedBase = 13;
    public int vida = 5;

    private const float timerRun = 1.25f;
    private float startTimer = 0f;
    private float timerAux = 0f;
    private float speed;
    private float boost;
    private float lastMove;

    void Start()
    {
        anime = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        speed = speedBase;
    }

    void Move()
    {
        //TODO Quando o personagem troca de direção ele para de correr
        float moveH = Input.GetAxis("Horizontal");

        if (moveH != 0 && Math.Abs(moveH) >= Math.Abs(lastMove))
        {
            if (state != CharacterStates.Jumping)
            {
                timerAux = Time.time;
            }
            if ((timerAux - startTimer) >= timerRun)
                boost = 1.3f;
            else
                boost = 0.75f;

            speed = speedBase * boost * moveH;
            if (speed > 0)
            {
                spriteRender.flipX = false;
                camera.MoveCamera(5 * Vector3.right);
            }
            else
            {
                spriteRender.flipX = true;
                camera.MoveCamera(5 * Vector3.left);
            }
            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        }
        //Personagem está parado
        else if (state != CharacterStates.Jumping)
        {
            camera.MoveCamera(Vector3.zero);
            startTimer = Time.time;
            timerAux = startTimer;
            boost = 0;
        }
        if (state != CharacterStates.Jumping && state != CharacterStates.Dead)
            MoveState();
        lastMove = moveH;
    }

    void MoveState()
    {
        switch (boost)
        {
            case 0:
                state = CharacterStates.Idle; break;
            case 0.75f:
                state = CharacterStates.Walking; break;
            case 1.3f:
                state = CharacterStates.Run; break;
        }
    }

    void Animation()
    {
        switch (state)
        {
            case CharacterStates.Idle:
                anime.SetInteger("Move", 0);
                break;
            case CharacterStates.Walking:
                anime.SetInteger("Move", 1);
                break;
            case CharacterStates.Run:
                anime.SetInteger("Move", 2);
                break;
            case CharacterStates.Jumping:
                anime.SetBool("Jump", true);
                break;
            case CharacterStates.Dead:
                anime.SetTrigger("Dead");
                break;
        }
    }

    void Jump()
    {
        if (Input.GetKey("w") && state != CharacterStates.Jumping)
        {
            rigidbody2D.AddForce(new Vector2(0, jumpForce));
            state = CharacterStates.Jumping;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (vida > 0)
        {
            Move();
            Jump();
            if (Input.GetKey("t"))
            {
                state = CharacterStates.Dead;
            }
            Animation();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anime.SetBool("Jump", false);
            state = CharacterStates.None;
        }
    }

}
