using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimObj : MonoBehaviour
{
    public void StopAnim()
    {
        GameManager.instance.OffPlayingAnim();
        GameManager.instance.RandomPattern();
    }
}
