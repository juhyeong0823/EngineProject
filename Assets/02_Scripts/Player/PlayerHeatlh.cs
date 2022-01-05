using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeatlh : MonoBehaviour
{
    public int hp;
    public bool isHitted = false;
    SpriteRenderer sr = null;
    Color myColor;
    void Start()
    {
        hp = GameManager.instance.hpCount;
        sr = GetComponent<SpriteRenderer>();
        myColor = sr.color;
    }


    private void OnTriggerEnter2D(Collider2D col) // �ǰݽ�, �̹� ���� ���°� �ƴϸ� ����, ü���� 0���ϰ� �Ǹ� ���ӿ���
    {
        if (col.CompareTag("Obstacle") && !isHitted)
        {
            hp--;
            StartCoroutine(Hitted());
            UIManager.instance.HpImageFill(UIManager.instance.hpImages[UIManager.instance.hpImages.Count - (hp + 1)], false); // ü�� �̹��� �ϳ� ���ֱ�
            UIManager.instance.OnHiitedEffect(); // �ǰݽ� Ư�� ���� â�� ��Ÿ���� ���̵�ƿ��

            if (hp <= 0)
            {
                GameManager.instance.GameOver();
            }
        }
    }
    
    IEnumerator Hitted() // �ǰ��� ����ϰ� ��� �÷��̾��� ���� �����ѻ����� �ٲ�ٰ� �ٽ� ���� ������ ���ư��� ��.
    {
        SoundManager.instance.efxPlayer.Play();
        isHitted = true;
        sr.color = new Color(myColor.r, myColor.g, myColor.b, 0.5f);
        yield return new WaitForSeconds(1.5f);
        sr.color = myColor; // �ٽ� �ڽ� ������
        isHitted = false;
    }
}
