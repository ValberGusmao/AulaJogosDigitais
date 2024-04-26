using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorOLD : MonoBehaviour
{
    public float speed = 20f;
    private float move;
    private Rigidbody2D body;
    private Vector2 force;
    private bool isGrounded;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();   
    }

    //Start e Awake
    void Start()
    {
        
    }

    //FixedUpdate(Movimentos Físicos), LateUpdate(ultimo a ser chamado)
    //e Update(Inputs e todo resto)
    void FixedUpdate()
    {
        move = Input.GetAxis("Horizontal");

        //body.IsTouchingLayers()
        //force = new Vector2(move * speed * 25 * Time.deltaTime, 0);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            force = new Vector2(0, 500);
        }
        body.AddForce(force);
        transform.position += new Vector3(move * speed * Time.deltaTime, 0,0);
        force = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
