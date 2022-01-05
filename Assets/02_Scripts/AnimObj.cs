using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimObj : MonoBehaviour
{
    public void StopAnim() // 애니메이션 종료시 실행할 이벤트함수
    {
        Debug.Log("StopAnim");
        if (GameManager.instance.playerHealth.hp < 5 && GameManager.instance.score < 5000f)
        {
            GameManager.instance.playerHealth.hp++;
            UIManager.instance.SetHp(GameManager.instance.playerHealth.hp);
        }
       
        StartCoroutine(RestTime());
    }

    // 바닥 이동시키기
    public void GroundMoveUp() => GameManager.instance.GroundMoveUp();
    public void GroundMoveDown() => GameManager.instance.GroundMoveDown();
    public void GroundMoveRight() => GameManager.instance.GroundMoveRight();
    public void GroundMoveLeft() => GameManager.instance.GroundMoveLeft();
    public void GroundMoveOriginPos() => GameManager.instance.GroundMoveOriginPos();

    //애니메이션이 끝나고 점수올라가고, 유저에게 잠깐 쉬는 시간을 주는 함수
    IEnumerator RestTime()
    {
        if (GameManager.instance.perfectClearChecker) // 패턴 하나가 종료될 때 까지 한번도 피격되지 않으면 추가점수 부여
        {
            GameManager.instance.PlusScore(300f);
            UIManager.instance.PlusScore_AnimEnd(); // 점수 추가 UI
            Debug.Log("scorePlus");
        }
        yield return new WaitForSeconds(3f);

        GameManager.instance.OffPlayingAnim(); 
        GameManager.instance.RandomPattern();
        Debug.Log("RestTime");
    }
}
