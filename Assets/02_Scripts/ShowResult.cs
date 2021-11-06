using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowResult : MonoBehaviour
{
    float score = 0f;
    const float S_SCORE = 10000f;
    public Image fillImage;
    public Image icon;

    public Text gradeText;
    public Text scoreText;

    WaitForSeconds ws = new WaitForSeconds(0.01f);

    public void ShowScore() => StartCoroutine(CoShowScore());

    public IEnumerator CoShowScore()
    {
        int duration = 0;
        int maxScore = (int)GameManager.instance.score;
        int remainScore = maxScore;
        float iconPosY = icon.rectTransform.anchoredPosition.y;

        while (score < maxScore )
        {
            duration = remainScore / 30; // Lerp���� ����
            //gradeText.rectTransform.Rotate(new Vector3(0, duration, 0)); // ���ư��� ȿ��
            if (duration <= 1) duration = 1; // 1���� ������ �Լ� �ȳ����ϱ� 1 ���ع����°�
            remainScore -= duration; // Lerp���
            score += duration;

            fillImage.fillAmount = Mathf.Clamp(score / S_SCORE, 0, 1);
            scoreText.text = score.ToString();

            Vector2 iconMovePos = new Vector2(icon.rectTransform.anchoredPosition.x, iconPosY + fillImage.rectTransform.rect.height * fillImage.fillAmount); // ���� �׸� ��
            icon.rectTransform.anchoredPosition = iconMovePos;
            yield return ws;
        }
        gradeText.gameObject.SetActive(true);
        UIManager.instance.CheckScore(fillImage.fillAmount, gradeText);
        gradeText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InSine).OnComplete(() => Debug.Log("Done"));

        UIManager.instance.InitGameUI();
        GameManager.instance.InitGameData();
    }
}
