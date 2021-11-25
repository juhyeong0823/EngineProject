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


    private void OnTriggerEnter2D(Collider2D col) // 피격시, 이미 맞은 상태가 아니면 실행, 체력이 0이하가 되면 게임오버
    {
        if (col.CompareTag("Obstacle") && !isHitted)
        {
            hp--;
            StartCoroutine(Hitted());
            UIManager.instance.HpImageFill(UIManager.instance.hpImages[UIManager.instance.hpImages.Count - (hp + 1)], false); // 체력 이미지 하나 없애기
            UIManager.instance.OnHiitedEffect(); // 피격시 특정 색의 창이 나타났다 페이드아웃됌

            if (hp <= 0)
            {
                GameManager.instance.GameOver();
            }
        }
    }
    
    IEnumerator Hitted() // 피격음 출력하고 잠시 플레이어의 몸을 투명한색으로 바꿨다가 다시 원래 색으로 돌아가게 함.
    {
        SoundManager.instance.efxPlayer.Play();
        isHitted = true;
        sr.color = new Color(myColor.r, myColor.g, myColor.b, 0.5f);
        yield return new WaitForSeconds(1.5f);
        sr.color = myColor; // 다시 자신 색으로
        isHitted = false;
    }
}
