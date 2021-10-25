using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    float x;
    public int jumpCount = 0;
    public float speed;
    public float jumpPower;

    public bool canJump = true;

    Rigidbody2D rigid;
    SpriteRenderer sr;

    public LayerMask isGround;
    public Transform rayPoint;

    WaitForSeconds ws = new WaitForSeconds(0.2f);

    void Awake()
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

        if (!Physics2D.Raycast(rayPoint.position, Vector2.down, 0.2f, isGround) && rigid.velocity.y == 0)
        {
            rigid.velocity = new Vector2(0, -4);
        }
        else
        {
            rigid.velocity = new Vector2(x * speed, rigid.velocity.y);
        }
        
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && canJump)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            jumpCount++;
        }
    }

    void GroundedCheck()
    {
        if (Physics2D.Raycast(rayPoint.position, Vector2.down, 0.2f, isGround)) jumpCount = 0;
        if (jumpCount < 2) canJump = true;
        else canJump = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Obstacle"))
        {
            Debug.Log("에잉");
            StartCoroutine(Hitted());
        }
    }


    IEnumerator Hitted()
    {
        Color myColor = sr.color;
        for (int i = 0; i < 2; i++)
        {
            yield return ws;
            sr.color = Color.gray; // 맞았을 때 투명하게
            yield return ws;
            sr.color = myColor; // 다시 자시 색으로
        }
    }
}
