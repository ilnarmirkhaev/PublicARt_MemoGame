using System;
using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Views;

namespace Core
{
    public static class ViewTimer
    {
        public static event Action OnTimerEnded;
        public static async Task Countdown(Label counter, int number)
        {
            // counter.style.fontSize = 64;
            //
            // var size = counter.style.fontSize.value.value;
        
            while (number > 0)
            {
                await AnimateUI.AnimateLabel(counter, 64, 100, number, 1);
                number--;
                // counter.text = number.ToString();
                // number--;
                //
                // var tween =
                //     DOTween.To(() => size, x=> counter.style.fontSize = x, 100, 0.5f);
                // tween.SetLoops(2, LoopType.Yoyo);
                // await tween.AsyncWaitForCompletion();
            }
        }

        public static IEnumerator TimerCoroutine(Label timer, int seconds)
        {
            var isHighlighted = false;
        
            while (seconds >= 0)
            {
                if (seconds <= 5 && !isHighlighted)
                {
                    View.AddStyleClass(timer, "timer-red");
                    isHighlighted = true;
                }
            
                timer.text = SecondsToTime(seconds);
                seconds--;
            
                yield return new WaitForSeconds(1);
            }
            
            OnTimerEnded?.Invoke();
        }

        private static string SecondsToTime(int seconds)
        {
            var minutes = seconds / 60;

            return $"{minutes}:{FormatSeconds(seconds % 60)}";
        }

        private static string FormatSeconds(int seconds)
        {
            return seconds < 10 ? "0" + seconds : seconds.ToString();
        }
    }
}