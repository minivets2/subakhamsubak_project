using UnityEngine;
using UnityEngine.UI;

public enum VolumeSliderType
{
    Music,
    SFX
}

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private VolumeSliderType type;
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (type == VolumeSliderType.Music)
        {
            slider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeMusicVolume(val));
            slider.value = AudioManager.Instance.musicSource.volume;   
        }
        else
        {
            slider.onValueChanged.AddListener(val => AudioManager.Instance.ChangeSfxVolume(val));
            slider.value = AudioManager.Instance.sfxSource.volume;   
        }
    }
}
