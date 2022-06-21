using System;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Views;

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
                    _audioSlider.SetEnabled(false);
                    _tween = AnimateUI.AnimateRotation(_buttonIcon, 360, LoopType.Incremental, Ease.Linear);
                }
                else
                {
                    _playButton.SetEnabled(true);
                    _audioSlider.SetEnabled(true);
                    _tween?.Kill();

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
        private readonly VisualElement _audioPlayerView;
        private readonly VisualElement _elementToClose;

        private Button _closeButton;
        private Button _playButton;

        private VisualElement _buttonIcon;

        private Slider _audioSlider;
        private VisualElement _dragContainer;
        private Label _currentTime;
        private Label _totalTime;

        // Styles
        private readonly string[] _iconStyles = { "icon-loading", "icon-pause", "icon-play" };
        private const int Padding = 16;

        // Events

        public static event Action<AudioPlayer> OnAudioPlay;
        public static event Action<AudioPlayer> OnAudioPause;
        public static event Action<AudioPlayer, float> OnSliderValueChanged;
        public static event Action<AudioPlayer> OnSliderDragStarted;
        public static event Action<AudioPlayer> OnSliderDragEnded;

        // Misc
        private Tweener _tween;

        // Audio
        public AudioClip AudioClip;
        private bool _clipIsDownloaded;

        // UI
        public AudioPlayer(VisualElement audioPlayerView, VisualElement visualElementToClose)
        {
            _audioPlayerView = audioPlayerView;
            View.Check(_audioPlayerView);
            
            _elementToClose = visualElementToClose;
            View.Check(_elementToClose);
            
            // UI Elements
            InitUIElements();
            
            SetDefaultAudioValues();
            
            // Hide all elements
            SetPadding(0);
            _audioPlayerView.style.display = DisplayStyle.None;
            
            SetAudioPlayerElementsDisplay(DisplayStyle.None);

            // Event listeners
            InitEventListeners();
            
            State = AudioPlayerState.NONE;
        }

        private void SetDefaultAudioValues()
        {
            _audioSlider.lowValue = 0;
            _audioSlider.value = 0;
            _audioSlider.highValue = 0;
            
            _currentTime.text = "0:00";
            _totalTime.text = "0:00";
        }

        private void InitUIElements()
        {
            _closeButton = _audioPlayerView.Q<Button>("CloseButton");
            
            _playButton = _audioPlayerView.Q<Button>("PlayButton");
            _buttonIcon = _playButton.Q<VisualElement>("Icon");
            
            _audioSlider = _audioPlayerView.Q<Slider>();
            _dragContainer = _audioSlider.Q<VisualElement>("unity-drag-container");
            _currentTime = _audioSlider.Q<Label>("CurrentTime");
            _totalTime = _audioSlider.Q<Label>("TotalTime");
            
            View.Check(_closeButton);
            
            View.Check(_playButton);
            View.Check(_buttonIcon);
            
            View.Check(_audioSlider);
            View.Check(_currentTime);
            View.Check(_totalTime);
        }

        private void InitEventListeners()
        {
            _closeButton.clicked += Close;
            _playButton.clicked += ChangePlayState;
            
            // Play only one AudioPlayer
            OnAudioPlay += player =>
            {
                if (player != this && State != AudioPlayerState.LOADING)
                {
                    State = AudioPlayerState.PAUSED;
                }
            };
            
            _audioSlider.RegisterValueChangedCallback(evt =>
            {
                OnSliderValueChanged?.Invoke(this, evt.newValue);
            });
            
            _dragContainer.RegisterCallback<PointerMoveEvent>(_ =>
            {
                OnSliderDragStarted?.Invoke(this);
                Debug.Log("Drag started");
            });
            
            _dragContainer.RegisterCallback<PointerUpEvent>(_ =>
            {
                OnSliderDragEnded?.Invoke(this);
                Debug.Log("Drag ended");
            });
        }

        public void SetSliderValue(float value)
        {
            _audioSlider.SetValueWithoutNotify(value);
            _currentTime.text = SecondsToTime((int)value);
        }

        // Logic
        private void ChangePlayState()
        {
            if (State == AudioPlayerState.PLAYING)
                State = AudioPlayerState.PAUSED;
            else if (State == AudioPlayerState.PAUSED)
                State = AudioPlayerState.PLAYING;
        }
        
        public async void Open()
        {
            _audioSlider.value = 0;
            _elementToClose.style.display = DisplayStyle.None;
            _audioPlayerView.style.display = DisplayStyle.Flex;

            await AnimateUI.AnimateHeight(_audioPlayerView, 0, 196, 0.5f, 1);
            
            State = _clipIsDownloaded ? AudioPlayerState.PLAYING : AudioPlayerState.LOADING;
            
            SetPadding(Padding);
            SetAudioPlayerElementsDisplay(DisplayStyle.Flex);
        }

        private async void Close()
        {
            SetAudioPlayerElementsDisplay(DisplayStyle.None);
            SetPadding(0);
            
            State = _clipIsDownloaded ? AudioPlayerState.PAUSED : AudioPlayerState.LOADING;

            await AnimateUI.AnimateHeight(_audioPlayerView, 196, 0, 0.5f, 1);

            _audioPlayerView.style.display = DisplayStyle.None;
            _elementToClose.style.display = DisplayStyle.Flex;
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            Debug.Log($"AudioClip {audioClip.name} is set!");
            _clipIsDownloaded = true;
            AudioClip = audioClip;
            
            _audioSlider.highValue = AudioClip.length;
            _totalTime.text = SecondsToTime((int)_audioSlider.highValue);
            
            if (_audioPlayerView.style.display == DisplayStyle.Flex)
                State = AudioPlayerState.PLAYING;
        }

        private void Play()
        {
            OnAudioPlay?.Invoke(this);
        }

        private void Pause()
        {
            OnAudioPause?.Invoke(this);
        }

        public void Restart()
        {
            _audioSlider.value = 0;
            State = AudioPlayerState.PAUSED;
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
        
        private static string SecondsToTime(int seconds)
        {
            var minutes = seconds / 60;

            return $"{minutes}:{FormatSeconds(seconds % 60)}";
        }

        private static string FormatSeconds(int seconds)
        {
            return seconds < 10 ? "0" + seconds : seconds.ToString();
        }
    }
}