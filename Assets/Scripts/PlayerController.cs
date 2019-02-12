﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 4f;
    public float speed = 1f;
    public float jumpPower = 0.11f;
    [HideInInspector] public bool onGround;
    bool jump;
    bool dJump;

    float horizontal;

    Animator animCtr;
    Rigidbody2D rb;
    SpriteRenderer spr;



	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animCtr = gameObject.GetComponent<Animator>();
        spr = gameObject.GetComponent<SpriteRenderer>();
    }
	

	void Update () {

        horizontal = Input.GetAxis("Horizontal");

        Jump();

        SetWalkAnim();

        FlipSprite();
    }

    private void FixedUpdate()
    {
        //evitar deslizamiento por el material {
        Vector3 fixedVel = rb.velocity;
        fixedVel.x *= 0.75f;
        if (onGround)
        {
            rb.velocity = fixedVel;
        }
        //}

        rb.AddForce(Vector2.right * speed * horizontal, ForceMode2D.Force);

        ClampearVel();

        if(jump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpPower);
            jump = false;
            
        }
    }

    void Jump()
    {
        if (onGround) //Salto de precaucion al caer
        {
            dJump = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                jump = true;
                dJump = true;
            }
            else if (dJump) //doble salto
            {
                jump = true;
                dJump = false;
            }
        }
    }

    void SetWalkAnim() //Animacion de andar
    {
        if (horizontal != 0 && onGround)
        {
            animCtr.SetBool("Walking", true);
        }
        else
        {
            animCtr.SetBool("Walking", false);
        }
    }

    void FlipSprite() //Dar la vuelta al sprite segun la direccion
    {
        if (horizontal > 0.1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (horizontal < -0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void ClampearVel()  //limitar velocidad
    {
        float speedClamped = Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);
        rb.velocity = new Vector2(speedClamped, rb.velocity.y);
    }

    private void OnBecameInvisible() //fuera de la camara
    {
        transform.position = new Vector3(-5.69f, -1.36f, 0f);
    }
    public void EnemyJump()
    {
        jump = true;
    }

    public void EnemyKnockBack(float enemyPosX)
    {
        jump = true;

        float side = Mathf.Sign(enemyPosX - transform.position.x);
        rb.AddForce(Vector2.left * jumpPower * side * 2, ForceMode2D.Impulse);

        Invoke("EnableColor", 1f);

        Color miColor = new Color(255/255f, 106/255f, 0/255f); //color personalizado rgb pero se necesitan valores tanto por 1.
        spr.color = miColor;
    }

    void EnableColor()
    {
        spr.color = Color.white;
    }
}
