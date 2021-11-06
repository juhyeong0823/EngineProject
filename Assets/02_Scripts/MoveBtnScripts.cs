using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveBtnScripts : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    PlayerMove pMove;
    public float x;

    private void Start()
    {
        pMove = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pMove.x = x;
        Debug.Log("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pMove.x = 0;
        Debug.Log("OnPointerExit");
    }
}
