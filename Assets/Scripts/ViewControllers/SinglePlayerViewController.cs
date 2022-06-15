using System;
using System.Threading.Tasks;
using Android;
using Core;
using GameModes;
using UnityEngine;
using UnityEngine.UIElements;
using Views;

namespace ViewControllers
{
    public class SinglePlayerViewController : ViewController
    {
        private SinglePlayerView _singlePlayerView;
        
        [Tooltip("Game duration in seconds")]
        [SerializeField] private int gameDuration;

        private Coroutine _activeTimer;
        private bool _isStarted;
        private Task _countdown;

        public static event Action OnGameStarted;
        public static event Action OnGameExited;
        
        // Event for switching _canSelect in CardSelector
        public static event Action<bool> OnGamePaused;

        protected override View View
        {
            get => _singlePlayerView;
            set => _singlePlayerView = value is SinglePlayerView view ? view : null;
        }

        private void OnEnable()
        {
            SinglePlayerGame.OnScored += AddScore;
            SinglePlayerGame.OnGameEnded += GameOver;
        }

        private void OnDisable()
        {
            SinglePlayerGame.OnScored -= AddScore;
            SinglePlayerGame.OnGameEnded -= GameOver;
        }
        
        private async void AddScore(int score)
        {
            await AnimateUI.AnimateLabel(_singlePlayerView.ScoreText, 20, 32, score, 1);
        }

        public override void Init(UIDocument document)
        {
            View = new SinglePlayerView(document, viewName);
            InitButtonListeners();
        }

        protected override void DefaultScreen()
        {
            OnGamePaused?.Invoke(true);
            
            View.ShowElement(_singlePlayerView.Transparent);
            View.ShowElement(_singlePlayerView.TopPanel);
            View.ShowElement(_singlePlayerView.ReturnButton);
            View.ShowElement(_singlePlayerView.StartPanel);
            
            View.HideElement(_singlePlayerView.Timer);
            View.HideElement(_singlePlayerView.Score);
            View.HideElement(_singlePlayerView.ExitPanel);
            View.HideElement(_singlePlayerView.Counter);
        }

        protected virtual void InitButtonListeners()
        {
            _singlePlayerView.StartButton.clicked += StartGame;
            _singlePlayerView.ReturnButton.clicked += Pause;
            AndroidSettings.OnBackButtonPressed += Pause;
            _singlePlayerView.ContinueButton.clicked += Resume;
            _singlePlayerView.ExitButton.clicked += Exit;
            _singlePlayerView.PlayAgainButton.clicked += StartGame;
            _singlePlayerView.ToBeginningButton.clicked += ReloadScene;
        }

        private async void StartGame()
        {
            OnGameStarted?.Invoke();

            OnGamePaused?.Invoke(true);
            
            _isStarted = true;
            
            View.HideElement(_singlePlayerView.StartPanel);
            View.HideElement(_singlePlayerView.GameOverPanel);
            View.ShowElement(_singlePlayerView.Transparent);
            
            View.HideElement(_singlePlayerView.ReturnButton);
            View.HideElement(_singlePlayerView.Timer);
            _singlePlayerView.ScoreText.text = "0";
            View.HideElement(_singlePlayerView.Score);
            
            View.ShowElement(_singlePlayerView.Counter);
            
            _countdown = ViewTimer.Countdown(_singlePlayerView.CounterText, 3);
            await _countdown;
            _countdown = null;

            View.HideElement(_singlePlayerView.Counter);
            View.HideElement(_singlePlayerView.Transparent);
            
            View.ShowElement(_singlePlayerView.ReturnButton);
            View.ShowElement(_singlePlayerView.Timer);
            View.ShowElement(_singlePlayerView.Score);

            _activeTimer = StartCoroutine(ViewTimer.TimerCoroutine(_singlePlayerView.Timer, gameDuration));
            
            OnGamePaused?.Invoke(false);
        }

        private void Pause()
        {
            if (ViewsManager.Current != this || _countdown != null) return;

            if (View.IsVisible(_singlePlayerView.GameOverPanel))
                return;

            OnGamePaused?.Invoke(true);
            if (!_isStarted)
                View.HideElement(_singlePlayerView.StartPanel);

            View.ShowElement(_singlePlayerView.ExitPanel);
            View.ShowElement(_singlePlayerView.Transparent);
            Time.timeScale = 0;
        }

        private void Resume()
        {
            OnGamePaused?.Invoke(false);
            if (_isStarted)
                View.HideElement(_singlePlayerView.Transparent);
            else
                View.ShowElement(_singlePlayerView.StartPanel);
            
            View.HideElement(_singlePlayerView.ExitPanel);
            Time.timeScale = 1;
        }

        private void Exit()
        {
            OnGameExited?.Invoke();
            OnGamePaused?.Invoke(true);
            
            _isStarted = false;
            if (_activeTimer != null)
                StopCoroutine(_activeTimer);
            
            View.RemoveStyleClass(_singlePlayerView.Timer, "timer-red");
            Time.timeScale = 1;
            ReloadScene();
            // ViewsManager.Return();
        }

        private void GameOver(int score)
        {
            if (_activeTimer != null)
                StopCoroutine(_activeTimer);

            OnGamePaused?.Invoke(true);
            View.RemoveStyleClass(_singlePlayerView.Timer, "timer-red");
            View.ShowElement(_singlePlayerView.GameOverPanel);
            _singlePlayerView.SetGameOverText(score);
        }
    }
}