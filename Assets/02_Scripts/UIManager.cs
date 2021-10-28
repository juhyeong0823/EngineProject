using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public CanvasGroup gameOverPanel;
    public GameObject resultPanel;

    public Text scoreText;

    public List<Image> hpImages = new List<Image>();

    private PlayerHeatlh player;

    public Button retryBtn;

    public Transform hpTrm;
    public Image hpImage;
    public int hpCount = 3;
    public float score = 0;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerHeatlh>();

        for (int i = 0; i < hpCount; i++) // hp �̹����� 
        {
            Image img = Instantiate(hpImage, hpTrm);
            HpImageFill(img, true); // ä���� ���� hp �߰��Ǵ��� ������ �� �ٲ㼭 ������ ���̰�
            hpImages.Add(img);
        }
        scoreText.text = 0.ToString();

        PanelOff(gameOverPanel);
        ButtonsInit();
    }

    private void Update()
    {
        score += Time.deltaTime;
        scoreText.text = Mathf.Floor(score).ToString();
    }

    public void PlusMaxHP()
    {
        Image img = Instantiate(hpImage, hpTrm);
        HpImageFill(img, true); // ä���� ���� hp �߰��Ǵ��� ������ �� �ٲ㼭 ������ ���̰�
        hpImages.Add(img);
        player.hp++;
    }

    public void HpImageFill(Image img, bool fill)
    {
        if (fill) img.color = Color.red;
        else img.color = Color.gray;
    }
    
    void ButtonsInit()
    {
        retryBtn.onClick.AddListener(() =>
        {
            PanelOff(gameOverPanel);
            SceneManager.LoadScene("InGame");
        });
    }

    public void GameOver()
    {
        PanelOn(gameOverPanel);
        GameManager.instance.AnimationOff();
        PlayerMove.canMove = false;

        resultPanel.GetComponent<ShowResult>().ShowScore();
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
    
    public void ButtonPointerEnter(Image img)
    {
        img.enabled = true;
    }

    public void ButtonPointerExit(Image img)
    {
        img.enabled = false;
    }
}
