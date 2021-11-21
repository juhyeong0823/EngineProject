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

    [Space(10)]
    [Header("체력")]
    public Transform hpTrm;
    public Image hpImage;

    [Space(10)]
    [Header("점수")]
    public Text scoreText;
    public Text plusScoreText;

    [Header("패널들")]
    public CanvasGroup resultPanel;
    public Image startPanel; //패널오브젝트임
    public CanvasGroup countdownPanel; // 321 시작!
    public CanvasGroup ingameUIPanel;
    public CanvasGroup menuPanel;
    public CanvasGroup settingPanel;
    public CanvasGroup rankingPanel;
    public CanvasGroup explainPanel;
    public GameObject topBar;
    public Image hittedPanel;

    [Space(10)]
    [Header("제목")]
    public Text topText;
    public Text middleText;
    public Text bottomText;
    public Text countdownText; // 3,2,1

    [Header("시작화면")]
    public Button startBtn;  // 게임시작
    public Button quitBtn;   // 게임종료
    public Button settingBtn; // 옵션
    public Button rankingBtn;
    public Button explainShowBtn;
    public Button explainOffBtn;
    public Button rankingPanelOffBtn;

    [Space(10)]
    public Button exitBtn;   // 게임종료
    public Button retryBtn;  // 재시작
    public Button menuOnBtn; // 인게임 메뉴 열기
    public Button menuOff;
    public Button backToStartBtn;
    public Button continueBtn;

    public InputField nickname;
    
    WaitForSeconds sec = new WaitForSeconds(1f);
    WaitForSeconds halfSec = new WaitForSeconds(0.5f);

    private void Start()
    {
        StartPanelButtonsOnOff(false);
        InitButtons();
        StartPanel_DOAnimPlay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PlayerMove.canMove) return;
            PanelOn(menuPanel, !menuPanel.interactable);
            Time.timeScale = menuPanel.interactable ? 0 : 1;
        }
    }

    void InitButtons()
    {
        retryBtn.onClick.AddListener(() =>
        {
            resultPanel.transform.DOMoveY(resultPanel.transform.position.y + 120, 1f).SetEase(Ease.OutQuint).OnComplete(() => StartCoroutine(OnGameStart()));
        });

        startBtn.onClick.AddListener(() =>
        {
            startPanel.transform.DOMoveX(startPanel.transform.position.x + 220f, 1f);
            StartCoroutine(OnGameStart());
        });

        settingBtn.onClick.AddListener(() =>
        {
            PanelOn(settingPanel, true);
        });

        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        menuOnBtn.onClick.AddListener(() =>
        {
            if (!PlayerMove.canMove) return; 
            PanelOn(menuPanel, !menuPanel.interactable);
            Time.timeScale = menuPanel.interactable ? 0 : 1;
        });

        backToStartBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
        });

        continueBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            PanelOn(menuPanel, false);
        });

        menuOff.onClick.AddListener(() =>
        {
            PanelOn(settingPanel, false);
        });

        rankingBtn.onClick.AddListener(() =>
        {
            if (!rankingPanel.interactable)
            {
                GameManager.instance.saver.Rank();
            }
            PanelOn(rankingPanel, true);
        });

        rankingPanelOffBtn.onClick.AddListener(() =>
        {
            PanelOn(rankingPanel, false);
            List<RectTransform> objs = new List<RectTransform>(GameManager.instance.saver.rankParent.GetComponentsInChildren<RectTransform>());
            objs.RemoveAt(0);
            foreach(var item in objs)
            {
                Destroy(item.gameObject);
            }
        });

        exitBtn.onClick.AddListener(() =>
        {
            GameManager.instance.BackToStart();
        });

        explainShowBtn.onClick.AddListener(() =>
        {
            PanelOn(explainPanel, true);
        });

        explainOffBtn.onClick.AddListener(() =>
        {
            PanelOn(explainPanel, false);
        });
    }

    private void StartPanelButtonsOnOff(bool on)
    {
        startBtn.interactable = on;
        settingBtn.interactable = on;
        quitBtn.interactable = on;
        rankingBtn.interactable = on;
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

    public void OnGameOver()
    {
        ingameUIPanel.transform.DOMoveY(ingameUIPanel.transform.position.y - 40, 0.5f);
        topBar.gameObject.SetActive(true);
        resultPanel.transform.DOMoveY(resultPanel.transform.position.y - 120, 1f).OnComplete(() =>
        resultPanel.GetComponentInChildren<ShowResult>().ShowScore());
    }

    public void OnBackToStart()
    {
        ingameUIPanel.transform.DOMoveY(ingameUIPanel.transform.position.y - 40, 0.5f);
        startPanel.transform.DOMoveX(startPanel.transform.position.x - 220f, 1f);
        PanelOn(menuPanel, false);
        Time.timeScale = 1;
    }

    public void InitGameUI()
    {
        scoreText.text = "0";
        SetHp(GameManager.instance.hpCount);
        topBar.SetActive(true);
        resultPanel.GetComponentInChildren<ShowResult>().gradeText.gameObject.SetActive(false);
    }

    void PanelOn(CanvasGroup panel, bool on)
    {
        panel.alpha = on ? 1 : 0;
        panel.interactable = on;
        panel.blocksRaycasts = on;
    }

    public void ImgOnOff(Image img)
    {
        img.enabled = !img.enabled;
    }


    public void SetHp(int hpCount)
    {
        List<RectTransform> images = new List<RectTransform>(hpTrm.GetComponentsInChildren<RectTransform>());
        images.RemoveAt(0);
        foreach (var item in images)
        {
            Destroy(item.gameObject);
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
        GameManager.instance.playerHealth.hp++;
    }

    public void HpImageFill(Image img, bool fill)
    {
        if (fill) img.color = Color.red;
        else img.color = new Color(0, 0, 0, 0);
    }

    public void SetText(Text text, string textValue)
    {
        text.text = textValue;
    }

    IEnumerator OnGameStart()
    {
        InitGameUI();
        yield return sec;
        PanelOn(countdownPanel, true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return halfSec;
        }
        ingameUIPanel.transform.DOMoveY(ingameUIPanel.transform.position.y + 40, 0.5f);
        yield return halfSec;
        countdownText.text = "시작!";

        PanelOn(countdownPanel, false);
        GameManager.instance.GameStart();
    }

    public void OnHiitedEffect()
    {
        hittedPanel.color = new Color(hittedPanel.color.r, hittedPanel.color.g, hittedPanel.color.b, 1f); // 이거 나중에 빼서 쓰자
        hittedPanel.DOFade(0, 0.2f);
        GameManager.instance.perfectClearChecker = false;
    }

    public void PlusScore_AnimEnd()
    {
        StartCoroutine(CoPlusScore_AnimEnd());
    }

    IEnumerator CoPlusScore_AnimEnd()
    {
        plusScoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        plusScoreText.gameObject.SetActive(false);
    }
}
