using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;
using DG.Tweening;

public class Test : MonoBehaviour
{
    RectTransform playButton;

    IEnumerator TestFunc()
    {
        while (true)
        {

            Tweener tweener = playButton.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 1f).OnComplete(() =>
            {
                Tweener tween = playButton.DOScale(new Vector3(1f, 1f, 1f), 1f);
            });

            yield return new WaitForSeconds(4f);

        }
    }
}
