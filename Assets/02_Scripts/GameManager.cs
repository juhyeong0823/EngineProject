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
    [SerializeField] private GameObject playingAnimObj;

    private Transform pOriginPos; // 플레이어 기존 위치
    private Transform gOriginPos; // 바닥 기존 위치
    public GameObject player;
    public GameObject ground;
    public int hpCount = 3;

    public float score = 0;
    public float scoreUpSpeed = 10f;

    private void Start()
    {
        pOriginPos = player.transform;
        gOriginPos = ground.transform;
        InitGameData();
    }

    private void Update()
    {
        ScoreUpdate();

        if (Input.GetKeyDown(KeyCode.Escape)) GameOver();
    }

    public void InitGameData()
    {
        score = 0;
        player.transform.position = pOriginPos.position;
    }

    public void GameOver()
    {
        PlayerMove.canMove = false;
        AnimationOff();
        UIManager.instance.ShowResult();
    }

    public void GameStart()
    {
        PlayerMove.canMove = true;
        Init_animObjs();
        RandomPattern();
    }



    void ScoreUpdate()
    {
        if (PlayerMove.canMove)
        {
            score += scoreUpSpeed * Time.deltaTime;
            UIManager.instance.SetText(UIManager.instance.scoreText, Mathf.Floor(score).ToString());
        }
    }

    public void PlusScore(int plusValue)
    {
        score += plusValue;
        UIManager.instance.SetText(UIManager.instance.scoreText, Mathf.Floor(score).ToString());
    }


    void Init_animObjs()
    {
        foreach (var item in animObjPrefab)
        {
            GameObject obj = Instantiate(item, this.transform);
            animObjs.Add(obj);
        }
    }

    public void OnGameOver()
    {
        AnimationOff();
        playingAnimObj = null;
    }


    public void RandomPattern()
    {
        if (animObjPrefab.Count <= 0) return;

        animObjs.Clear();
        if (animObjs.Count <= 0)
        {
            foreach (var item in animObjPrefab)
            {
                animObjs.Add(item);
            }
        }

        int rand = Random.Range(0, animObjs.Count - 1);
        animObjs[rand].SetActive(true);
        animObjs.Remove(playingAnimObj);
        playingAnimObj = animObjs[rand];
    }

    public void OffPlayingAnim()
    {
        playingAnimObj.SetActive(false);
    }

    public void AnimationOff()
    {
        foreach (var anim in animObjs)
        {
            anim.SetActive(false);
        }
    }


    #region Anim Function

    public void GroundMoveUp() {
        ground.transform.DOMoveY(ground.transform.position.y + 5f, 1.5f);
        ground.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void GroundMoveDown() { 
        ground.transform.DOMoveY(ground.transform.position.y - 5f, 1.5f);
        ground.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void GroundMoveRight() { 
        ground.transform.DOMoveX(ground.transform.position.x - 5f, 1.5f);
        ground.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void GroundMoveLeft() { 
        ground.transform.DOMoveX(ground.transform.position.x - 5f, 1.5f);
        ground.GetComponent<BoxCollider2D>().enabled = false;
    }

    //애는 바닥 이동하는 애니메이션이 끝나면 실행해서 다시 돌려보내고..
    public void GroundMoveOriginPos() {
        ground.transform.DOMove(gOriginPos.position, 1.5f);
        ground.GetComponent<BoxCollider2D>().enabled = true;
    }


    #endregion

}
