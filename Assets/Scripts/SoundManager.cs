using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public AudioSource audioSource;
    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    public void AudioPlay(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);

    }
}
