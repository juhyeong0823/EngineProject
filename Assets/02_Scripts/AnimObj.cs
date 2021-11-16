using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimObj : MonoBehaviour
{
    public void StopAnim()
    {
        Debug.Log("StopAnim");
        if (GameManager.instance.playerHealth.hp < 5)
        {
            GameManager.instance.playerHealth.hp++;
            UIManager.instance.SetHp(GameManager.instance.playerHealth.hp);
        }
       
        StartCoroutine(RestTime());
    }

    public void GroundMoveUp() => GameManager.instance.GroundMoveUp();
    public void GroundMoveDown() => GameManager.instance.GroundMoveDown();
    public void GroundMoveRight() => GameManager.instance.GroundMoveRight();
    public void GroundMoveLeft() => GameManager.instance.GroundMoveLeft();
    public void GroundMoveOriginPos() => GameManager.instance.GroundMoveOriginPos();

    IEnumerator RestTime()
    {
        if (GameManager.instance.perfectClearChecker)
        {
            GameManager.instance.PlusScore(300f);
            UIManager.instance.PlusScore_AnimEnd();
            Debug.Log("scorePlus");
        }
        yield return new WaitForSeconds(3f);

        GameManager.instance.OffPlayingAnim();
        GameManager.instance.RandomPattern();
        Debug.Log("RestTime");
    }
}
