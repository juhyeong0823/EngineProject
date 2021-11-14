using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeatlh : MonoBehaviour
{
    public int hp;
    public bool isHitted = false;
    public SpriteRenderer sr = null;
    Color myColor;
    void Start()
    {
        hp = GameManager.instance.hpCount;
        sr = GetComponent<SpriteRenderer>();
        myColor = sr.color;
    }

    IEnumerator NotHittable()
    {
        isHitted = true;
        yield return new WaitForSeconds(1.5f);
        isHitted = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Obstacle") && !isHitted)
        {
            hp--;
            StartCoroutine(NotHittable());
            StartCoroutine(Hitted());
            UIManager.instance.HpImageFill(UIManager.instance.hpImages[UIManager.instance.hpImages.Count - (hp + 1)], false);
            UIManager.instance.OnHiitedEffect();

            if (hp <= 0)
            {
                GameManager.instance.GameOver();
            }
        }
    }

    IEnumerator Hitted()
    {
        sr.color = Color.black; // 맞았을 때 투명하게
        yield return new WaitForSeconds(1.5f);
        sr.color = myColor; // 다시 자신 색으로
    }
}
