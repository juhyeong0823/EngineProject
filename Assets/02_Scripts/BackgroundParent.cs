using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParent : MonoBehaviour
{
    public float length;

    private void Start()
    {
        Background[] backgrounds = GetComponentsInChildren<Background>();


        foreach(Background item in backgrounds)
        {
            length += item.scrollLength;
        }
    }
}
