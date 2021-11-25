using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowResult : MonoBehaviour
{
    const float S_SCORE = 10000f; // s ��� ��� ���� ����, �׷����� ����
    public Image fillImage;  // �󸶳� �������� �׸����� �����ֱ����� �뵵 

    public Text gradeText; 
    public Text scoreText; 
    public Text bestScoreText;

    WaitForSeconds ws = new WaitForSeconds(0.01f); // Lerp ��������

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
        int duration = 0; // ���ھ� ��ȭ��
        int newScore = (int)GameManager.instance.score; // newScore�� ���� �÷����� ������ ���� ���
        int remainScore = newScore; // �÷���� �ϴ� ������ remainScore�� ���ܵα�
        float score = 0f; // ������ ��������� �־�� ����.

        GameManager.instance.saver.LoadScore(bestScoreText); // ���� ������ �������� 

        while (score < newScore )
        {
            duration = remainScore / 30; // Lerp���� ����
            if (duration <= 1) duration = 1; // 1���� ������ �Լ� �ȳ����ϱ� 1 ���ع����°�
            remainScore -= duration; // Lerp���
            score += duration; // �÷��� �ϴ� ���� remain�� ����Ͽ� ���ھ ��ȭ��Ŵ.

            fillImage.fillAmount = Mathf.Clamp(score / S_SCORE, 0, 1); // ������ s��޿� ���� �󸶸�ŭ ������ �����ֱ�
            scoreText.text = score.ToString(); // ���ھ� ��ȭ�� ��ġ�� �ؽ�Ʈ�� ����ֱ�

            yield return ws;
        }
        //���ƿ��� ȿ�� �����, ������ ���� ��� ���ؼ� �����ֱ�
        gradeText.rectTransform.localScale = new Vector3(3, 3, 1);
        gradeText.gameObject.SetActive(true);
        CheckScore(fillImage.fillAmount, gradeText);
        gradeText.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InSine);
    }
}
