using Localization;
using UnityEngine.UIElements;

namespace Views
{
    public class SinglePlayerView : View
    {
        // UI Elements
        // Top panel
        public VisualElement TopPanel { get; }
        public Button ReturnButton { get; }
        public Label Timer { get; }
        public VisualElement Score { get; }
        public Label ScoreText { get; }

        // Start panel
        public VisualElement StartPanel { get; }
        private Label StartPanelText { get; }
        public Button StartButton { get; }
        
        // Exit panel
        public VisualElement ExitPanel { get; }
        private Label ExitPanelText { get; }
        public Button ContinueButton { get; }
        public Button ExitButton { get; }
        
        // Game Over Panel
        public VisualElement GameOverPanel { get; }
        private Label GameOverText { get; }
        public Button PlayAgainButton { get; }
        public Button ToBeginningButton { get; }
        
        // Misc
        public VisualElement Transparent { get; }
        public VisualElement Counter { get; }
        public Label CounterText { get; }
        
        // Text localization
        private LocalizedString _startPanelText;
        private LocalizedString _startButtonText;
        private LocalizedString _exitPanelText;
        private LocalizedString _continueButtonText;
        private LocalizedString _exitButtonText;

        private LocalizedString _playAgainButtonText;
        private LocalizedString _toBeginningButtonText;
        
        public SinglePlayerView(UIDocument document, string name) : base(document, name)
        {
            InitLocalizationText();
            TopPanel = Main.Q<VisualElement>("TopPanel");
            ReturnButton = Main.Q<Button>("ReturnButton");
            Timer = Main.Q<Label>("Timer");
            Score = Main.Q<VisualElement>("Score");
            ScoreText = Main.Q<Label>("ScoreText");

            StartPanel = Main.Q<VisualElement>("StartPanel");
            StartPanelText = StartPanel.Q<Label>();
            StartButton = Main.Q<Button>("StartButton");
            
            ExitPanel = Main.Q<VisualElement>("ExitPanel");
            ExitPanelText = ExitPanel.Q<Label>();
            ContinueButton = Main.Q<Button>("ContinueButton");
            ExitButton = Main.Q<Button>("ExitButton");

            GameOverPanel = Main.Q<VisualElement>("GameOverPanel");
            GameOverText = Main.Q<Label>("GameOverText");
            PlayAgainButton = Main.Q<Button>("PlayAgainButton");
            ToBeginningButton = Main.Q<Button>("ToBeginningButton");
            
            Transparent = Main.Q<VisualElement>("Transparent");
            Counter = Main.Q<VisualElement>("Counter");
            CounterText = Main.Q<Label>("CounterText");
        }
        
        protected override void ChangeLanguage()
        {
            SetUIText(StartPanelText, _startPanelText.Value);
            SetUIText(StartButton, _startButtonText.Value);
            SetUIText(ExitPanelText, _exitPanelText.Value);
            SetUIText(ContinueButton, _continueButtonText.Value);
            SetUIText(ExitButton, _exitButtonText.Value);
            SetUIText(PlayAgainButton, _playAgainButtonText.Value);
            SetUIText(ToBeginningButton, _toBeginningButtonText.Value);
        }

        protected sealed override void InitLocalizationText()
        {
            _startPanelText = new LocalizedString(
                "На игровом поле 16 карточек рубашкой вверх, успей найти 8 пар как можно скорее.\n\nУдачи!",
                "There are 16 face down cards on the playing field, hurry up to find 8 pairs as soon as possible.\n\nGood luck!"
            );

            _startButtonText = new LocalizedString("СТАРТ", "PLAY");

            _continueButtonText = new LocalizedString("Продолжить играть", "Continue");

            _exitPanelText = new LocalizedString(
                "Уверен, что хочешь выйти?",
                "Are you sure you want to exit the game?"
            );

            _exitButtonText = new LocalizedString("Выйти", "Exit");
            _playAgainButtonText = new LocalizedString("Играть ещё раз", "Play again");
            _toBeginningButtonText = new LocalizedString("В начало", "Back to menu");
        }

        public void SetGameOverText(int score)
        {
            string result;
            if (LocalizedString.CurrentLanguage == LocalizedString.Language.RU)
            {
                result = $"Ты набрал {FormatScore(score)}.\n";
                result += score switch
                {
                    <= 2 => "Лучшее впереди.\nЕще раз?",
                    <= 6 => "Молодец! Еще раз?",
                    _ => "Отлично!"
                };
            }
            else
            {
                result = $"You scored {FormatScore(score)}.\n";
                result += score switch
                {
                    <= 2 => "Don't be upset!  Try again!",
                    <= 6 => "Well done! Try again!",
                    _ => "Excellent!"
                };
            }

            GameOverText.text = result;
        }

        private static string FormatScore(int score)
        {
            if (LocalizedString.CurrentLanguage == LocalizedString.Language.RU)
                return score switch
                {
                    1 => "1 очко",
                    >= 2 and <= 4 => $"{score} очка",
                    _ => $"{score} очков"
                };
            else
                return score switch
                {
                    1 => "1 point",
                    _ => $"{score} points"
                };
        }
    }
}