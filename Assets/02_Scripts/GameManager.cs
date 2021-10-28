using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    #region 싱글톤
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

    public List<GameObject> animObjPrefab = new List<GameObject>();
    public List<GameObject> animObj = new List<GameObject>();
    [SerializeField] private GameObject playingAnimObj;
    float changeDelay = 3f;

    private void Start()
    {
        if (animObjPrefab.Count <= 0)
        {
            Debug.Log("만들 애니메이션이 없어");
            return;
        }
        foreach (var item in animObjPrefab)
        {
            GameObject obj = Instantiate(item, this.transform);
            animObj.Add(obj);
        }
        RandomPattern();
        //StartCoroutine(ChangeColor());
    }

    public void AnimationOff()
    {
        foreach(var anim in animObj)
        {
            anim.SetActive(false);
        }
    }

    public void RandomPattern()
    {
        if (animObj.Count <= 0)
        {
            foreach (var item in animObjPrefab)
            {
                animObj.Add(item);
            }
        }

        int rand = Random.Range(0, animObj.Count - 1);
        animObj[rand].SetActive(true);
        animObj.Remove(playingAnimObj);
        playingAnimObj = animObj[rand];
    }

    public void OffPlayingAnim()
    {
        playingAnimObj.SetActive(false);
    }

    //IEnumerator ChangeColor()
    //{
    //    while (true)
    //    {
    //        Color targetColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 0.5f));

    //        float t = 0;
    //        while (true)
    //        {
    //            Color c = Color.Lerp(neonMat.GetColor("_EmissionColor"), targetColor, Time.deltaTime * changeDelay);
    //            neonMat.SetColor("_EmissionColor", c);
    //            yield return null;

    //            t += Time.deltaTime;

    //            if (t >= changeDelay) break;
    //        }
    //    }
    //}
}
