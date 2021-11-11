using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimObj : MonoBehaviour
{

    public void StopAnim()
    {
        GameManager.instance.OffPlayingAnim();
        GameManager.instance.RandomPattern();
    }

    public void GroundMoveUp() => GameManager.instance.GroundMoveUp();
    public void GroundMoveDown() => GameManager.instance.GroundMoveDown();
    public void GroundMoveRight() => GameManager.instance.GroundMoveRight();
    public void GroundMoveLeft() => GameManager.instance.GroundMoveLeft();
    public void GroundMoveOriginPos() => GameManager.instance.GroundMoveOriginPos();
}
