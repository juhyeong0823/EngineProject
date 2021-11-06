using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    public static bool canMove = false;
    public bool canJump = true;

    public int jumpCount = 0;
    public int maxJumpCount = 2;
    public float speed;
    public float jumpPower;
    public float x = 0f;

    Rigidbody2D rigid;
    SpriteRenderer sr;


    WaitForSeconds ws1 = new WaitForSeconds(0.2f); // 피격시 반짝반짝 딜레이

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }



    void Update()
    {
        if (!canMove)
        {
            Debug.Log("움직일 수 없는 상태");
            return;
        }
        GroundedCheck();
        Move();

        if (Input.GetButtonDown("Jump")) Jump();
    }

    void Move()
    {
        //x = Input.GetAxisRaw("Horizontal");
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8.5f, 8.5f), transform.position.y);
        rigid.velocity = new Vector2(x * speed, rigid.velocity.y);
    }

    public void Jump()
    {
        if (canJump)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            jumpCount++;
        }
    }

    void GroundedCheck()
    {
        if (jumpCount < maxJumpCount) canJump = true;
        else canJump = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Obstacle"))
        {
            StartCoroutine(Hitted());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }

    IEnumerator Hitted()
    {
        Color myColor = sr.color;
        for (int i = 0; i < 2; i++)
        {
            yield return ws1;
            sr.color = Color.gray; // 맞았을 때 투명하게
            yield return ws1;
            sr.color = myColor; // 다시 자시 색으로
        }
    }
}
