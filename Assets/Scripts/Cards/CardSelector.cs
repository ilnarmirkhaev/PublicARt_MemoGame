using System;
using UnityEngine;
using ViewControllers;

namespace Cards
{
    public class CardSelector : MonoBehaviour
    {
        private Camera _camera;
        [SerializeField] private Transform cardDestination;
        private Card _previousCard;
        private bool _canSelect = true;
        private static bool _isPaused;
        public static bool IsComparing;

        public static event Action OnCardGuessed;
        public static event Action OnTurnEnded;
         
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            SinglePlayerViewController.OnGamePaused += _pauseGame;
            SinglePlayerViewController.OnGameStarted += ResetSelector;
            SinglePlayerViewController.OnGameExited += ResetSelector;
            
            MultiPlayerViewController.OnGamePaused += _pauseGame;
            MultiPlayerViewController.OnGameStarted += ResetSelector;
            MultiPlayerViewController.OnGameExited += ResetSelector;
        }

        private readonly Action<bool> _pauseGame = (value) => { _isPaused = value; };

        private void OnDisable()
        {
            SinglePlayerViewController.OnGamePaused -= _pauseGame;
            SinglePlayerViewController.OnGameStarted -= ResetSelector;
            SinglePlayerViewController.OnGameExited -= ResetSelector;
            
            MultiPlayerViewController.OnGamePaused -= _pauseGame;
            MultiPlayerViewController.OnGameStarted -= ResetSelector;
            MultiPlayerViewController.OnGameExited -= ResetSelector;
        }

        private void Update()
        {
            if (_isPaused || !_canSelect) return;
            
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                var ray = _camera.ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray, out var hit,100))
                {
                    var card = hit.collider.GetComponent<Card>();
                    if (!card.IsFlipping)
                        CompareCard(card, 0.375f, 0.25f);
                }
            }
        }

        private async void CompareCard(Card card, float forwardDuration, float backwardDuration)
        {
            if (card.Texture == null || !card.CanFlip) return;
            
            IsComparing = true;
            _canSelect = false;
            
            await card.Flip(forwardDuration, backwardDuration, cardDestination.position);
            card.CanFlip = false;

            if (_previousCard == null)
            {
                // First card selected
                _previousCard = card;
            }
            else if (_previousCard.Texture == card.Texture)
            {
                // Second card selected, are equal
                OnCardGuessed?.Invoke();
                
                _previousCard.Jump(0.5f);
                card.Jump(0.5f);
                
                _previousCard = null;
            }
            else
            {
                // Second card selected, not equal
                _previousCard.CanFlip = true;
                card.CanFlip = true;

                _previousCard.Flip(forwardDuration, backwardDuration);
                card.Flip(forwardDuration, backwardDuration);
                
                _previousCard = null;
                
                OnTurnEnded?.Invoke();
            }

            _canSelect = true;
            IsComparing = false;
        }

        private void ResetSelector()
        {
            _previousCard = null;
            _canSelect = true;
            _isPaused = true;
            IsComparing = false;
        }
    }
}