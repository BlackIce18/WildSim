using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderVolume : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Sounds _sounds;
    [SerializeField] private SoundManager _soundManager;

    private void Start()
    {
        if(_slider == null)
        {
            _slider = GetComponent<Slider>();
        }
        if (_audioSource != null)
        {
            ChangeAudioSourceVolume();
        }
        if (_soundManager != null)
        {
            ChangeSoundManagerAudioSourceListVolume();
        }

        LoadFromPrefsSliderValues();
    }

    private void LoadFromPrefsSliderValues()
    {
        if (_sounds == Sounds.sounds)
        {
            _slider.value = PlayerPrefs.GetFloat(_soundManager.SoundVolumePrefs);
        }
        else if (_sounds == Sounds.musics)
        {
            _slider.value = PlayerPrefs.GetFloat(_soundManager.MusicVolumePrefs);
        }
    }

    public void ChangeSoundManagerAudioSourceListVolume()
    {
        _soundManager.ChangeAudioSourcesVolume(ChooseList(), Mathf.Clamp01(_slider.value));
        _soundManager.SaveSettings();
    }
    public void ChangeAudioSourceVolume()
    {
        _audioSource.volume = Mathf.Clamp01(_slider.value);
    }
    private List<AudioSource> ChooseList()
    {
        List<AudioSource> result = new List<AudioSource>();

        switch (_sounds)
        {
            case Sounds.sounds:
                result = _soundManager.Sounds;
                break;
            case Sounds.musics:
                result = _soundManager.Musics;
                break;
        }

        return result;
    }
}
