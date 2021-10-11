using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //하루 치 운동시간이 지나면 몸무게 빼고 다음날로 넘기거나 운동 중지
    public float moveDistance;
    public float speed;

    PlayerStatus playerStat;

    void Start()
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    void Update()
    {
        moveDistance += speed;
    }
}
