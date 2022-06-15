using Localization;
using UnityEngine.UIElements;

namespace Views
{
    public class MultiPlayerView : View
    {
        // Top panel
        public VisualElement TopPanel { get; }
        public Button ReturnButton { get; }
        
        // Players panels
        public VisualElement PlayerOnePanel { get; }
        public Label PlayerOneText { get; }
        public VisualElement ScoreOne { get; }
        public Label ScoreOneText { get; }
        
        public VisualElement PlayerTwoPanel { get; }
        public Label PlayerTwoText { get; }
        public VisualElement ScoreTwo { get; }
        public Label ScoreTwoText { get; }

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
        public Label GameOverText { get; }
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

        private LocalizedString _playerOne;
        private LocalizedString _playerTwo;
        
        public MultiPlayerView(UIDocument document, string name) : base(document, name)
        {
            InitLocalizationText();
            TopPanel = Main.Q<VisualElement>("TopPanel");
            ReturnButton = Main.Q<Button>("ReturnButton");

            PlayerOnePanel = Main.Q<VisualElement>("PlayerOnePanel");
            PlayerOneText = PlayerOnePanel.Q<Label>();
            ScoreOne = Main.Q<VisualElement>("ScoreOne");
            ScoreOneText = Main.Q<Label>("ScoreOneText");
            PlayerTwoPanel = Main.Q<VisualElement>("PlayerTwoPanel");
            PlayerTwoText = PlayerTwoPanel.Q<Label>();
            ScoreTwo = Main.Q<VisualElement>("ScoreTwo");
            ScoreTwoText = Main.Q<Label>("ScoreTwoText");

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
            SetUIText(PlayerOneText, _playerOne.Value);
            SetUIText(PlayerTwoText, _playerTwo.Value);
        }

        protected sealed override void InitLocalizationText()
        {
            _startPanelText = new LocalizedString(
                "На игровом поле 16 карточек рубашкой вверх, найдите 8 пар.\nНе угадал с первого раза — попробуй ещё.\n\nУдачи!",
                "There are 16 face down cards on the playing field, you need to find 8 pairs. If you don't get it right the first time, try again.\n\nGood luck!"
            );

            _startButtonText = new LocalizedString("СТАРТ", "PLAY");

            _continueButtonText = new LocalizedString("Продолжить играть", "Continue");

            _exitPanelText = new LocalizedString(
                "Уверены, что хотите выйти?",
                "Are you sure you want to exit the game?"
            );

            _exitButtonText = new LocalizedString("Выйти", "Exit");
            _playAgainButtonText = new LocalizedString("Играть ещё раз", "Play again");
            _toBeginningButtonText = new LocalizedString("В начало", "Back to menu");
            _playerOne = new LocalizedString("Игрок 1", "Player 1");
            _playerTwo = new LocalizedString("Игрок 2", "Player 2");
        }
        
        public void SetGameOverText(int score1, int score2)
        {
            string result;
            if (LocalizedString.CurrentLanguage == LocalizedString.Language.RU)
            {
                if (score1 > score2)
                    result = "Победил — игрок 1!\n\nИгрок 2 — хорошая попытка!";
                else if (score1 < score2)
                    result = "Победил — игрок 2!\n\nИгрок 1 — хорошая попытка!";
                else
                    result = "Ничья!";
            }
            else
            {
                if (score1 > score2)
                    result = "Player 1 won!\n\nPlayer 2 — Nice try!";
                else if (score1 < score2)
                    result = "Player 2 won!\n\nPlayer 1 — Nice try!";
                else
                    result = "It's a draw!\n\nTry again!";
            }

            GameOverText.text = result;
        }
    }
}