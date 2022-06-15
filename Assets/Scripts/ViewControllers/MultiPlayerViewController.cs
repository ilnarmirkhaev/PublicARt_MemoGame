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
    public class MultiPlayerViewController : ViewController
    {
        private MultiPlayerView _multiPlayerView;

        private bool _isStarted;
        private Task _countdown;

        public static event Action OnGameStarted;
        public static event Action OnGameExited;
        
        // Event for switching _canSelect in CardSelector
        public static event Action<bool> OnGamePaused;

        protected override View View
        {
            get => _multiPlayerView;
            set => _multiPlayerView = value is MultiPlayerView view ? view : null;
        }

        private void OnEnable()
        {
            MultiPlayerGame.OnScored += AddScore;
            MultiPlayerGame.OnTurnEnded += SwitchTurns;
            MultiPlayerGame.OnGameEnded += GameOver;
        }

        private void OnDisable()
        {
            MultiPlayerGame.OnScored -= AddScore;
            MultiPlayerGame.OnTurnEnded -= SwitchTurns;
            MultiPlayerGame.OnGameEnded -= GameOver;
        }

        private void SwitchTurns(bool isPlayerOneTurn)
        {
            if (isPlayerOneTurn)
            {
                View.RemoveStyleClass(_multiPlayerView.PlayerOnePanel, "player-panel-transparent");
                View.AddStyleClass(_multiPlayerView.PlayerTwoPanel, "player-panel-transparent");
            }
            else
            {
                View.RemoveStyleClass(_multiPlayerView.PlayerTwoPanel, "player-panel-transparent");
                View.AddStyleClass(_multiPlayerView.PlayerOnePanel, "player-panel-transparent");
            }
        }
        
        private async void AddScore(int score, bool isPlayerOneTurn)
        {
            if (isPlayerOneTurn)
                await AnimateUI.AnimateLabel(_multiPlayerView.ScoreOneText, 20, 32, score, 1);
            else
                await AnimateUI.AnimateLabel(_multiPlayerView.ScoreTwoText, 20, 32, score, 1);
        }

        public override void Init(UIDocument document)
        {
            View = new MultiPlayerView(document, viewName);
            InitButtonListeners();
        }

        protected override void DefaultScreen()
        {
            OnGamePaused?.Invoke(true);
            
            View.ShowElement(_multiPlayerView.Transparent);
            View.ShowElement(_multiPlayerView.TopPanel);
            View.ShowElement(_multiPlayerView.ReturnButton);
            View.ShowElement(_multiPlayerView.StartPanel);
            
            View.HideElement(_multiPlayerView.ExitPanel);
            View.HideElement(_multiPlayerView.Counter);
        }

        protected virtual void InitButtonListeners()
        {
            _multiPlayerView.StartButton.clicked += StartGame;
            _multiPlayerView.ReturnButton.clicked += Pause;
            AndroidSettings.OnBackButtonPressed += Pause;
            _multiPlayerView.ContinueButton.clicked += Resume;
            _multiPlayerView.ExitButton.clicked += Exit;
            _multiPlayerView.PlayAgainButton.clicked += StartGame;
            _multiPlayerView.ToBeginningButton.clicked += ReloadScene;
        }

        private async void StartGame()
        {
            OnGameStarted?.Invoke();

            OnGamePaused?.Invoke(true);
            
            _isStarted = true;
            
            View.HideElement(_multiPlayerView.StartPanel);
            View.HideElement(_multiPlayerView.GameOverPanel);
            View.ShowElement(_multiPlayerView.Transparent);
            
            View.HideElement(_multiPlayerView.ReturnButton);
            _multiPlayerView.ScoreOneText.text = "0";
            _multiPlayerView.ScoreTwoText.text = "0";
            View.HideElement(_multiPlayerView.PlayerOnePanel);
            View.HideElement(_multiPlayerView.PlayerTwoPanel);
            
            View.ShowElement(_multiPlayerView.Counter);
            
            _countdown = ViewTimer.Countdown(_multiPlayerView.CounterText, 3);
            
            await _countdown;
            _countdown = null;

            View.HideElement(_multiPlayerView.Counter);
            View.HideElement(_multiPlayerView.Transparent);
            
            View.ShowElement(_multiPlayerView.ReturnButton);
            View.ShowElement(_multiPlayerView.PlayerOnePanel);
            View.ShowElement(_multiPlayerView.PlayerTwoPanel);
            View.RemoveStyleClass(_multiPlayerView.PlayerOnePanel, "player-panel-transparent");
            View.AddStyleClass(_multiPlayerView.PlayerTwoPanel, "player-panel-transparent");

            OnGamePaused?.Invoke(false);
        }

        private void Pause()
        {
            if (ViewsManager.Current != this || _countdown != null) return;
            
            if (View.IsVisible(_multiPlayerView.GameOverPanel))
                return;

            OnGamePaused?.Invoke(true);
            if (!_isStarted)
                View.HideElement(_multiPlayerView.StartPanel);

            View.ShowElement(_multiPlayerView.ExitPanel);
            View.ShowElement(_multiPlayerView.Transparent);
            Time.timeScale = 0;
        }

        private void Resume()
        {
            OnGamePaused?.Invoke(false);
            if (_isStarted)
                View.HideElement(_multiPlayerView.Transparent);
            else
                View.ShowElement(_multiPlayerView.StartPanel);
            
            View.HideElement(_multiPlayerView.ExitPanel);
            Time.timeScale = 1;
        }

        private void Exit()
        {
            View.HideElement(_multiPlayerView.PlayerOnePanel);
            View.HideElement(_multiPlayerView.PlayerTwoPanel);
            OnGameExited?.Invoke();
            OnGamePaused?.Invoke(true);
            
            _isStarted = false;
            
            Time.timeScale = 1;
            // ViewsManager.Return();
            ReloadScene();
        }

        private void GameOver(int score1, int score2)
        {
            OnGamePaused?.Invoke(true);
            View.ShowElement(_multiPlayerView.GameOverPanel);
            _multiPlayerView.SetGameOverText(score1, score2);
        }
    }
}