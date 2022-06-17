using System.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core
{
    public static class AnimateUI
    {
        public static async Task AnimateLabel(Label label, int startFontSize, int endFontSize, int number,
            float duration)
        {
            label.style.fontSize = startFontSize;
            var size = label.style.fontSize.value.value;
            label.text = number.ToString();

            var halfDur = duration * 0.5f;

            var tween =
                DOTween.To(() => size, x => label.style.fontSize = x, endFontSize, halfDur);
            tween.SetLoops(2, LoopType.Yoyo);
            await tween.AsyncWaitForCompletion();
        }

        public static TweenerCore<Vector3, Vector3, VectorOptions> AnimateRotation(VisualElement element, float angle,
            LoopType loopType = LoopType.Yoyo, Ease ease = Ease.InOutCirc)
        {
            element.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            var tween = DOTween.To(
                () => element.worldTransform.rotation.eulerAngles,
                x => element.transform.rotation = Quaternion.Euler(x),
                new Vector3(0, 0, -angle),
                1f
            ).SetLoops(-1, loopType).SetEase(ease);

            return tween;
        }

        public static void AnimateWidth(VisualElement element, float max, float min, LoopType loopType = LoopType.Yoyo,
            Ease ease = Ease.InOutCirc)
        {
            element.style.width = max;

            var tween = DOTween.To(
                () => element.style.width.value.value,
                x => element.style.width = x,
                min,
                1f
            ).SetLoops(-1, loopType).SetEase(ease);
        }

        public static async Task AnimateHeight(VisualElement element, float start, float end, float duration = 1f,
            int loopCount = -1, LoopType loopType = LoopType.Yoyo, Ease ease = Ease.InOutCirc)
        {
            element.style.height = start;

            var tween = DOTween.To(
                () => element.style.height.value.value,
                x => element.style.height = x,
                end,
                duration
            ).SetLoops(loopCount, loopType).SetEase(ease);
            await tween.AsyncWaitForCompletion();
        }
    }
}