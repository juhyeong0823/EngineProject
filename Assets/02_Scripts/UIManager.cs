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

    public const float HIGHEST_GRADE_SCORE = 10000f;

    private bool isFirstPlay = true;

    public List<Image> hpImages = new List<Image>();
    public RectTransform titleTextMovePos;

    [Space(10)]
    [Header("체력")]
    public Transform hpImagesParent;
    public Image hpImage;

    [Space(10)]
    [Header("점수")]
    public Text scoreText; // 게임 중에 보일 점수
    public Text plusScoreText; // 점수가 추가될 때 그만큼의 점수 + 된것을 보여줌

    [Header("패널들")]
    public CanvasGroup resultPanel; // 결과
    public Image startPanel; //패널오브젝트임
    public CanvasGroup countdownPanel; // 321 시작!
    public CanvasGroup ingameUIPanel; // 모바일 빌드용, 현재는 사용 X
    public CanvasGroup menuPanel; // 메뉴
    public CanvasGroup settingPanel; // 옵션
    public CanvasGroup rankingPanel; // 랭킹
    public CanvasGroup explainPanel; // 게임 설명
    public GameObject topBar; // 맨위 점수나 체력 등을 적당한 때 끄고 켜기 위해 묶어놓은 친구.
    public Image hittedPanel; // 피격시 켜지고, 페이드아웃할 패널

    [Space(10)]
    [Header("제목")] // 시작시 제목이 3줄로 나뉘어 나옴!
    public Text topText;   // "Move"
    public Text middleText;// "And"
    public Text bottomText;// "Food"

    public Text countdownText; // 3,2,1

    [Header("시작화면")]
    public Button startBtn;  // 게임시작
    public Button quitBtn;   // 게임종료
    public Button settingBtn; // 옵션
    public Button settingOffBtn; // 옵션 끄기 버튼
    public Button explainShowBtn; // 게임설명 창 켜기
    public Button explainOffBtn; // 게임설명 창 끄기
    public Button rankingBtn; // 랭킹 창 켜고, 랭킹 데이터 넣어주기
    public Button rankingPanelOffBtn; // 랭킹 창 끄기

    [Space(10)]
    public Button exitBtn;        // 게임종료
    public Button retryBtn;       // 재시작
    public Button menuOnBtn;      // 인게임 메뉴 열기
    public Button backToMain; // 메인화면 돌아가기
    public Button continueBtn;    // 일시정지 종료, 계속 진행

    public InputField nickname; //이름 입력
    
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
        if (Input.GetKeyDown(KeyCode.Escape)) MenuOn();
    }

    void InitButtons() // 버튼 onClick 함수 추가
    {
        retryBtn.onClick.AddListener(() =>
        {
            resultPanel.transform.DOMoveY(resultPanel.transform.position.y + 120, 1f).
            SetEase(Ease.OutQuint).OnComplete(() => StartCoroutine(OnGameStart()));
            //결과 화면 위로 치우고 게임시작
        });

        startBtn.onClick.AddListener(() =>
        {
            startPanel.transform.DOMoveX(startPanel.transform.position.x + 220f, 1f).OnComplete(() =>
            {
                StartCoroutine(OnGameStart());
            });
            //시작화면 옆으로 치우고 게임시작
        });

        settingBtn.onClick.AddListener(() =>
        {
            PanelOn(settingPanel, true);
            // 옵션창 켜기
        });

        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
            //게임종료
        });

        menuOnBtn.onClick.AddListener(() =>
        {
            MenuOn();
        });

        backToMain.onClick.AddListener(() =>
        {
            GameManager.instance.BackToStart(); // 메인으로 돌아가게
        });

        continueBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1; 
            PanelOn(menuPanel, false); // 메뉴 끄기
        });

        settingOffBtn.onClick.AddListener(() =>
        {
            PanelOn(settingPanel, false); // 옵션창 끄기
        });

        rankingBtn.onClick.AddListener(() =>
        {
            if (!rankingPanel.interactable) GameManager.instance.saver.Rank(); // 랭킹창이 켜진게 아니라면, Rank함수 실행
            PanelOn(rankingPanel, true);    
        });

        rankingPanelOffBtn.onClick.AddListener(() =>
        {
            PanelOn(rankingPanel, false); // 랭킹창 끄기
            List<RectTransform> objs = new List<RectTransform>(GameManager.instance.saver.rankParent.GetComponentsInChildren<RectTransform>()); // 랭킹 프리팹들을 가져오는 용도
            objs.RemoveAt(0); // 랭킹 프리팹들이 들어가는 위치는 지우면 안되니까!
            foreach(var item in objs) Destroy(item.gameObject); // 프리팹들 전부 삭제
        });

        exitBtn.onClick.AddListener(() =>
        {
            GameManager.instance.BackToStart(); // // 메인으로 돌아가게
        });

        explainShowBtn.onClick.AddListener(() =>
        {
            PanelOn(explainPanel, true); // 게임 설명창 켜기
        });

        explainOffBtn.onClick.AddListener(() =>
        {
            PanelOn(explainPanel, false); // 게임 설명창 끄기
        });
    }

    private void StartPanelButtonsOnOff(bool on) // 게임 시작시 제목이 나타나는 애니메이션이  끝나면 게임 시작과 옵션, 랭킹, 종료 버튼을 누를 수 있게 만들어주는 함수
    {
        startBtn.interactable = on;
        settingBtn.interactable = on;
        quitBtn.interactable = on;
        rankingBtn.interactable = on;
    }

    public void StartPanel_DOAnimPlay() // 게임 시작시 제목이 날아오는 애니메이션 구현한 것.
    {
        if (isFirstPlay) // 게임을 켜고 한번만 실행되도록
        {
            topText.rectTransform.DOMoveX(titleTextMovePos.position.x, 0.5f).OnComplete(() =>
            middleText.rectTransform.DOMoveX(titleTextMovePos.position.x, 0.5f).OnComplete(() =>
            bottomText.rectTransform.DOMoveX(titleTextMovePos.position.x, 0.5f).OnComplete(() => StartPanelButtonsOnOff(true))));

            isFirstPlay = false; // 이건 처음 시작할 때만 쓸 애니메이션이니까 ㅁㅁ..
        }
    }

    public void OnGameOver() // 게임 오버 시 UI들 초기화 
    {
        ingameUIPanel.transform.DOMoveY(ingameUIPanel.transform.position.y - 40, 0.5f); // 점프나 이동버튼 안보이게 밑으로 치우고
        topBar.gameObject.SetActive(true); // 체력과 점수 UI 꺼주고
        resultPanel.transform.DOMoveY(resultPanel.transform.position.y - 120, 1f).OnComplete(() => // 결과창이 보이도록 가져오고
        resultPanel.GetComponentInChildren<ShowResult>().ShowScore()); // 결과창 점수올라가거나 등급 보여주는 것 등등 실행
    }

    public void OnBackToStart() // 게임 중단 시 UI 초기화
    {
        ingameUIPanel.transform.DOMoveY(ingameUIPanel.transform.position.y - 40, 0.5f); // 점프나 이동버튼 안보이게 밑으로 치우고
        startPanel.transform.DOMoveX(startPanel.transform.position.x - 220f, 1f); // 시작화면 보이도록 가져오기
        PanelOn(menuPanel, false); // 메뉴창 끄기
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
        List<RectTransform> images = new List<RectTransform>(hpImagesParent.GetComponentsInChildren<RectTransform>());
        images.RemoveAt(0);
        foreach (var item in images)
        {
            Destroy(item.gameObject);
        }
        hpImages.Clear();
        for (int i = 0; i < hpCount; i++) // hp 이미지들 
        {
            Image img = Instantiate(hpImage, hpImagesParent);
            HpImageFill(img, true); // 채워진 상태 hp 추가되더라도 맞으면 색 바꿔서 맞은거 보이게
            hpImages.Add(img);
        }
    }

    public void PlusMaxHP()
    {
        Image img = Instantiate(hpImage, hpImagesParent);
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

    void MenuOn()
    {
        if (!PlayerMove.canMove) return; // 게임을 시작한 상태가 아니면 실행불가
        PanelOn(menuPanel, !menuPanel.interactable); //메뉴창 켜기
        Time.timeScale = menuPanel.interactable ? 0 : 1; // 메뉴창이 켜지면 0 켜지지 않으면 1로
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

    

    IEnumerator CoPlusScore_AnimEnd()
    {
        plusScoreText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        plusScoreText.gameObject.SetActive(false);
    }
}
