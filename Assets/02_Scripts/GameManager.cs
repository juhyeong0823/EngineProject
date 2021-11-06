using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
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

    private Transform startPos; // ∞‘¿”Ω√¿€ ¿ßƒ°
    public GameObject player;
    public int hpCount = 3;

    public float score = 0;
    public float scoreUpSpeed = 10f;

    private void Start()
    {
        startPos = player.transform;
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
        player.transform.position = startPos.position;
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
}
