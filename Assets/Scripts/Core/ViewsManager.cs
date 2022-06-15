using System;
using System.Collections.Generic;
using UnityEngine;
using ViewControllers;

namespace Core
{
    public static class ViewsManager
    {
        private static List<ViewController> _viewControllers;
        private static Stack<ViewController> _history;
        public static ViewController Current;

        public static event Action OnExited;
    
        public static void Init(List<ViewController> viewControllers)
        {
            _viewControllers = viewControllers;
            _history = new Stack<ViewController>();
        
            Open(typeof(StartMenuViewController));
        }

        public static void Open(Type controllerType)
        {
            foreach (var vc in _viewControllers)
            {
                if (vc.GetType() != controllerType) continue;
            
                if (Current != null)
                    Current.Close();
                
                vc.Open();
                AddToHistory(vc);
                Current = vc;
            
                return;
            }
        
            Debug.LogWarning($"{controllerType} is missing");
        }

        public static void Return()
        {
            if (_history.Count == 0) return;
        
            if (Current != null)
                Current.Close();

            _history.Pop();

            if (_history.TryPeek(out var vc))
            {
                vc.Open();
                Current = vc;
            }
            else
            {
                OnExited?.Invoke();
            }
        }

        private static void AddToHistory(ViewController viewController)
        {
            _history.Push(viewController);
        }
    }
}