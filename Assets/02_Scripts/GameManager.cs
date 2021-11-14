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

    public List<GameObject> animObjPrefab = new List<GameObject>();
    private List<GameObject> animObjs = new List<GameObject>();

    public PlayerHeatlh playerHealth;


    private GameObject playingAnimObj;
    public GameObject player;
    public GameObject ground;
    private Vector3 gOriginPos; // �ٴ� ���� ��ġ
    private Vector3 pOriginPos; // �÷��̾� ���� ��ġ

    public int hpCount = 3;

    public float score = 0;
    public float scoreUpSpeed = 10f;

    private void Start()
    {
        pOriginPos = player.transform.position;
        gOriginPos = ground.transform.position;
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHeatlh>();
        InitGameData();

    }

    private void Update()
    {
        ScoreUpdate();
    }

    public void GameOver()
    {
        PlayerMove.canMove = false;
        UIManager.instance.OnGameOver();
        AllOfAnimationOff();
        playingAnimObj = null;
    }

    public void GameStart()
    {
        PlayerMove.canMove = true;
        playerHealth.hp = hpCount;
        Init_animObjs();
        RandomPattern();
        InitGameData();
    }

    public void InitGameData()
    {
        score = 0;
        player.transform.position = pOriginPos;
        ground.transform.position = gOriginPos;
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
        if (animObjPrefab.Count <= 0)
        {
            Debug.Log("�ִϸ��̼� �־�");
            return;
        }

        if (animObjs.Count <= 0) Init_animObjs();
 
        int rand = Random.Range(0, animObjs.Count - 1);
        playingAnimObj = animObjs[rand];
        playingAnimObj.SetActive(true);
        animObjs.Remove(playingAnimObj);
    }

    public void OffPlayingAnim()
    {
        Destroy(playingAnimObj);
    }

    public void AllOfAnimationOff()
    {
        Destroy(playingAnimObj);
        playingAnimObj = null; // �ʿ��� �ڵ��ΰ�?
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

    //�ִ� �ٴ� �̵��ϴ� �ִϸ��̼��� ������ �����ؼ� �ٽ� ����������..
    public void GroundMoveOriginPos() {
        ground.transform.DOMove(gOriginPos, 1.5f);
    }


    #endregion

}
