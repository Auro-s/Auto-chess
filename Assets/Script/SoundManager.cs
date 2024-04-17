using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;

    public void SetVolume()
    {
        float volume = slider.value;
        mixer.SetFloat("Master", Mathf.Log10(volume)*20);
    }
}
