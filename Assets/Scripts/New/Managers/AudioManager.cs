using UnityEngine;
using UnityEngine.Audio;

namespace New.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _bgmSource;

        [SerializeField]
        private AudioSource _sfxSource;

        [SerializeField]
        private AudioMixer _audioMixer;
        
        // _sfxSource has Audio Random Container inside Audio Resource field for punches
        [field: SerializeField] public AudioClip IntroBGMClip { get; private set; }
        [field: SerializeField] public AudioClip MenuBGMClip { get; private set; }
        [field: SerializeField] public AudioClip InGameBGMClip { get; private set; }
        [field: SerializeField] public AudioClip BellClip { get; private set; }
        [field: SerializeField] public AudioClip DeathClip { get; private set; }
        
        public void PlayBGM(AudioClip clip, bool loop = true)
        {
            _bgmSource.clip = clip;
            _bgmSource.loop = loop;
            _bgmSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            _sfxSource.PlayOneShot(clip);
        }

        public void PlayPunch()
        {
            _sfxSource.Play();
        }

        public void SetBGMVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0.0001f, 1f); // avoid -Infinity
            _audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20f);
        }

        public void SetSFXVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0.0001f, 1f); // avoid -Infinity
            _audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20f);
        }

        public void SetMasterVolume(float volume)
        {
            volume = Mathf.Clamp(volume, 0.0001f, 1f); // avoid -Infinity
            _audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20f);
        }
    }
}