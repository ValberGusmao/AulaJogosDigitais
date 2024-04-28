using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleEnums;

public class Jogador : MonoBehaviour
{
    public enum CharacterStates
    {   Hit,
        Run,
        Idle,
        Dead,
        None,
        Walking
    }

    private SpriteRenderer spriteRender;
    private Rigidbody2D rigidbody2D;
    private float hitForce = 20;
    private Animator anime;

    private CharacterStates state = CharacterStates.Idle;

    public CameraController camera;
    public int jumpForce = 200;
    public int speedBase = 13;
    public int vida = 5;

    private const float timerRun = 1.25f;
    private float startTimer = 0f;
    private float timerAux = 0f;

    private float lastMove;
    private float speed;
    private float boost;

    private bool isJump;

    void Awake()
    {
        anime = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        speed = speedBase;
        Inimigo.onAtack += HitEffect;
    }

    void Move()
    {
        //TODO Quando o personagem troca de direção ele para de correr

        if (state != CharacterStates.Hit)
        {
            float moveH = Input.GetAxis("Horizontal");
            if (moveH != 0 && Math.Abs(moveH) >= Math.Abs(lastMove))
            {
                if (!isJump)
                    timerAux = Time.time;

                if ((timerAux - startTimer) >= timerRun)
                    state = CharacterStates.Run;
                else
                    state = CharacterStates.Walking;

                speed = speedBase * boost * moveH;
                if (moveH > 0)
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
            else if (!isJump)
            {
                camera.MoveCamera(Vector3.zero);
                state = CharacterStates.Idle;
                startTimer = Time.time;
                timerAux = startTimer;
            }
            MoveState();
            lastMove = moveH;
        }
    }

    void MoveState()
    {
        switch (state)
        {
            case CharacterStates.Idle:
                boost = 0; break;
            case CharacterStates.Walking:
                boost = 1; break;
            case CharacterStates.Run:
                boost = 1.6f; break;
        }
    }

    void Animation()
    {
        if (isJump)
            anime.SetBool("Jump", true);
        else
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
                    break; ;
                case CharacterStates.Dead:
                    anime.SetTrigger("Dead");
                    state = CharacterStates.None;
                    break;
            }
        }
    }

    void Jump()
    {
        if (Input.GetKey("w") && !isJump && state != CharacterStates.Hit)
        {
            rigidbody2D.AddForce(new Vector2(0, jumpForce));
            isJump = true;
        }
    }

    void HitEffect(int damage, Vector2 direcao)
    {
        //Não me pergunte porque o rigibody2d ficar null depois de reiniciar a fase durante
        //alguns frames
        if (lastMove != 0)
        {

            if (lastMove < 0)
                rigidbody2D.velocity = hitForce * Vector2.right;
            else
                rigidbody2D.velocity = hitForce * Vector2.left;
        }
        else
        {

            if (direcao.x > 0)
                rigidbody2D.velocity = hitForce * Vector2.right;
            else if (direcao.x < 0)
                rigidbody2D.velocity = hitForce * Vector2.left;

        }
        rigidbody2D.velocity += Vector2.up * hitForce / 3;
        vida -= vida <= 0 ? 0 : damage;
        state = CharacterStates.Hit;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (vida > 0)
        {
            Move();
            Jump();
        }
        else if (state != CharacterStates.None)
            state = CharacterStates.Dead;
        Animation();
    }

    void OnDestroy()
    {
        Inimigo.onAtack -= HitEffect;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anime.SetBool("Jump", false);
            state = CharacterStates.None;
            isJump = false;
        }
    }

}
