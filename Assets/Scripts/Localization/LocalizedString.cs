using System;

namespace Localization
{
    [Serializable]
    public class LocalizedString
    {
        public enum Language
        {
            RU,
            EN
        }

        // public delegate void LanguageChangeHandler();
        //
        // public static LanguageChangeHandler OnLanguageChange;
        public static event Action OnLanguageChanged;

        public static Language CurrentLanguage;

        private string _ru;
        private string _en;

        public string Value => CurrentLanguage == Language.RU ? _ru : _en;

        public LocalizedString(string ru, string en)
        {
            _ru = ru;
            _en = en;
        }

        public static void CheckLanguage()
        {
            OnLanguageChanged?.Invoke();
        }

        public static void ChangeLanguage(Language newLanguage)
        {
            if (newLanguage != CurrentLanguage)
            {
                CurrentLanguage = newLanguage;
                OnLanguageChanged?.Invoke();
            }
        }
    }
}