using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class ScoreVo
{
    public ScoreVo(int score)
    {
        this.score = score;
    }

    public int score;
}

public class Saver : MonoBehaviour
{
    public string baseUrl = "http://localhost:54000";
    string score = "10";
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            string json = JsonUtility.ToJson(new ScoreVo(10));
            Debug.Log(json);
            SendPostRequest(baseUrl, json);
        }
    }


    public void SendPostRequest(string url, string payload)
    {
        StartCoroutine(SendPost($"{baseUrl}/save", payload));
    }

    IEnumerator SendPost(string url, string payload)
    {
        UnityWebRequest req = UnityWebRequest.Post(url, payload);
        req.SetRequestHeader("Content-Type", "application/json");

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(payload);
        req.uploadHandler = new UploadHandlerRaw(jsonToSend);   

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string data = req.downloadHandler.text;
            Debug.Log(data);
        }
        else
        {
            Debug.Log("우린 망했어 끝이야");
        }

    }
}
