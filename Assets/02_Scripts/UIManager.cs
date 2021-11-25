using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    #region �̱���
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
    [Header("ü��")]
    public Transform hpImagesParent;
    public Image hpImage;

    [Space(10)]
    [Header("����")]
    public Text scoreText; // ���� �߿� ���� ����
    public Text plusScoreText; // ������ �߰��� �� �׸�ŭ�� ���� + �Ȱ��� ������

    [Header("�гε�")]
    public CanvasGroup resultPanel; // ���
    public Image startPanel; //�гο�����Ʈ��
    public CanvasGroup countdownPanel; // 321 ����!
    public CanvasGroup ingameUIPanel; // ����� �����, ����� ��� X
    public CanvasGroup menuPanel; // �޴�
    public CanvasGroup settingPanel; // �ɼ�
    public CanvasGroup rankingPanel; // ��ŷ
    public CanvasGroup explainPanel; // ���� ����
    public GameObject topBar; // ���� ������ ü�� ���� ������ �� ���� �ѱ� ���� ������� ģ��.
    public Image hittedPanel; // �ǰݽ� ������, ���̵�ƿ��� �г�

    [Space(10)]
    [Header("����")] // ���۽� ������ 3�ٷ� ������ ����!
    public Text topText;   // "Move"
    public Text middleText;// "And"
    public Text bottomText;// "Food"

    public Text countdownText; // 3,2,1

    [Header("����ȭ��")]
    public Button startBtn;  // ���ӽ���
    public Button quitBtn;   // ��������
    public Button settingBtn; // �ɼ�
    public Button settingOffBtn; // �ɼ� ���� ��ư
    public Button explainShowBtn; // ���Ӽ��� â �ѱ�
    public Button explainOffBtn; // ���Ӽ��� â ����
    public Button rankingBtn; // ��ŷ â �Ѱ�, ��ŷ ������ �־��ֱ�
    public Button rankingPanelOffBtn; // ��ŷ â ����

    [Space(10)]
    public Button exitBtn;        // ��������
    public Button retryBtn;       // �����
    public Button menuOnBtn;      // �ΰ��� �޴� ����
    public Button backToMain; // ����ȭ�� ���ư���
    public Button continueBtn;    // �Ͻ����� ����, ��� ����

    public InputField nickname; //�̸� �Է�
    
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

    void InitButtons() // ��ư onClick �Լ� �߰�
    {
        retryBtn.onClick.AddListener(() =>
        {
            resultPanel.transform.DOMoveY(resultPanel.transform.position.y + 120, 1f).
            SetEase(Ease.OutQuint).OnComplete(() => StartCoroutine(OnGameStart()));
            //��� ȭ�� ���� ġ��� ���ӽ���
        });

        startBtn.onClick.AddListener(() =>
        {
            startPanel.transform.DOMoveX(startPanel.transform.position.x + 220f, 1f).OnComplete(() =>
            {
                StartCoroutine(OnGameStart());
            });
            //����ȭ�� ������ ġ��� ���ӽ���
        });

        settingBtn.onClick.AddListener(() =>
        {
            PanelOn(settingPanel, true);
            // �ɼ�â �ѱ�
        });

        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
            //��������
        });

        menuOnBtn.onClick.AddListener(() =>
        {
            MenuOn();
        });

        backToMain.onClick.AddListener(() =>
        {
            GameManager.instance.BackToStart(); // �������� ���ư���
        });

        continueBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1; 
            PanelOn(menuPanel, false); // �޴� ����
        });

        settingOffBtn.onClick.AddListener(() =>
        {
            PanelOn(settingPanel, false); // �ɼ�â ����
        });

        rankingBtn.onClick.AddListener(() =>
        {
            if (!rankingPanel.interactable) GameManager.instance.saver.Rank(); // ��ŷâ�� ������ �ƴ϶��, Rank�Լ� ����
            PanelOn(rankingPanel, true);    
        });

        rankingPanelOffBtn.onClick.AddListener(() =>
        {
            PanelOn(rankingPanel, false); // ��ŷâ ����
            List<RectTransform> objs = new List<RectTransform>(GameManager.instance.saver.rankParent.GetComponentsInChildren<RectTransform>()); // ��ŷ �����յ��� �������� �뵵
            objs.RemoveAt(0); // ��ŷ �����յ��� ���� ��ġ�� ����� �ȵǴϱ�!
            foreach(var item in objs) Destroy(item.gameObject); // �����յ� ���� ����
        });

        exitBtn.onClick.AddListener(() =>
        {
            GameManager.instance.BackToStart(); // // �������� ���ư���
        });

        explainShowBtn.onClick.AddListener(() =>
        {
            PanelOn(explainPanel, true); // ���� ����â �ѱ�
        });

        explainOffBtn.onClick.AddListener(() =>
        {
            PanelOn(explainPanel, false); // ���� ����â ����
        });
    }

    private void StartPanelButtonsOnOff(bool on) // ���� ���۽� ������ ��Ÿ���� �ִϸ��̼���  ������ ���� ���۰� �ɼ�, ��ŷ, ���� ��ư�� ���� �� �ְ� ������ִ� �Լ�
    {
        startBtn.interactable = on;
        settingBtn.interactable = on;
        quitBtn.interactable = on;
        rankingBtn.interactable = on;
    }

    public void StartPanel_DOAnimPlay() // ���� ���۽� ������ ���ƿ��� �ִϸ��̼� ������ ��.
    {
        if (isFirstPlay) // ������ �Ѱ� �ѹ��� ����ǵ���
        {
            topText.rectTransform.DOMoveX(titleTextMovePos.position.x, 0.5f).OnComplete(() =>
            middleText.rectTransform.DOMoveX(titleTextMovePos.position.x, 0.5f).OnComplete(() =>
            bottomText.rectTransform.DOMoveX(titleTextMovePos.position.x, 0.5f).OnComplete(() => StartPanelButtonsOnOff(true))));

            isFirstPlay = false; // �̰� ó�� ������ ���� �� �ִϸ��̼��̴ϱ� ����..
        }
    }

    public void OnGameOver() // ���� ���� �� UI�� �ʱ�ȭ 
    {
        ingameUIPanel.transform.DOMoveY(ingameUIPanel.transform.position.y - 40, 0.5f); // ������ �̵���ư �Ⱥ��̰� ������ ġ���
        topBar.gameObject.SetActive(true); // ü�°� ���� UI ���ְ�
        resultPanel.transform.DOMoveY(resultPanel.transform.position.y - 120, 1f).OnComplete(() => // ���â�� ���̵��� ��������
        resultPanel.GetComponentInChildren<ShowResult>().ShowScore()); // ���â �����ö󰡰ų� ��� �����ִ� �� ��� ����
    }

    public void OnBackToStart() // ���� �ߴ� �� UI �ʱ�ȭ
    {
        ingameUIPanel.transform.DOMoveY(ingameUIPanel.transform.position.y - 40, 0.5f); // ������ �̵���ư �Ⱥ��̰� ������ ġ���
        startPanel.transform.DOMoveX(startPanel.transform.position.x - 220f, 1f); // ����ȭ�� ���̵��� ��������
        PanelOn(menuPanel, false); // �޴�â ����
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
        for (int i = 0; i < hpCount; i++) // hp �̹����� 
        {
            Image img = Instantiate(hpImage, hpImagesParent);
            HpImageFill(img, true); // ä���� ���� hp �߰��Ǵ��� ������ �� �ٲ㼭 ������ ���̰�
            hpImages.Add(img);
        }
    }

    public void PlusMaxHP()
    {
        Image img = Instantiate(hpImage, hpImagesParent);
        HpImageFill(img, true); // ä���� ���� hp �߰��Ǵ��� ������ �� �ٲ㼭 ������ ���̰�
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
        if (!PlayerMove.canMove) return; // ������ ������ ���°� �ƴϸ� ����Ұ�
        PanelOn(menuPanel, !menuPanel.interactable); //�޴�â �ѱ�
        Time.timeScale = menuPanel.interactable ? 0 : 1; // �޴�â�� ������ 0 ������ ������ 1��
    }

    public void OnHiitedEffect()
    {
        hittedPanel.color = new Color(hittedPanel.color.r, hittedPanel.color.g, hittedPanel.color.b, 1f); // �̰� ���߿� ���� ����
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
        countdownText.text = "����!";

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
