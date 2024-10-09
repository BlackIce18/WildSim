using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds
{
    musics,
    sounds
}
public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _musics = new List<AudioSource>();
    [SerializeField] private List<AudioSource> _sounds = new List<AudioSource>();
    [SerializeField] private List<AudioSource> _paused = new List<AudioSource>();
    public List<AudioSource> Musics { get { return _musics; } }
    public List<AudioSource> Sounds { get { return _sounds; } }
    [Range(0f,1f)]
    [SerializeField] private float _musicVolume = 1.0f;
    [Range(0f,1f)]
    [SerializeField] private float _soundVolume = 1.0f;

    public readonly string MusicVolumePrefs = "MusicVolume";
    public readonly string SoundVolumePrefs = "SoundVolume";
    [SerializeField] private bool _LoadPrefs = true;
    private void Start()
    {
        ChangeAudioSourcesVolume(_musics, _musicVolume);
        ChangeAudioSourcesVolume(_sounds, _soundVolume);

        if (_LoadPrefs)
        {
            LoadSettings();
        }
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey(MusicVolumePrefs))
        {
            _musicVolume = PlayerPrefs.GetFloat(MusicVolumePrefs);
            ChangeAudioSourcesVolume(_musics, _musicVolume);
        }
        if (PlayerPrefs.HasKey(SoundVolumePrefs)) {
            _soundVolume = PlayerPrefs.GetFloat(SoundVolumePrefs);
            ChangeAudioSourcesVolume(_sounds, _soundVolume);
        }
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MusicVolumePrefs, _musicVolume);
        PlayerPrefs.SetFloat(SoundVolumePrefs, _soundVolume);
    }
    public void ChangeAudioSourcesVolume(List<AudioSource> audioSources, float volume) 
    {
        foreach (var audio in audioSources) { audio.volume = Mathf.Clamp01(volume); }
    }
    public void ChangeSoundVolume(AudioSource audio, float volume)
    {
        audio.volume = volume;
    }

    public void PlayOnce(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
    public void PlayOnce(AudioSource audioSource)
    {
        audioSource.Play();
    }
    public void PlayLooped(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.loop = true;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlayLoopedList(AudioSource audioSource, List<AudioClip> audioClipList)
    {
        StartCoroutine(PlayList(audioSource, audioClipList));
    }

    private IEnumerator PlayList(AudioSource audioSource, List<AudioClip> audioClipList)
    {
        foreach(var audioClip in audioClipList)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            yield return PlaySound(audioClip);
        }
    }
    private IEnumerator PlaySound(AudioClip audioClip)
    {
        yield return new WaitForSeconds(audioClip.length);
        yield return true;
    }

    public void PlayRandomAudioClip(AudioSource audioSource, List<AudioClip> audioClips)
    {
        int randomNumber = Random.Range(0, audioClips.Count+1);

        PlayOnce(audioSource, audioClips[randomNumber]);
    }


    public void PauseAudioSource(AudioSource audioSource)
    {
        audioSource.Pause();
        _paused.Add(audioSource);
    }

    public void StopAudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void ResumeAudioSource(AudioSource audioSource)
    {
        foreach(var pausedSource in  _paused)
        {
            if(pausedSource == audioSource)
            {
                audioSource.Play();
            }
        }
    }
}
