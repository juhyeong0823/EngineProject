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

    public List<GameObject> animObjPrefab = new List<GameObject>(); // 패턴 프리팹들
    public List<GameObject> animObjs = new List<GameObject>(); // 실행시 넣어줄, 랜덤으로 실행해줄 리스트

    public PlayerHeatlh playerHealth; // 플레이어 체력
    public Saver saver; // 저장해주는 친구

    public bool perfectClearChecker = true;

    private GameObject playingAnimObj;
    public GameObject player;
    public GameObject ground;

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

    public void GameOver() // 게임오버 됐을 때
    {
        PlayerMove.canMove = false;
        UIManager.instance.OnGameOver(); // UI들 초기화
        AllOfAnimationOff(); // 모든 애니메이션 삭제 및 중지
        SoundManager.instance.audioPlayer.Stop(); // 노래끄기
    }

    public void BackToStart()
    {
        PlayerMove.canMove = false; 
        UIManager.instance.OnBackToStart(); // 시작화면으로 돌아감
        UIManager.instance.topBar.SetActive(false); // 일시정지, 점수창, 체력 등 안보이도록
        AllOfAnimationOff(); // 모든 애니메이션 삭제 및 중지
        SoundManager.instance.audioPlayer.Stop(); // 노래 끄기 
        Time.timeScale = 1; // 일시정지를 누르거나 esc를 누르면 시간이 멈춘 상태이기 때문
    }

    public void GameStart() // 게임시작시 실행할 것들 
    {
        PlayerMove.canMove = true; 
        playerHealth.hp = hpCount; // 체력 초기화
        perfectClearChecker = true; // 추가점수 부여 boolean값 설정
        UIManager.instance.InitGameUI(); 
        InitGameData();
        Init_animObjs();
        RandomPattern();
        SoundManager.instance.audioPlayer.Play(); // 노래틀기

    }

    public void InitGameData() // 플레이어, 바닥 위치 처음위치로 점수를 0으로 초기화
    {
        score = 0;
        player.transform.position = pOriginPos.position;
        ground.transform.position = gOriginPos.position;
    }

    void ScoreUpdate() // 게임 플레이중이면 자동으로 점수가 올라감
    {
        if (PlayerMove.canMove)
        {
            PlusScore(scoreUpSpeed * Time.deltaTime);
        }
    }

    public void PlusScore(float plusValue) // 점수 추가 함수
    {
        score += plusValue;
        UIManager.instance.SetText(UIManager.instance.scoreText, Mathf.Floor(score).ToString());
    }

    void Init_animObjs() // 패턴은 켜지는 순간 실행되기 때문에 만들어두고 전부 꺼버린 뒤 실행할 패턴 리스트에 추가
    {
        animObjs.Clear();
        foreach (var item in animObjPrefab)
        {
            GameObject obj = Instantiate(item, this.transform);
            obj.SetActive(false);
            animObjs.Add(obj);
        }
    }

    public void RandomPattern() // 실행할 패턴 애니메이션 리스트 관리 및 랜덤한 패턴 실행, 추가점수 부여 여부 bool 값 초기화
    {
        if (animObjs.Count <= 0) Init_animObjs();

        int rand = Random.Range(0, animObjs.Count - 1);
        playingAnimObj = animObjs[rand];
        playingAnimObj.SetActive(true);
        animObjs.Remove(playingAnimObj);
        perfectClearChecker = true;
    }

    public void OffPlayingAnim() // 실행중인 애니메이션 멈추기
    {
        Destroy(playingAnimObj); // 실행중인 애니메이션 없애기
        playingAnimObj = null; // 필요한 코드인가? 안전용 코드
    }

    public void AllOfAnimationOff() // 플레이중인 패턴애니메이션 삭제 후 실행할 패턴 오브젝트 리스트 전부 삭제
    {
        OffPlayingAnim();
        playingAnimObj = null; // 필요한 코드인가? 안전용 코드
        foreach (var anim in animObjs)
        {
            Destroy(anim);
        }
        animObjs.Clear();
    }

    //패턴 오브젝트에 넣어줄 친구, 바닥의 이동 등을 담당할듯
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

    //애는 바닥 이동하는 애니메이션이 끝나면 실행해서 다시 돌려보내고..
    public void GroundMoveOriginPos()
    {
        ground.transform.DOMove(gOriginPos.position, 1.5f);
    }

    #endregion

}
