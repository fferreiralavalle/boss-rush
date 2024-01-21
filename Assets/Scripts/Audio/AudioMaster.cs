using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public static AudioMaster Instance;

    public float effectsVolume = 0.5f;
    public float effectsVolumeMultiplier = 0.5f;
    public float musicVolume = 0.5f;
    public float musicVolumeMultiplier = 0.5f;

    public AudioSource musicSource;

    protected List<AudioSource> soundEffectsPlaying = new List<AudioSource>();
    protected AudioData musicData;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMusic(AudioData audio)
    {
        if (audio == null || !audio.audioClip) return;
        musicData = audio;
        musicSource.clip = audio.audioClip;
        musicSource.loop = audio.loop;
        musicSource.volume = musicVolume * audio.volumeMultiplier * musicVolumeMultiplier;
        musicSource.pitch = audio.GetRandomPitch();
        musicSource.PlayDelayed(audio.delay);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySoundEffect(AudioData audio)
    {
        if (audio == null || !audio.audioClip) return;
        AudioSource audioMono = gameObject.AddComponent<AudioSource>();
        audioMono.clip = audio.audioClip;
        audioMono.volume = effectsVolume * audio.volumeMultiplier * musicVolumeMultiplier;
        audioMono.pitch = audio.GetRandomPitch();
        audioMono.PlayDelayed(audio.delay);
        float duration = audio.audioClip.length + 0.1f;
        soundEffectsPlaying.Add(audioMono);
        Destroy(audioMono, duration);
        StartCoroutine(DeleteFromArray(audioMono, duration));
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        float audioDataVolume = musicData != null ? musicData.volumeMultiplier : 1f;
        musicSource.volume = audioDataVolume * musicVolume * musicVolumeMultiplier;
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;
    }

    protected IEnumerator DeleteFromArray(AudioSource audioMono, float delay)
    {
        yield return new WaitForSeconds(delay);
        soundEffectsPlaying.Remove(audioMono);
    }
}
