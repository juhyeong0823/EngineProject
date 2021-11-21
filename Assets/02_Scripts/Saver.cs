using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class saveDataVo
{
    public int score;
    public string name;

    public saveDataVo(int score, string name)
    {
        this.score = score;
        this.name = name;
    }
}

[System.Serializable]
public class saveDataListVO
{
    public List<saveDataVo> list;
}

[System.Serializable]
public class ScoreVo
{
    public string score;

    public ScoreVo(string score)
    {
        this.score = score;
    }
}

[System.Serializable]
public class NameVO
{
    public string name;

    public NameVO(string name)
    {
        this.name = name;
    }
}



public class Saver : MonoBehaviour
{
    public RankShowObjPrefab rankPrefab;
    public Transform rankParent;

    public string baseUrl = "http://localhost:54000";

    private void Start()
    {
        Debug.Log("세이버");
    }

    public void Save(int score)
    {
        Debug.Log("Save");
        string name = UIManager.instance.nickname.text;
        Debug.Log(name.Length);
        if (name.Length > 1)
        {
            string json = JsonUtility.ToJson(new saveDataVo(score, name));
            SendPostRequest($"{baseUrl}/save", json, (res) => Debug.Log("Save 완료"));
        }
    }

    public void Rank()
    {
        Debug.Log("Rank");
        SendPostRequest($"{baseUrl}/rank", "", (res) =>
        {
            Debug.Log(res);
            if (res != null)
            {
                saveDataListVO vo = JsonUtility.FromJson<saveDataListVO>(res);

                foreach (var item in vo.list)
                {
                    RankShowObjPrefab obj = Instantiate(rankPrefab, rankParent);
                    obj.NameText.text = item.name;
                    obj.ScoreText.text = item.score.ToString();
                }
            }
        });
    }

    public void LoadScore(Text scoreText)
    {
        Debug.Log("LoadScore");
        string json = JsonUtility.ToJson(new NameVO(UIManager.instance.nickname.text));

        if (UIManager.instance.nickname.text.Length <= 1)
        {
            UIManager.instance.resultPanel.GetComponentInChildren<ShowResult>().bestScoreText.text = $"BestScore : 0";
            return;
        }

        SendPostRequest($"{baseUrl}/load", json, (res) =>
        {
            Debug.Log(res);

            if(res.Length > 1)
            {
                ScoreVo vo = JsonUtility.FromJson<ScoreVo>(res);
                int score = int.Parse(vo.score);

                if (score > GameManager.instance.score)
                {
                    Debug.Log("이미 최고점수가 있고, 지금 세운 기록보다 더 높은 점수입니다.");
                    scoreText.text = $"Best Score : {score}";
                }
                else
                {
                    Debug.Log("이미 최고점수가 있지만, 지금 세운 기록이 더 높은 점수입니다. 점수를 업데이트하겠습니다.");
                    scoreText.text = $"Best Score : {(int)GameManager.instance.score}";
                    Save((int)GameManager.instance.score);
                }
            }
            else
            {
                Debug.Log("기록이 없습니다 새로 저장하겠습니다.");
                scoreText.text = $"Best Score : {(int)GameManager.instance.score}";
                Save((int)GameManager.instance.score);
            }
        });
    }

    public void SendPostRequest(string url, string payload, Action<string> callback)
    {
        StartCoroutine(SendPost(url, payload, callback));
    }

    IEnumerator SendPost(string url, string payload, Action<string> callback)
    {
        Debug.Log($"SendPost : {payload}");
        UnityWebRequest req = UnityWebRequest.Post(url, payload);
        req.SetRequestHeader("Content-Type", "application/json");

        if(payload.Length > 1)
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(payload);
            req.uploadHandler = new UploadHandlerRaw(jsonToSend);
        }
        
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("연결 성공");
            string data = req.downloadHandler.text;
            callback(data);
        }
    }
}
