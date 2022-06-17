using System;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace AudioPlayerTemplate
{
    public class AudioPlayer
    {
        // Audio Player State
        private enum AudioPlayerState
        {
            NONE = -1,
            LOADING = 0,
            PLAYING = 1,
            PAUSED = 2
        }

        private AudioPlayerState _state;

        private AudioPlayerState State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                
                _state = value;
                if (_state == AudioPlayerState.NONE) return;
                
                SetButtonIcon(_state);

                if (_state == AudioPlayerState.LOADING)
                {
                    _playButton.SetEnabled(false);
                    _tweener = AnimateUI.AnimateRotation(_buttonIcon, 360, LoopType.Incremental, Ease.Linear);
                }
                else
                {
                    _playButton.SetEnabled(true);
                    _tweener?.Kill();

                    if (_state == AudioPlayerState.PLAYING)
                    {
                        Play();
                    }
                    else
                    {
                        Pause();
                    }
                }
            }
        }

        // UI Elements
        private VisualElement _audioPlayerView;
        private VisualElement _elementToClose;

        private Button _closeButton;
        private Button _playButton;

        private VisualElement _buttonIcon;

        private Slider _audioSlider;

        // Styles
        private string[] _iconStyles = { "icon-loading", "icon-pause", "icon-play" };
        private const int Padding = 16;

        // Events
        public static event Action OnAudioPlayerOpened;
        public static event Action OnAudioPlayerClosed;

        public static event Action<AudioClip> OnAudioPlay;
        public static event Action<AudioClip> OnAudioPause;

        // Misc
        private Tweener _tweener;

        private AudioClip _audioClip;
        private bool _clipIsDownloaded;

        public AudioPlayer(VisualElement audioPlayerView, VisualElement visualElementToClose)
        {
            _audioPlayerView = audioPlayerView;
            _elementToClose = visualElementToClose;

            // UI Elements
            InitUIElements();
            
            // Hide all elements
            SetPadding(0);
            _audioPlayerView.style.display = DisplayStyle.None;
            
            SetAudioPlayerElementsDisplay(DisplayStyle.None);

            // Event listeners
            _closeButton.clicked += Close;
            _playButton.clicked += ChangePlayState;

            State = AudioPlayerState.NONE;
        }

        private void InitUIElements()
        {
            _closeButton = _audioPlayerView.Q<Button>("CloseButton");
            _playButton = _audioPlayerView.Q<Button>("PlayButton");
            _audioSlider = _audioPlayerView.Q<Slider>();

            _buttonIcon = _playButton.Q<VisualElement>("Icon");
        }

        // Logic
        public void OnAudioLoaded()
        {
            State = AudioPlayerState.PLAYING;
            _clipIsDownloaded = true;
        }

        private void ChangePlayState()
        {
            if (State == AudioPlayerState.PLAYING)
                State = AudioPlayerState.PAUSED;
            else if (State == AudioPlayerState.PAUSED)
                State = AudioPlayerState.PLAYING;
        }
        
        public async void Open()
        {
            State = _clipIsDownloaded ? AudioPlayerState.PLAYING : AudioPlayerState.LOADING;
            
            OnAudioPlayerOpened?.Invoke();
            _elementToClose.style.display = DisplayStyle.None;
            _audioPlayerView.style.display = DisplayStyle.Flex;

            await AnimateUI.AnimateHeight(_audioPlayerView, 0, 196, 0.5f, 1);
            SetPadding(Padding);
            
            SetAudioPlayerElementsDisplay(DisplayStyle.Flex);
        }

        private async void Close()
        {
            State = _clipIsDownloaded ? AudioPlayerState.PAUSED : AudioPlayerState.LOADING;
            
            SetAudioPlayerElementsDisplay(DisplayStyle.None);

            SetPadding(0);
            await AnimateUI.AnimateHeight(_audioPlayerView, 196, 0, 0.5f, 1);

            _audioPlayerView.style.display = DisplayStyle.None;
            _elementToClose.style.display = DisplayStyle.Flex;
            OnAudioPlayerClosed?.Invoke();
        }

        public void SetAudioClip(AudioClip audioClip) => _audioClip = audioClip;

        private void Play()
        {
            OnAudioPlay?.Invoke(_audioClip);
        }

        private void Pause()
        {
            OnAudioPause?.Invoke(_audioClip);
        }
        
        // Styling methods
        private void SetButtonIcon(AudioPlayerState state)
        {
            _buttonIcon.transform.rotation = Quaternion.Euler(Vector3.zero);
            
            for (var i = 0; i < _iconStyles.Length; i++)
                _buttonIcon.EnableInClassList(_iconStyles[i], (int)state == i);
        }
        
        private void SetPadding(int value)
        {
            _audioPlayerView.style.paddingTop = value;
            _audioPlayerView.style.paddingRight = value;
            _audioPlayerView.style.paddingBottom = value;
            _audioPlayerView.style.paddingLeft = value;
        }

        private void SetAudioPlayerElementsDisplay(DisplayStyle displayStyle)
        {
            _closeButton.style.display = displayStyle;
            _playButton.style.display = displayStyle;
            _audioSlider.style.display = displayStyle;
        }

        private void Check(VisualElement element)
        {
            if (element == null) Debug.Log("хуй");
        }
    }
}