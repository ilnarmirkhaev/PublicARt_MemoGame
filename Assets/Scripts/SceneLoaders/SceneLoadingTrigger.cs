using System;
using UnityEngine;

namespace SceneLoaders
{
    public class SceneLoadingTrigger : MonoBehaviour
    {
        public static event Action OnSceneOpened;

        private void Start()
        {
            OnSceneOpened?.Invoke();
        }
    }
}
