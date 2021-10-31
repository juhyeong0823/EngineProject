using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternScriptForAnimEvent : MonoBehaviour
{
    private AudioSource ac;
    private void Start()
    {
        ac = SoundManager.instance.efxPlayer;
    }

    public void PlayEfx(AudioClip clip)
    {
        ac.clip = clip;
        ac.Play();
    }
}
