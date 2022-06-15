using System;
using Cards;
using Core;
using ViewControllers;

namespace GameModes
{
    public class SinglePlayerGame : GameMode
    {
        private int _points;

        private int Points
        {
            get => _points;
            set
            {
                _points = value;
                if (_points == 8)
                    GameEnd();
            }
        }
        
        public static event Action<int> OnScored;
        public static event Action<int> OnGameEnded;

        private void OnEnable()
        {
            SinglePlayerViewController.OnGameStarted += GameStart;
            CardSelector.OnCardGuessed += Score;
            ViewTimer.OnTimerEnded += GameEnd;
        }

        private void OnDisable()
        {
            SinglePlayerViewController.OnGameStarted -= GameStart;
            CardSelector.OnCardGuessed -= Score;
            ViewTimer.OnTimerEnded -= GameEnd;
        }

        protected override void Score()
        {
            Points += 1;
            OnScored?.Invoke(Points);
        }

        protected override void GameStart()
        {
            Points = 0;
        }

        protected override async void GameEnd()
        {
            await WaitUntil(() => CardSelector.IsComparing == false);
            OnGameEnded?.Invoke(Points);
        }
    }
}