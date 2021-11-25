using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowResult : MonoBehaviour
{
    const float S_SCORE = 10000f; // s 등급 찍기 위한 점수, 그래프상 만점
    public Image fillImage;  // 얼마나 깼는지를 그림으로 보여주기위한 용도 

    public Text gradeText; 
    public Text scoreText; 
    public Text bestScoreText;

    WaitForSeconds ws = new WaitForSeconds(0.01f); // Lerp 느낌나게

    public void ShowScore() => StartCoroutine(CoShowScore());

    public void CheckScore(float value, Text changeText)
    {
        if      (value < 0.3f) ShowGrade("F", changeText);
        else if (value < 0.4f) ShowGrade("E", changeText);
        else if (value < 0.5f) ShowGrade("D", changeText);
        else if (value < 0.6f) ShowGrade("C", changeText);
        else if (value < 0.7f) ShowGrade("B", changeText);
        else if (value < 0.8f) ShowGrade("A", changeText);
        else if (value >= 1f) ShowGrade("S", changeText);
    }

    public void ShowGrade(string grade, Text changeText)
    {
        switch (grade)
        {
            case "F":
                changeText.text = "F";
                break;
            case "D":
                changeText.text = "E";
                break;
            case "E":
                changeText.text = "D";
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
        int duration = 0; // 스코어 변화량
        int newScore = (int)GameManager.instance.score; // newScore에 지금 플레이한 게임의 점수 기록
        int remainScore = newScore; // 올려줘야 하는 점수를 remainScore로 남겨두기
        float score = 0f; // 보여줄 결과점수를 넣어둘 변수.

        GameManager.instance.saver.LoadScore(bestScoreText); // 지금 점수만 적혀있음 

        while (score < newScore )
        {
            duration = remainScore / 30; // Lerp느낌 나게
            if (duration <= 1) duration = 1; // 1보다 작으면 함수 안끝나니까 1 더해버리는거
            remainScore -= duration; // Lerp비슷
            score += duration; // 올려야 하는 점수 remain에 비례하여 스코어를 변화시킴.

            fillImage.fillAmount = Mathf.Clamp(score / S_SCORE, 0, 1); // 점수가 s등급에 비해 얼마만큼 높은지 보여주기
            scoreText.text = score.ToString(); // 스코어 변화한 수치를 텍스트에 담아주기

            yield return ws;
        }
        //날아오는 효과 만들기, 점수에 따라 등급 정해서 보여주기
        gradeText.rectTransform.localScale = new Vector3(3, 3, 1);
        gradeText.gameObject.SetActive(true);
        CheckScore(fillImage.fillAmount, gradeText);
        gradeText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InSine);
    }
}
