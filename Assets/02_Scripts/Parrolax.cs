using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrolax : MonoBehaviour
{
    

    public float length, startPos;
    public GameObject cam;
    public float parallaxEffect;

    

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x; // 콜라이더 크기 ? 그냥  그림의 사이즈라고 보자
    }


    private void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));

        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
