using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float scrollLength;
    private float startPositionX;
    private float moveDistance = 0f;
    private float speed = 0.05f;

    public int backgroundObjCount;


    void Start()
    {
        startPositionX = this.transform.localPosition.x;
        scrollLength = this.GetComponent<Renderer>().bounds.size.x;

    }   

    void Update()
    {
        moveDistance += speed; 
        transform.Translate(Vector2.left * speed);

        MoveOriginPos();
    }

    void MoveOriginPos()
    {
        if (Mathf.Abs(moveDistance) >= transform.GetComponentInParent<BackgroundParent>().length)
        {
            this.transform.localPosition = new Vector3(startPositionX, transform.localPosition.y, transform.localPosition.z);
            moveDistance = 0;
        }
    }

}
