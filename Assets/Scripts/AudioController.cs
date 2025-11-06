using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] AudioSource BgmAudio;
    [SerializeField] AudioSource SfxAudio;


    public AudioClip Bgm;
    public AudioClip Malehurt;
    public AudioClip Femalehurt;

    public AudioClip Magic;

    public AudioClip MaleAttack;

    public AudioClip FemaleAttack;

    // public AudioClip Transform;
    

    private void Start()
    {
        BgmAudio.clip = Bgm;
        BgmAudio.loop = true; // 设置 BGM 循环播放
        BgmAudio.volume = 0.3f; // 设置 BGM 音量 (0.0 到 1.0)
        BgmAudio.Play();
    }

    // Update is called once per frame

    public void PlaySfx(AudioClip clip,float volume = 1f)
    {   
        SfxAudio.volume = volume; // 设置音效音量
        SfxAudio.PlayOneShot(clip);
    }

}
