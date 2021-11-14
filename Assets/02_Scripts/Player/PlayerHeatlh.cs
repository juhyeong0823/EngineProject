using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeatlh : MonoBehaviour
{

    public int hp;


    void Start()
    {
        hp = GameManager.instance.hpCount;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Obstacle"))
        {
            hp--;
            UIManager.instance.HpImageFill(UIManager.instance.hpImages[UIManager.instance.hpImages.Count - (hp + 1)], false);
            UIManager.instance.OnHiitedEffect();
            if (hp <= 0)
            {
                GameManager.instance.GameOver();
            }
        }
    }
}
