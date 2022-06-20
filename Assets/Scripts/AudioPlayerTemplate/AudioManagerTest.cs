using UnityEngine;

namespace AudioPlayerTemplate
{
    public class AudioManagerTest : MonoBehaviour
    {
        private AudioSource _source;
        private AudioPlayer _current;

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

        private void PlayAudio(AudioPlayer audioPlayer)
        {
            _current = audioPlayer;
            
            if (_source.clip != _current.AudioClip)
            {
                _source.clip = _current.AudioClip;
                _source.time = 0;
            }
            
            _source.Play();
        }

        private void PauseAudio(AudioPlayer audioPlayer)
        {
            if (_current == audioPlayer)
                _source.Pause();
        }
    }
}