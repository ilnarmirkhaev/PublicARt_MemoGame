using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameModes
{
    public abstract class GameMode : MonoBehaviour
    {
        protected abstract void Score();

        protected abstract void GameStart();

        protected abstract void GameEnd();

        protected static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, 
                Task.Delay(timeout))) 
                throw new TimeoutException();
        }
    }
}