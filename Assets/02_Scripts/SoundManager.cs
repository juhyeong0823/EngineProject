using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
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


    private void Update()
    {
        VolumeSetting();
    }

    void VolumeSetting()
    {
        audioPlayer.volume = audioSlider.value;
        efxPlayer.volume = efxSlider.value;
    }


}
