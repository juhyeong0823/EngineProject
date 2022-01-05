using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LollipopAttack : MonoBehaviour
{
    public GameObject bullet;
    public RectTransform bulletPos; // 처음 위치
    WaitForSeconds ws= new WaitForSeconds(1.5f);


    public void Attack() // 애니메이션 이벤트 함수
    {
        StartCoroutine(CircleShoot());
    }

    IEnumerator CircleShoot() //  4~7개 랜덤 갯수로 원형으로 쏘는 친구
    {
        for (int j = 0; j < 7; j++)
        {
            float repeatTime = Random.Range(4, 8);
            for (int i = 0; i < repeatTime; i++)
            {
                GameObject obj = Instantiate(bullet, bulletPos.anchoredPosition, Quaternion.identity,this.transform);
                obj.GetComponent<RectTransform>().localScale = new Vector3((4f / repeatTime), 4f / repeatTime, 1f); // 4개 내보내면 사이즈를 1로.
                obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(Mathf.PI * i * 2 / repeatTime), Mathf.Sin(Mathf.PI * i * 2 / repeatTime)) * 200);
            }
            yield return ws;
        }
    }
}
