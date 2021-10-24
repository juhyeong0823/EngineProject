using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Completeness : MonoBehaviour
{
    public Image fillImage;
    public Image icon;
    PlayerMove playerMove;

    private float startPosX;

    private void Start()
    {
        playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
        startPosX = icon.transform.position.x;
    }

    void Update()
    {

    }
}
