using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//VO들은 전부 서버와 데이터를 주고받기 위한 수단입니다.

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
            string json = JsonUtility.ToJson(new saveDataVo(score, name)); // 제이슨으로 변환해서
            SendPostRequest($"{baseUrl}/save", json, (res) => Debug.Log("Save 완료")); // 보내버립니다!
        }
    }

    public void Rank()
    {
        Debug.Log("Rank");
        SendPostRequest($"{baseUrl}/rank", "", (res) => 
        {
            Debug.Log(res); // 이 res에는 이름과 점수 리스트가 넘어오고
            if (res != null)
            {
                saveDataListVO vo = JsonUtility.FromJson<saveDataListVO>(res); // 리스트로 받아서

                foreach (var item in vo.list) // 프리팹 생성후 그 안의 텍스트들에 서버로부터 받은 값을 넣어줍니다.
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
        string json = JsonUtility.ToJson(new NameVO(UIManager.instance.nickname.text)); // 이름을 보내주고

        if (UIManager.instance.nickname.text.Length <= 1) // 이름을 넣지 않았다면 로드하지 않습니다.
        {
            UIManager.instance.resultPanel.GetComponentInChildren<ShowResult>().bestScoreText.text = $"BestScore : 0";
            return;
        }

        SendPostRequest($"{baseUrl}/load", json, (res) =>
        {
            Debug.Log(res); // 점수를 받아와 업데이트하거나 새로 저장, 불러오기 등을 실행합니다

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

        if(payload.Length > 1) // 혹시 전송할 payload가 없을 수 있으니까
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
