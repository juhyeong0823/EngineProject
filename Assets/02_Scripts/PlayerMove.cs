using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //�Ϸ� ġ ��ð��� ������ ������ ���� �������� �ѱ�ų� � ����
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
