using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float timer = 1f;

    private void Start()
    {
        Destroy(this.gameObject, timer);
    }
}
