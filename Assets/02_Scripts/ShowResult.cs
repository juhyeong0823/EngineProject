using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        int maxScore = (int)UIManager.instance.score;
        int remainScore = maxScore;
        float iconPosY = icon.rectTransform.anchoredPosition.y;
        print("iconposY"+iconPosY);
        while (score < maxScore )
        {
            duration = remainScore / 30;
            if (duration <= 1) duration = 1;

            remainScore -= duration;
            score += duration;

            scoreText.text = score.ToString();
            fillImage.fillAmount = Mathf.Clamp(score / S_SCORE, 0, 1);

            Vector2 iconMovePos = new Vector2(icon.rectTransform.anchoredPosition.x, iconPosY + fillImage.rectTransform.rect.height * fillImage.fillAmount);
            print(fillImage.rectTransform.rect.height * fillImage.fillAmount);
            print("movePos" +iconMovePos);
            icon.rectTransform.anchoredPosition = iconMovePos;

            yield return ws;

            CheckScore();
        }
        Debug.Log("¿ÍÀÏ ²ý");
    }

    private void CheckScore()
    {
        if (fillImage.fillAmount < 0.3f) ShowGrade("F");
        else if (fillImage.fillAmount < 0.4f) ShowGrade("D");
        else if (fillImage.fillAmount < 0.5f) ShowGrade("E");
        else if (fillImage.fillAmount < 0.6f) ShowGrade("C");
        else if (fillImage.fillAmount < 0.7f) ShowGrade("B");
        else if (fillImage.fillAmount < 0.9f) ShowGrade("A");
        else if (fillImage.fillAmount <= 1f) ShowGrade("S");
    }
    public void ShowGrade(string grade)
    {
        switch (grade)
        {
            case "F":
                gradeText.text = "F";
                break;
            case "D":
                gradeText.text = "D";
                break;
            case "E":
                gradeText.text = "E";
                break;
            case "C":
                gradeText.text = "C";
                break;
            case "B":
                gradeText.text = "B";
                break;
            case "A":
                gradeText.text = "A";
                break;
            case "S":
                gradeText.text = "S";
                break;
        }
    }
}
