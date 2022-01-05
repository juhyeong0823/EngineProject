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
    public float y = 0f;
    public LayerMask groundLayer;
    public Transform groundChecker;


    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove) return;
        Move();
        CanJumpCheck();
        if (Input.GetButtonDown("Jump")) Jump();

    }

    public void Jump()
    {
        if (y < 0)
        {
            Debug.Log("아랫점프");
            RaycastHit2D hit = Physics2D.Raycast(groundChecker.position, Vector2.down);

            if (hit.collider != null && hit.transform.name != "BaseGround") // 맨 밑에 바닥에서도 내려가 버리면 문제가 생기니까
            {
                if (hit.collider.CompareTag("Ground")) StartCoroutine(DownJump(hit.transform.gameObject.GetComponent<BoxCollider2D>()));
            }
        }
        else if (canJump)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            jumpCount++;
        }

    }

    IEnumerator DownJump(BoxCollider2D col) // 플레이어 아래 오브젝트의 콜라이더를 꺼서 잠시 내려갈 수 있게 만듬
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }

    void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8.5f, 8.5f), transform.position.y); // 일정 범위내에서만 이동
        rigid.velocity = new Vector2(x * speed, rigid.velocity.y);
    }

    void CanJumpCheck()
    {
        if (Physics2D.Raycast(groundChecker.position, Vector2.down, 0.05f, groundLayer) && rigid.velocity.y <= 0)
        {
            jumpCount = 0;
        }
        if (jumpCount < maxJumpCount) canJump = true;
        else canJump = false;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("NoLimitJump"))
        {
            Debug.Log("체크");
            jumpCount = 0;
        }
    }
}
