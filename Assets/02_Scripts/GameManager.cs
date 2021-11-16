using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
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

    public List<GameObject> animObjPrefab = new List<GameObject>();
    public List<GameObject> animObjs = new List<GameObject>();

    public PlayerHeatlh playerHealth;
    public Saver saver;

    public bool perfectClearChecker = true;

    private GameObject playingAnimObj;
    public GameObject player;
    public GameObject ground;
    public GameObject potionPrefab;

    public Transform gOriginPos; // 바닥 기존 위치
    public Transform pOriginPos; // 플레이어 기존 위치
    public Transform potionMakePos; // 포션 생성 위치

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

    public void GameOver()
    {
        PlayerMove.canMove = false;
        UIManager.instance.OnGameOver();
        UIManager.instance.topBar.SetActive(false);
        AllOfAnimationOff();
        playingAnimObj = null;
        SoundManager.instance.audioPlayer.Stop();
    }

    public void BackToStart()
    {
        PlayerMove.canMove = false;
        UIManager.instance.OnBackToStart();
        UIManager.instance.topBar.SetActive(false);
        AllOfAnimationOff();
        playingAnimObj = null;
        SoundManager.instance.audioPlayer.Stop();
    }

    public void GameStart()
    {
        PlayerMove.canMove = true;
        playerHealth.hp = hpCount;
        perfectClearChecker = true;
        UIManager.instance.InitGameUI();
        InitGameData();
        Init_animObjs();
        RandomPattern();
        SoundManager.instance.audioPlayer.Play();
        
    }

    public void InitGameData()
    {
        score = 0;
        player.transform.position = pOriginPos.position;
        ground.transform.position = gOriginPos.position;
    }

    void ScoreUpdate()
    {
        if (PlayerMove.canMove)
        {
            PlusScore(scoreUpSpeed * Time.deltaTime);
        }
    }

    public void PlusScore(float plusValue)
    {
        score += plusValue;
        UIManager.instance.SetText(UIManager.instance.scoreText, Mathf.Floor(score).ToString());
    }

    void Init_animObjs()
    {
        animObjs.Clear();
        foreach (var item in animObjPrefab)
        {
            GameObject obj = Instantiate(item, this.transform);
            obj.SetActive(false);
            animObjs.Add(obj);
        }
    }
     
    public void RandomPattern()
    {
        if (animObjs.Count <= 0) Init_animObjs();
 
        int rand = Random.Range(0, animObjs.Count - 1);
        playingAnimObj = animObjs[rand];
        playingAnimObj.SetActive(true);
        animObjs.Remove(playingAnimObj);
        perfectClearChecker = true;
    }

    public void OffPlayingAnim()
    {
        Destroy(playingAnimObj);
    }

    public void AllOfAnimationOff()
    {
        Destroy(playingAnimObj);
        playingAnimObj = null; // 필요한 코드인가?
        foreach (var anim in animObjs)
        {
            Destroy(anim);
        }
        animObjs.Clear();
    }

    #region Anim Function

    public void GroundMoveUp() {
        ground.transform.DOMoveY(ground.transform.position.y + 5f, 1.5f);
    }

    public void GroundMoveDown() { 
        ground.transform.DOMoveY(ground.transform.position.y - 5f, 1.5f);
    }

    public void GroundMoveRight() { 
        ground.transform.DOMoveX(ground.transform.position.x - 5f, 1.5f);
    }

    public void GroundMoveLeft() { 
        ground.transform.DOMoveX(ground.transform.position.x - 5f, 1.5f);
    }

    //애는 바닥 이동하는 애니메이션이 끝나면 실행해서 다시 돌려보내고..
    public void GroundMoveOriginPos() {
        ground.transform.DOMove(gOriginPos.position, 1.5f);
    }

    public void DropPotion() 
    {
        if(score < 6000) // c등급까지만 포션이 나옴
            Instantiate(potionPrefab, potionMakePos.position, Quaternion.identity);
    }
    
    #endregion

}
