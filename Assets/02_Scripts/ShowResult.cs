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

    public void CheckScore(float value, Text changeText)
    {
        if (value < 0.3f) ShowGrade("F", changeText);
        else if (value < 0.4f) ShowGrade("D", changeText);
        else if (value < 0.5f) ShowGrade("E", changeText);
        else if (value < 0.6f) ShowGrade("C", changeText);
        else if (value < 0.7f) ShowGrade("B", changeText);
        else if (value < 0.9f) ShowGrade("A", changeText);
        else if (value <= 1f) ShowGrade("S", changeText);
    }

    public void ShowGrade(string grade, Text changeText)
    {
        switch (grade)
        {
            case "F":
                changeText.text = "F";
                break;
            case "D":
                changeText.text = "D";
                break;
            case "E":
                changeText.text = "E";
                break;
            case "C":
                changeText.text = "C";
                break;
            case "B":
                changeText.text = "B";
                break;
            case "A":
                changeText.text = "A";
                break;
            case "S":
                changeText.text = "S";
                break;
        }
    }

    public IEnumerator CoShowScore()
    {
        int duration = 0;
        int maxScore = (int)GameManager.instance.score;
        int remainScore = maxScore;
        float iconPosY = icon.rectTransform.anchoredPosition.y;

        while (score < maxScore )
        {
            duration = remainScore / 30; // Lerp느낌 나게
            //gradeText.rectTransform.Rotate(new Vector3(0, duration, 0)); // 돌아가는 효과
            if (duration <= 1) duration = 1; // 1보다 작으면 함수 안끝나니까 1 더해버리는거
            remainScore -= duration; // Lerp비슷
            score += duration;

            fillImage.fillAmount = Mathf.Clamp(score / S_SCORE, 0, 1);
            scoreText.text = score.ToString();

            Vector2 iconMovePos = new Vector2(icon.rectTransform.anchoredPosition.x, iconPosY + fillImage.rectTransform.rect.height * fillImage.fillAmount); // 옆에 그림 선
            icon.rectTransform.anchoredPosition = iconMovePos;
            yield return ws;
        }
        gradeText.gameObject.SetActive(true);
        CheckScore(fillImage.fillAmount, gradeText);
        gradeText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InSine).OnComplete(() => {
            UIManager.instance.InitGameUI();
            GameManager.instance.InitGameData();
        });
    }


}
