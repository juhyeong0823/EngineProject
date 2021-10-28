using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    public static bool canMove = true;
    public bool canJump = true;

    public int jumpCount = 0;
    public int maxJumpCount = 2;
    public float speed;
    public float jumpPower;

    float x = 0f;

    Rigidbody2D rigid;
    SpriteRenderer sr;
   // public ParticleSystem jumpEfx;


    WaitForSeconds ws1 = new WaitForSeconds(0.2f); // �ǰݽ� ��¦��¦ ������
    WaitForSeconds ws2 = new WaitForSeconds(0.3f); // ���� ����Ʈ ������

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        canMove = true;
    }

    private void Start()
    {
        //play on awake�� �������ִ� ��ƼŬ�� ���� �� ������..
        //jumpEfx.Stop();
    }

    void Update()
    {
        if (!canMove)
        {
            Debug.Log("������ �� ���� ����");
            return;
        }
        GroundedCheck();
        Move();
        Jump();
    }

    void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8.5f, 8.5f),transform.position.y);

        rigid.velocity = new Vector2(x * speed, rigid.velocity.y);
        
        //if (!Physics2D.Raycast(groundChecker.position, Vector2.down, 0.2f, isGround) && rigid.velocity.y == 0)
        //{
        //    rigid.velocity = new Vector2(0, -4);
        //}
        //else
        //{
        //    rigid.velocity = new Vector2(x * speed, rigid.velocity.y);
        //}
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && canJump)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            jumpCount++;
            //StopCoroutine("EfxOff");
            //jumpEfx.Play();
            //StartCoroutine(EfxOff(ws2, jumpEfx));
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
            sr.color = Color.gray; // �¾��� �� �����ϰ�
            yield return ws1;
            sr.color = myColor; // �ٽ� �ڽ� ������
        }
    }

    IEnumerator EfxOff(WaitForSeconds ws, ParticleSystem efx)
    {
        yield return ws;
        efx.Stop();
    }
}
