using Core;
using Localization;
using UnityEngine.UIElements;
using Views;

namespace LoadingScreen
{
    public class LoaderView : View
    {
        public Button ReturnButton { get; }
        
        // Download Request
        public VisualElement DownloadRequestPanel { get; }
        public Label DownloadSize { get; }
        public Button DownloadButton { get; }
        
        public VisualElement LoadingPanel { get; }
        public Label Progress { get; }
        public Label LoadingLabel { get; }

        private LocalizedString _loadingText;
        private LocalizedString _downloadSizeText;
        private LocalizedString _downloadButtonText;

        public LoaderView(UIDocument document, string viewName) : base(document, viewName)
        {
            InitLocalizationText();
            
            ReturnButton = Main.Q<Button>("ReturnButton");

            DownloadRequestPanel = Main.Q<VisualElement>("DownloadRequest");
            DownloadSize = DownloadRequestPanel.Q<Label>("DownloadSize");
            DownloadButton = DownloadRequestPanel.Q<Button>("DownloadButton");

            LoadingPanel = Main.Q<VisualElement>("Loading");
            Progress = LoadingPanel.Q<Label>("Progress");
            LoadingLabel = LoadingPanel.Q<Label>("LoadingText");
            
            LoadingLabel.text = _loadingText.Value;
        }
        
        protected sealed override void InitLocalizationText()
        {
            _loadingText = new LocalizedString("Идет загрузка...", "Loading...");
            _downloadSizeText = new LocalizedString("Требуется загрузить: ", "Need to download: ");
            _downloadButtonText = new LocalizedString("Загрузить", "Download");
        }

        public void SetDownloadSizeText(long bytes)
        {
            DownloadSize.text = _downloadSizeText.Value + ByteConverter.SizeSuffix(bytes);
        }
        
        protected override void ChangeLanguage()
        {
            SetUIText(LoadingLabel, _loadingText.Value);
            SetUIText(DownloadSize, _downloadSizeText.Value);
            SetUIText(DownloadButton, _downloadButtonText.Value);
        }
    }
}