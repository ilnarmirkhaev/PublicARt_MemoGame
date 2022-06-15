using System;
using UnityEngine;
using UnityEngine.UIElements;
using Views;

namespace ViewControllers
{
    public abstract class ViewController : MonoBehaviour
    {
        protected virtual View View { get; set; }
        
        [SerializeField] protected string viewName;
        
        public abstract void Init(UIDocument document);

        public static event Action OnSceneReloaded;

        protected virtual void DefaultScreen()
        {
        }

        public void Open()
        {
            DefaultScreen();
            View.Main.style.display = DisplayStyle.Flex;
        }

        public void Close()
        {
            View.Main.style.display = DisplayStyle.None;
        }
        
        protected static void ReloadScene()
        {
            OnSceneReloaded?.Invoke();
        }
    }
}