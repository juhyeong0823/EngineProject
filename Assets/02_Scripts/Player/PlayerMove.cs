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
    

    WaitForSeconds ws = new WaitForSeconds(0.2f); // ÇÇ°Ý½Ã ¹ÝÂ¦¹ÝÂ¦ µô·¹ÀÌ

    void Awake()
    {
        SpriteRenderer sr;
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
        if (y < 0)
        {
            Debug.Log("¾Æ·§Á¡ÇÁ");
            RaycastHit2D hit = Physics2D.Raycast(groundChecker.position, Vector2.down);

            if(hit.collider != null && hit.transform.name != "BaseGround")
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

    IEnumerator DownJump(BoxCollider2D col) // ¾Æ·§Á¡ÇÁ
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }

    void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
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



}
