using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaScript : MonoBehaviour
{
    public GameObject bananaPrefab;
    public Transform spawnTrm;

    public void DropBanana()
    {
        StartCoroutine(CoDropBanana());
    }

    IEnumerator CoDropBanana()
    {
        for (int i = 0; i < 25; i++) 
        {
            for(int j = 0; j < 2; j++)
            {
                GameObject obj = Instantiate(bananaPrefab, spawnTrm);
                int rand = Random.Range(-8, 8);

                obj.transform.position = new Vector3(rand, 4, 0);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
