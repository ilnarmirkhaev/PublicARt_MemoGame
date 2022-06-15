using System.Collections.Generic;
using Localization;
using UnityEngine;
using UnityEngine.UIElements;
using ViewControllers;

namespace Core
{
    public class ViewsInitializer : MonoBehaviour
    {
        [SerializeField] private UIDocument document;
    
        private void Awake()
        {
            var viewControllers = GetViewControllers();
        
            InitControllers(viewControllers);
        
            ViewsManager.Init(viewControllers);
        }

        private List<ViewController> GetViewControllers()
        {
            return new List<ViewController>(GetComponents<ViewController>());
        }

        private void InitControllers(List<ViewController> viewControllers)
        {
            foreach (var vc in viewControllers)
            {
                vc.Init(document);
            }
        }
    }
}
