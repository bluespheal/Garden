using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private AudioClip[] musicClipArray;
    [SerializeField]
    private AudioClip[] sfxClipArray;

    [SerializeField]
    private float musicVolume;
    [SerializeField]
    private float sfxVolume;

    public float SfxVolume { get => sfxVolume; set => sfxVolume = value; }


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
