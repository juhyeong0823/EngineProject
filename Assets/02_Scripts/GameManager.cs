using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region
    private static GameManager Instance = null;
    public static GameManager instance
    {
        get
        {
            if(Instance == null)
            {
                return null;
            }
            return Instance;
        }
    }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public List<GameObject> animationObj = new List<GameObject>();

    public void AnimationOff()
    {
        foreach(var anim in animationObj)
        {
            anim.SetActive(false);
        }
    }
}
