using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer masterMixer;

    public Slider musicSlider;
    public Slider effectSlider;
    public Toggle onOffToggle;

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(value => UpdateMusicVolume(value));
        effectSlider.onValueChanged.AddListener(value => UpdateEffectVolume(value));
        onOffToggle.onValueChanged.AddListener(value => UpdateSoundOnOff(value));
        UpdateMusicVolume(1f);
        UpdateEffectVolume(1f);
    }
    public void UpdateMusicVolume(float value)
    {
        masterMixer.SetFloat(Define.SOUND_BGM , Mathf.Log10(value) * 20f);
    }
    public void UpdateEffectVolume(float value)
    {
        masterMixer.SetFloat(Define.SOUND_SFX , Mathf.Log10(value) * 20f);
    }
    public void UpdateSoundOnOff(bool value) 
    { 
        masterMixer.SetFloat(Define.SOUND_BGM, value ? Mathf.Log10(musicSlider.value) * 20 : -80f);
        masterMixer.SetFloat(Define.SOUND_SFX, value ? Mathf.Log10(effectSlider.value) * 20 : -80f);
    }
}
