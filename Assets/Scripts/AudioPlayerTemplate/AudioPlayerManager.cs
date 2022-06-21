using UnityEngine;

namespace AudioPlayerTemplate
{
    public class AudioPlayerManager : MonoBehaviour
    {
        private AudioPlayer _currentPlayer;
        private AudioSource _source;
        private bool _wasPlaying;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            AudioPlayer.OnAudioPlay += PlayAudio;
            AudioPlayer.OnAudioPause += PauseAudio;
            AudioPlayer.OnSliderValueChanged += ChangeAudioTime;
            AudioPlayer.OnSliderDragStarted += PauseBySlider;
            AudioPlayer.OnSliderDragEnded += Resume;
        }

        private void OnDisable()
        {
            AudioPlayer.OnAudioPlay -= PlayAudio;
            AudioPlayer.OnAudioPause -= PauseAudio;
            AudioPlayer.OnSliderValueChanged -= ChangeAudioTime;
            AudioPlayer.OnSliderDragStarted -= PauseBySlider;
            AudioPlayer.OnSliderDragEnded -= Resume;
        }

        private void Update()
        {
            if (_source.clip == null) return;
            
            if (_source.isPlaying)
                _currentPlayer.SetSliderValue(_source.time);

            if (_source.time >= _source.clip.length && !_source.isPlaying)
            {
                _currentPlayer.Restart();
                _source.time = 0;
            }
        }

        private void PlayAudio(AudioPlayer audioPlayer)
        {
            _currentPlayer = audioPlayer;

            if (_source.clip != _currentPlayer.AudioClip)
            {
                _source.clip = _currentPlayer.AudioClip;
                _source.time = 0;
            }

            _source.Play();
        }

        private void PauseAudio(AudioPlayer audioPlayer)
        {
            if (_currentPlayer != audioPlayer) return;
            
            _source.Pause();
        }

        private void PauseBySlider(AudioPlayer audioPlayer)
        {
            if (_currentPlayer != audioPlayer) return;
            if (!_source.isPlaying) return;
            
            _wasPlaying = true;
            _source.Pause();
        }

        private void Resume(AudioPlayer audioPlayer)
        {
            if (_currentPlayer != audioPlayer) return;
            if (!_wasPlaying) return;
            
            _source.Play();
            _wasPlaying = false;
        }

        private void ChangeAudioTime(AudioPlayer audioPlayer, float newTime)
        {
            if (_currentPlayer != audioPlayer) return;
            
            _source.time = newTime;
        }
    }
}