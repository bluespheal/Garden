using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip[] musicClipArray;
    public AudioClip[] sfxClipArray;

    [SerializeField]public float musicVolume;
    [SerializeField]public float sfxVolume;

    private void Start()
    {
        musicSource.volume = musicVolume;
    }



    private AudioClip GetMusicClip(int clip)
    {
        return musicClipArray[clip];
    }

    public void PlaySong(int audioClip)
    {
        musicSource.clip = GetMusicClip(audioClip);
        musicSource.Play();
    }
    private AudioClip GetSFXClip(int clip)
    {
        return sfxClipArray[clip];
    }
    public void PlaySFX(int audioClip)
    {
        sfxSource.PlayOneShot(GetSFXClip(audioClip), sfxVolume);
    }
}
