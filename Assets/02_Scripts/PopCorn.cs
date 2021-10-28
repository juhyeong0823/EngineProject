using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopCorn : MonoBehaviour
{
    public GameObject popCornPrefab;
    private List<GameObject> popCorns = new List<GameObject>();

    public Vector3 spawnPos;

    private void Awake()
    {
        float transition = 0;
        for (int i = 0; i < 10; i++)
        {
            transition += 0.2f;
            GameObject popcorn = Instantiate(popCornPrefab, spawnPos + new Vector3(transition, 0, 0), Quaternion.identity, this.transform); ;
            popcorn.SetActive(false);
            popCorns.Add(popcorn);
        }
    }

    public void PopCornExplosion()
    {
        foreach (var corn in popCorns)
        {
            corn.SetActive(true);
        }
        StartCoroutine(CornDrop());
    }



    IEnumerator CornDrop()
    {
        foreach (var item in popCorns)
        {
            yield return new WaitForSeconds(0.5f);

            item.SetActive(true);
        }
    }
}
