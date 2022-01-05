using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region �̱���
    private static GameManager Instance = null;
    public static GameManager instance
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

    public List<GameObject> animObjPrefab = new List<GameObject>(); // ���� �����յ�
    public List<GameObject> animObjs = new List<GameObject>(); // ����� �־���, �������� �������� ����Ʈ

    public PlayerHeatlh playerHealth; // �÷��̾� ü��
    public Saver saver; // �������ִ� ģ��

    public bool perfectClearChecker = true;

    private GameObject playingAnimObj;
    public GameObject player;
    public GameObject ground;

    public Transform gOriginPos; // �ٴ� ���� ��ġ
    public Transform pOriginPos; // �÷��̾� ���� ��ġ
    public Transform potionMakePos; // ���� ���� ��ġ

    public int hpCount = 3;

    public float score = 0;
    public float scoreUpSpeed = 10f;

    private void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHeatlh>();
    }

    private void Update()
    {
        ScoreUpdate();
    }

    public void GameOver() // ���ӿ��� ���� ��
    {
        PlayerMove.canMove = false;
        UIManager.instance.OnGameOver(); // UI�� �ʱ�ȭ
        AllOfAnimationOff(); // ��� �ִϸ��̼� ���� �� ����
        SoundManager.instance.audioPlayer.Stop(); // �뷡����
    }

    public void BackToStart()
    {
        PlayerMove.canMove = false; 
        UIManager.instance.OnBackToStart(); // ����ȭ������ ���ư�
        UIManager.instance.topBar.SetActive(false); // �Ͻ�����, ����â, ü�� �� �Ⱥ��̵���
        AllOfAnimationOff(); // ��� �ִϸ��̼� ���� �� ����
        SoundManager.instance.audioPlayer.Stop(); // �뷡 ���� 
        Time.timeScale = 1; // �Ͻ������� �����ų� esc�� ������ �ð��� ���� �����̱� ����
    }

    public void GameStart() // ���ӽ��۽� ������ �͵� 
    {
        PlayerMove.canMove = true; 
        playerHealth.hp = hpCount; // ü�� �ʱ�ȭ
        perfectClearChecker = true; // �߰����� �ο� boolean�� ����
        UIManager.instance.InitGameUI(); 
        InitGameData();
        Init_animObjs();
        RandomPattern();
        SoundManager.instance.audioPlayer.Play(); // �뷡Ʋ��

    }

    public void InitGameData() // �÷��̾�, �ٴ� ��ġ ó����ġ�� ������ 0���� �ʱ�ȭ
    {
        score = 0;
        player.transform.position = pOriginPos.position;
        ground.transform.position = gOriginPos.position;
    }

    void ScoreUpdate() // ���� �÷������̸� �ڵ����� ������ �ö�
    {
        if (PlayerMove.canMove)
        {
            PlusScore(scoreUpSpeed * Time.deltaTime);
        }
    }

    public void PlusScore(float plusValue) // ���� �߰� �Լ�
    {
        score += plusValue;
        UIManager.instance.SetText(UIManager.instance.scoreText, Mathf.Floor(score).ToString());
    }

    void Init_animObjs() // ������ ������ ���� ����Ǳ� ������ �����ΰ� ���� ������ �� ������ ���� ����Ʈ�� �߰�
    {
        animObjs.Clear();
        foreach (var item in animObjPrefab)
        {
            GameObject obj = Instantiate(item, this.transform);
            obj.SetActive(false);
            animObjs.Add(obj);
        }
    }

    public void RandomPattern() // ������ ���� �ִϸ��̼� ����Ʈ ���� �� ������ ���� ����, �߰����� �ο� ���� bool �� �ʱ�ȭ
    {
        if (animObjs.Count <= 0) Init_animObjs();

        int rand = Random.Range(0, animObjs.Count - 1);
        playingAnimObj = animObjs[rand];
        playingAnimObj.SetActive(true);
        animObjs.Remove(playingAnimObj);
        perfectClearChecker = true;
    }

    public void OffPlayingAnim() // �������� �ִϸ��̼� ���߱�
    {
        Destroy(playingAnimObj); // �������� �ִϸ��̼� ���ֱ�
        playingAnimObj = null; // �ʿ��� �ڵ��ΰ�? ������ �ڵ�
    }

    public void AllOfAnimationOff() // �÷������� ���Ͼִϸ��̼� ���� �� ������ ���� ������Ʈ ����Ʈ ���� ����
    {
        OffPlayingAnim();
        playingAnimObj = null; // �ʿ��� �ڵ��ΰ�? ������ �ڵ�
        foreach (var anim in animObjs)
        {
            Destroy(anim);
        }
        animObjs.Clear();
    }

    //���� ������Ʈ�� �־��� ģ��, �ٴ��� �̵� ���� ����ҵ�
    #region Anim Function


    public void GroundMoveUp()
    {
        ground.transform.DOMoveY(ground.transform.position.y + 5f, 1.5f);
    }

    public void GroundMoveDown()
    {
        ground.transform.DOMoveY(ground.transform.position.y - 5f, 1.5f);
    }

    public void GroundMoveRight()
    {
        ground.transform.DOMoveX(ground.transform.position.x - 5f, 1.5f);
    }

    public void GroundMoveLeft()
    {
        ground.transform.DOMoveX(ground.transform.position.x - 5f, 1.5f);
    }

    //�ִ� �ٴ� �̵��ϴ� �ִϸ��̼��� ������ �����ؼ� �ٽ� ����������..
    public void GroundMoveOriginPos()
    {
        ground.transform.DOMove(gOriginPos.position, 1.5f);
    }

    #endregion

}
