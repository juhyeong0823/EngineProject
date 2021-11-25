using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region 싱글톤
    private static SoundManager Instance = null;
    public static SoundManager instance
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

    public Slider audioSlider;
    public Slider efxSlider;
    public AudioSource audioPlayer;
    public AudioSource efxPlayer;


    //기본 음량은 적게
    private void Start()
    {
        audioSlider.value = 0.1f;
        efxSlider.value = 0.1f;
    }

    private void Update()
    {
        VolumeSetting();
    }

    //슬라이더의 벨류에 따라 음량 조절
    void VolumeSetting()
    {
        audioPlayer.volume = audioSlider.value;
        efxPlayer.volume = efxSlider.value;
    }
}
