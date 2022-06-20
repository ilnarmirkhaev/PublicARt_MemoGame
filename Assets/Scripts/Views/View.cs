using Localization;
using UnityEngine;
using UnityEngine.UIElements;

namespace Views
{
    public class View
    {
        public readonly VisualElement Main;

        protected View(UIDocument document, string viewName)
        {
            Main = document.rootVisualElement.Q<VisualElement>(viewName);
            LocalizedString.OnLanguageChanged += ChangeLanguage;

            if (Main != null) return;
            
            Debug.LogError("Missing main visual element or name is incorrect");
        }

        public static void ShowElement(VisualElement element)
        {
            element.style.display = DisplayStyle.Flex;
        }

        public static void HideElement(VisualElement element)
        {
            element.style.display = DisplayStyle.None;
        }

        public static bool IsVisible(VisualElement element)
        {
            return element.style.display == DisplayStyle.Flex;
        }

        public static void AddStyleClass(VisualElement element, string className)
        {
            element.EnableInClassList(className, true);
        }
        
        public static void RemoveStyleClass(VisualElement element, string className)
        {
            element.EnableInClassList(className, false);
        }

        protected virtual void ChangeLanguage(){}
        
        protected virtual void InitLocalizationText(){}
        
        protected static void SetUIText(Label label, string text)
        {
            label.text = text;
        }
        
        protected static void SetUIText(Button button, string text)
        {
            button.text = text;
        }
        
        public static void Check(VisualElement element)
        {
            if (element == null) Debug.LogWarning($"{nameof(element)} is missing");
        }
    }
}