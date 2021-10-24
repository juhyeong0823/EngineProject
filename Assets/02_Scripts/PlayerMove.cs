using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    float x;
    float playerHeight;
    public int jumpCount = 0;
    public float speed;
    public float jumpPower;

    public bool canJump = true;

    Rigidbody2D rigid;
    SpriteRenderer sr;

    public LayerMask isGround;
    public Transform rayPoint;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GroundedCheck();
        Move();
        Jump();
    }

    void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(x * speed, rigid.velocity.y);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && canJump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    void GroundedCheck()
    {
        if (Physics2D.Raycast(rayPoint.position, Vector2.down, 1f, isGround)) jumpCount = 0;
        if (jumpCount < 2) canJump = true;
        else canJump = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Obstacle"))
        {
            Debug.Log("¾ÆÆÄ");
        }
    }
}
