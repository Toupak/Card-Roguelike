using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace BoomLib.Pause_Menu.Scripts
{
    public class VolumeController : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private AudioMixer musicMixer;

        [Space]
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private AudioMixer sfxMixer;

        private float musicVolume = 1.0f;
        private float sfxVolume = 1.0f;
        
        private void Start()
        {
            if (musicSlider != null)
                musicSlider.onValueChanged.AddListener(OnUpdateMusicVolume);
            
            if (sfxSlider != null)
                sfxSlider.onValueChanged.AddListener(OnUpdateSFXVolume);
        }

        private void OnUpdateMusicVolume(float volume)
        {
            musicVolume = Tools.Tools.NormalizedValueToDecibel(volume);
            musicMixer.SetFloat("MixerVolume", musicVolume);
        }
        
        private void OnUpdateSFXVolume(float volume)
        {
            sfxVolume = Tools.Tools.NormalizedValueToDecibel(volume);
            sfxMixer.SetFloat("MixerVolume", sfxVolume);
        }
    }
}
