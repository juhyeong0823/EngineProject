using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    #region 싱글톤
    private static UIManager Instance = null;
    public static UIManager instance
    {
        get
        {
            if (Instance == null)
            {
                return null;
            }
            return Instance;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public const float S_GRADE_SCORE = 10000f;

    private bool isFirstPlay = true;

    public List<Image> hpImages = new List<Image>();

    public RectTransform startPanelTextMovePos;
    public Transform hpTrm;

    public CanvasGroup resultPanel;
    public Image startPanel; //패널오브젝트임
    public CanvasGroup countdownPanel; // 321 시작!
    public CanvasGroup ingameUIPanel;

    private PlayerHeatlh player;

    public Text scoreText;
    public Text bg_gradeText;
    [Space(10)]
    public Text topText;
    public Text middleText;
    public Text bottomText;
    public Text countdownText;

    public Image hpImage;

    public Button retryBtn;
    public Button startBtn;
    public Button settingBtn;
    public Button quitBtn;

    WaitForSeconds ws = new WaitForSeconds(0.01f);
    WaitForSeconds sec = new WaitForSeconds(1f);
    WaitForSeconds halfSec = new WaitForSeconds(0.5f);

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerHeatlh>();
        ButtonsInit();
        InitGameUI();
        StartCoroutine(CoShowGrade());
        StartPanel_DOAnimPlay();
    }

    void ButtonsInit()
    {
        StartPanelButtonsOnOff(false);

        retryBtn.onClick.AddListener(() =>
        {
            resultPanel.transform.DOMoveY(resultPanel.transform.position.y + 120, 1f).SetEase(Ease.OutQuint).OnComplete(() => StartCoroutine(OnGameStart()));
        });

        startBtn.onClick.AddListener(() =>
        {
            startPanel.transform.DOMoveX(startPanel.transform.position.x + 220f, 1f);
            StartCoroutine(OnGameStart());
        });
    }

    private void StartPanelButtonsOnOff(bool on)
    {
        startBtn.interactable = on;
        settingBtn.interactable = on;
        quitBtn.interactable = on;
    }

    public void StartPanel_DOAnimPlay()
    {
        if (isFirstPlay)
        {
            topText.rectTransform.DOMoveX(startPanelTextMovePos.position.x, 0.5f).OnComplete(() =>
            middleText.rectTransform.DOMoveX(startPanelTextMovePos.position.x, 0.5f).OnComplete(() =>
            bottomText.rectTransform.DOMoveX(startPanelTextMovePos.position.x, 0.5f).OnComplete(() => StartPanelButtonsOnOff(true)
            ))); // OnCompletre 개수만큼 박아놓으면 오류는 안날겨

            isFirstPlay = false; // 이건 처음 시작할 때만 쓸 애니메이션이니까 ㅁㅁ..
        }
    }

    public void ShowResult()
    {
        ingameUIPanel.transform.DOMoveY(ingameUIPanel.transform.position.y - 35, 0.5f);
        resultPanel.transform.DOMoveY(resultPanel.transform.position.y - 120, 1f).OnComplete(() =>
        resultPanel.GetComponentInChildren<ShowResult>().ShowScore());
    }

    public void InitGameUI()
    {
        scoreText.text = "0";
        SetHp(GameManager.instance.hpCount);
    }

    void PanelOff(CanvasGroup panel)
    {
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }

    void PanelOn(CanvasGroup panel)
    {
        panel.alpha = 1;
        panel.interactable = true;
        panel.blocksRaycasts = true;
    }

    public void ImgOnOff(Image img)
    {
        img.enabled = !img.enabled;
    }

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

    public IEnumerator CoShowGrade()
    {
        while (PlayerMove.canMove)
        {
            float value = Mathf.Clamp(GameManager.instance.score / S_GRADE_SCORE, 0, 1);
            CheckScore(value, bg_gradeText);
            yield return ws;
        }
    } // 배경에 있는 등급보여주는 그거 ㅇㅇ

    public void ShowGrade() => StartCoroutine(CoShowGrade());




    public void SetHp(int hpCount)
    {
        foreach(var item in hpImages)
        {
            Destroy(item);
        }
        hpImages.Clear();
        for (int i = 0; i < hpCount; i++) // hp 이미지들 
        {
            Image img = Instantiate(hpImage, hpTrm);
            HpImageFill(img, true); // 채워진 상태 hp 추가되더라도 맞으면 색 바꿔서 맞은거 보이게
            hpImages.Add(img);
        }
    }

    public void PlusMaxHP()
    {
        Image img = Instantiate(hpImage, hpTrm);
        HpImageFill(img, true); // 채워진 상태 hp 추가되더라도 맞으면 색 바꿔서 맞은거 보이게
        hpImages.Add(img);
        player.hp++;
    }

    public void HpImageFill(Image img, bool fill)
    {
        if (fill) img.color = Color.red;
        else img.color = Color.gray;
    }

    public void SetText(Text text, string textValue)
    {
        text.text = textValue;
    }

    IEnumerator OnGameStart()
    {
        yield return sec;
        PanelOn(countdownPanel);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return halfSec;
        }
        ingameUIPanel.transform.DOMoveY(ingameUIPanel.transform.position.y + 35, 0.5f);
        yield return halfSec;
        countdownText.text = "시작!";

        PanelOff(countdownPanel);
        GameManager.instance.GameStart();
    }


}
