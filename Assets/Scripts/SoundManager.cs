using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }
    public AudioClip bgm;
    public AudioClip btnClick;
    public AudioClip cardSound;
    public AudioClip bellRing;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgm;
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.Play();//게임시작 시 bgm재생
    }
    public void RingBellSound()
    {
        audioSource.PlayOneShot(bellRing);
    }
    public void BtnClickSound()
    {
        audioSource.PlayOneShot(btnClick);
    }
    public void CardSound()
    {
        audioSource.PlayOneShot(cardSound);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
