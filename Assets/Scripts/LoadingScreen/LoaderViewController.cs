using System;
using Android;
using SceneLoaders;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using ViewControllers;
using Views;

namespace LoadingScreen
{
    public class LoaderViewController : ViewController
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private SceneLoader sceneLoader;
        
        
        private LoaderView _loaderView;
        private Label _progressLabel;

        public static event Action OnDownloadRequested;

        protected override View View
        {
            get => _loaderView;
            set => _loaderView = value is LoaderView view ? view : null;
        }

        private void Awake()
        {
            Init(uiDocument);
            _progressLabel = _loaderView.Progress;
        }

        private void OnEnable()
        {
            _loaderView.ReturnButton.clicked += Exit;
            AndroidSettings.OnBackButtonPressed += Exit;
            
            _loaderView.DownloadButton.clicked += RequestDownload;
            SceneLoader.OnSceneLoaded += Disable;
            SceneLoader.OnDownloadSizeReceived += SetDownloadSizeText;
        }

        private void OnDisable()
        {
            _loaderView.ReturnButton.clicked -= Exit;
            AndroidSettings.OnBackButtonPressed -= Exit;
            
            _loaderView.DownloadButton.clicked -= RequestDownload;
            SceneLoader.OnSceneLoaded -= Disable;
            SceneLoader.OnDownloadSizeReceived -= SetDownloadSizeText;
        }

        private void RequestDownload()
        {
            View.HideElement(_loaderView.DownloadRequestPanel);
            View.ShowElement(_loaderView.LoadingPanel);
            OnDownloadRequested?.Invoke();
        }

        private void SetDownloadSizeText(long bytes)
        {
            if (bytes <= 0)
                Disable();
            else
            {
                View.ShowElement(_loaderView.ReturnButton);
                View.ShowElement(_loaderView.DownloadRequestPanel);
                _loaderView.SetDownloadSizeText(bytes);
            }
        }

        private void Update()
        {
            _progressLabel.text = $"{sceneLoader.progress}%";
        }

        public override void Init(UIDocument document)
        {
            View = new LoaderView(document, viewName);
        }
        
        private static void Exit() => SceneManager.LoadScene(0);

        private void Disable() => gameObject.SetActive(false);
    }
}