using System;
using Cards;
using ViewControllers;

namespace GameModes
{
    public class MultiPlayerGame : GameMode
    {
        private int _points1;
        private int _points2;

        private int Points1
        {
            get => _points1;
            set
            {
                _points1 = value;
                CheckForGameOver();
            }
        }
        
        private int Points2
        {
            get => _points2;
            set
            {
                _points2 = value;
                CheckForGameOver();
            }
        }

        private bool _isPlayerOneTurn = true;
        
        public static event Action<int, bool> OnScored;
        public static event Action<bool> OnTurnEnded;
        public static event Action<int, int> OnGameEnded;

        private void OnEnable()
        {
            MultiPlayerViewController.OnGameStarted += GameStart;
            CardSelector.OnCardGuessed += Score;
            CardSelector.OnTurnEnded += EndTurn;
        }

        private void OnDisable()
        {
            MultiPlayerViewController.OnGameStarted -= GameStart;
            CardSelector.OnCardGuessed -= Score;
            CardSelector.OnTurnEnded -= EndTurn;
        }

        protected override void Score()
        {
            if (_isPlayerOneTurn)
            {
                Points1 += 1;
                OnScored?.Invoke(Points1, _isPlayerOneTurn);
            }
            else
            {
                Points2 += 1;
                OnScored?.Invoke(Points2, _isPlayerOneTurn);
            }
        }

        private void EndTurn()
        {
            _isPlayerOneTurn = !_isPlayerOneTurn;
            OnTurnEnded?.Invoke(_isPlayerOneTurn);
        }

        protected override void GameStart()
        {
            Points1 = 0;
            Points2 = 0;

            _isPlayerOneTurn = true;
        }

        private void CheckForGameOver()
        {
            if (_points1 + _points2 >= 8)
                GameEnd();
        }

        protected override async void GameEnd()
        {
            await WaitUntil(() => CardSelector.IsComparing == false);
            OnGameEnded?.Invoke(Points1, Points2);
        }
    }
}