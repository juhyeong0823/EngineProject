using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaScript : MonoBehaviour
{
    public GameObject bananaPrefab;

    public void DropBanana()
    {
        
    }

    IEnumerator CoDropBanana()
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject obj = Instantiate(bananaPrefab, this.transform);
            int rand = Random.Range(-8, 8);

            obj.transform.position = new Vector3(4, rand, 0);

            yield return new WaitForSeconds(0.3f);
        }
    }

}
