using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float scrollLength;
    public float startPositionX;
    public float moveDistance = 0f;
    public float speed = 0.05f;

    void Start()
    {
        startPositionX = this.transform.position.x;
        scrollLength = this.GetComponent<Renderer>().bounds.size.x;
    }

    void Update()
    {
        moveDistance += speed; 
        transform.Translate(Vector2.left * speed);

        if(Mathf.Abs(moveDistance) >= scrollLength)
        {
            this.transform.localPosition = new Vector3(startPositionX, transform.position.y, transform.position.z);
            moveDistance = 0;
        }    
    }

   
}
