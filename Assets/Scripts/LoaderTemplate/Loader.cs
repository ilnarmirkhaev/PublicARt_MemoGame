using Core;
using DG.Tweening;
using UnityEngine.UIElements;

namespace LoaderTemplate
{
    public class Loader
    {
        private VisualElement _loaderView;
        private VisualElement _icon;
        private Tweener _tweener;

        public Loader(VisualElement loaderView)
        {
            _loaderView = loaderView;

            _icon = _loaderView.Q<VisualElement>("CanIcon");
        }

        public void SetActive(bool value)
        {
            _loaderView.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;

            if (value)
                _tweener = AnimateUI.AnimateRotation(_icon, 20);
            else
                _tweener.Kill();
        }
    }
}