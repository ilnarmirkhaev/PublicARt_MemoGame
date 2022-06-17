using UnityEngine;

namespace AudioPlayerTemplate
{
    public class AudioManagerTest : MonoBehaviour
    {
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            AudioPlayer.OnAudioPlay += PlayAudio;
            AudioPlayer.OnAudioPause += PauseAudio;
        }

        private void OnDisable()
        {
            AudioPlayer.OnAudioPlay -= PlayAudio;
            AudioPlayer.OnAudioPause -= PauseAudio;
        }

        private void PlayAudio(AudioClip audioClip)
        {
            if (_source.clip != audioClip)
            {
                _source.clip = audioClip;
                _source.time = 0;
            }
            
            _source.Play();
        }

        private void PauseAudio(AudioClip obj)
        {
            _source.Stop();
        }
    }
}