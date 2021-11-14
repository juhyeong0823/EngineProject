using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    public static bool canMove = false;
    public bool canJump = true;

    public int jumpCount = 0;
    public int maxJumpCount = 2;
    public float speed;
    public float jumpPower;
    public float x = 0f;
    public LayerMask groundLayer;
    public Transform groundChecker;


    Rigidbody2D rigid;
    SpriteRenderer sr;

    WaitForSeconds ws = new WaitForSeconds(0.2f); // 피격시 반짝반짝 딜레이
    WaitForSeconds sec01 = new WaitForSeconds(0.05f); // 점프때매

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!canMove) return;
        Move();
        CanJumpCheck();
        if (Input.GetButtonDown("Jump"))    Jump();

    }

    public void Jump()
    {
        if (canJump)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            jumpCount++;
        }
    }

    void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8.5f, 8.5f), transform.position.y);
        rigid.velocity = new Vector2(x * speed, rigid.velocity.y);
    }

    void CanJumpCheck()
    {
        if (Physics2D.Raycast(groundChecker.position, Vector2.down, 0.05f, groundLayer))
        {
            jumpCount = 0;
        } 
        if (jumpCount < maxJumpCount) canJump = true;
        else canJump = false;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Obstacle"))
        {
            StartCoroutine(Hitted());
            //피격소리
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
            sr.color = myColor; // 다시 자신 색으로
        }
    }
}
