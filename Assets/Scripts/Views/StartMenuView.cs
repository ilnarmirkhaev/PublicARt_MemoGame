using Localization;
using UnityEngine.UIElements;

namespace Views
{
    public class StartMenuView : View
    {
        // UI Elements
        private Label GameTitle { get; }
        private Label Description { get; }
        public Button ReturnButton { get; }
        public Button SinglePlayerButton { get; }
        public Button MultiPlayerButton { get; }
        
        // Text localization
        private LocalizedString _title;
        private LocalizedString _description;
        private LocalizedString _singlePlayer;
        private LocalizedString _multiPlayer;


        public StartMenuView(UIDocument document, string name) : base(document, name)
        {
            InitLocalizationText();
            GameTitle = Main.Q<Label>("GameTitle");
            Description = Main.Q<Label>("Description");
            
            ReturnButton = Main.Q<Button>("ReturnButton");
            SinglePlayerButton = Main.Q<Button>("SinglePlayerButton");
            MultiPlayerButton = Main.Q<Button>("MultiPlayerButton");
        }

        protected override void ChangeLanguage()
        {
            SetUIText(GameTitle, _title.Value);
            SetUIText(Description, _description.Value);
            SetUIText(SinglePlayerButton, _singlePlayer.Value);
            SetUIText(MultiPlayerButton, _multiPlayer.Value);
        }

        protected sealed override void InitLocalizationText()
        {
            _title = new LocalizedString("Игра «Стрит-арт Мемо»", "“Street Art Memo” Game");
            _description = new LocalizedString(
                "Стрит-арт Мемо — это популярная игра, развивающая память и внимательность. " 
                + "Правила несложные и быстро усваиваются: нужно находить пары среди карт с разными изображениями", 
                "Street art Memo is a popular game that develops memory and attentiveness. " 
                + "The rules are simple and quickly learned: you need to find pairs among the cards with the different images."
            );
            _singlePlayer = new LocalizedString("Игра на время", "1 player");
            _multiPlayer = new LocalizedString("2 игрока", "2 players");
        }
    }
}