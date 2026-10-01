using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip SFX;
    [SerializeField] private int MaxVol;
    [SerializeField] private List<AudioClip> SoundTracks;
    
    void Start()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlayMusicRand()
    {
        int STindex = Random.Range(0, SoundTracks.Count);
        backgroundMusic = SoundTracks[STindex];
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void SetVolume(float volume)
    {
        musicSource.volume = volume;

    }
    public void FadeIn()
    {

    }
    public void FadeOut()
    {
        for (int i = 0; i < MaxVol; i++)
        {
            musicSource.volume--;
        }
    }
    public void StopSFX()
    {
        SFXSource.Stop();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.clip = clip;
        SFXSource.PlayOneShot(clip);
    }
}
