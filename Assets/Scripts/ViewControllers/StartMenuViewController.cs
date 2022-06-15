using System;
using Android;
using Core;
using UnityEngine.UIElements;
using Views;

namespace ViewControllers
{
    public class StartMenuViewController : ViewController
    {
        private StartMenuView _startMenuView;
        private SinglePlayerViewController _singlePlayerViewController;
        private MultiPlayerViewController _multiPlayerViewController;
        protected override View View
        {
            get => _startMenuView;
            set => _startMenuView = value is StartMenuView view ? view : null;
        }

        public static event Action OnGameEntered;

        private void Awake()
        {
            _singlePlayerViewController = GetComponent<SinglePlayerViewController>();
            _multiPlayerViewController = GetComponent<MultiPlayerViewController>();
        }

        public override void Init(UIDocument document)
        {
            View = new StartMenuView(document, viewName);
            InitButtonListeners();
        }

        protected virtual void InitButtonListeners()
        {
            _startMenuView.ReturnButton.clicked += ViewsManager.Return;
            AndroidSettings.OnBackButtonPressed += Return;
            _startMenuView.SinglePlayerButton.clicked += () =>
            {
                _singlePlayerViewController.enabled = true;
                _multiPlayerViewController.enabled = false;
                ViewsManager.Open(typeof(SinglePlayerViewController));
                OnGameEntered?.Invoke();
            };
            _startMenuView.MultiPlayerButton.clicked += () =>
            {
                _singlePlayerViewController.enabled = false;
                _multiPlayerViewController.enabled = true;
                ViewsManager.Open(typeof(MultiPlayerViewController));
                OnGameEntered?.Invoke();
            };
        }

        private void Return()
        {
            if (ViewsManager.Current == this)
                ViewsManager.Return();
        }
    }
}