using System;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace LoaderTemplate
{
    public class LoaderTest : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        
        private VisualElement _screenView;
        private Loader _loader;
        
        // UI Elements
        
        public Button DownloadButton { get; set; }
        public VisualElement LoaderView { get; set; }
        public VisualElement DownloadView { get; set; }
        public VisualElement ScalingBox { get; set; }

        private void Awake()
        {
            _screenView = uiDocument.rootVisualElement.Q<VisualElement>("LoadingScreen");
            DownloadButton = _screenView.Q<Button>("DownloadButton");
            DownloadView = _screenView.Q<VisualElement>("DownloadRequest");
            LoaderView = _screenView.Q<VisualElement>("Loader");
            ScalingBox = _screenView.Q<VisualElement>("ScalingBox");
            
            _loader = new Loader(LoaderView);
        }


        private void OnEnable()
        {
            DownloadButton.clicked += ShowLoader;
        }

        private void OnDisable()
        {
            DownloadButton.clicked -= ShowLoader;
        }

        private void ShowLoader()
        {
            DownloadView.style.display = DisplayStyle.None;
            ScalingBox.style.display = DisplayStyle.Flex;
            _loader.SetActive(true);
        }
    }
}