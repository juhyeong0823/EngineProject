using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpBtnScripts : MonoBehaviour, IPointerDownHandler
{
    PlayerMove pMove;
    private void Awake()
    {
        pMove = GameObject.Find("Player").GetComponent<PlayerMove>();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        pMove.Jump();
        Debug.Log("JumpEvent");
    }
}
