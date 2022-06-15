#if UNITY_ANDROID
#define USE_ANDROID
#endif

using System;
using UnityEngine;

namespace Android
{
    public class AndroidSettings : MonoBehaviour
    {
#if USE_ANDROID
        public static AndroidSettings Instance { get; private set; }
        public static event Action OnBackButtonPressed;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            
            ApplicationChrome.statusBarState = ApplicationChrome.States.VisibleOverContent;
            ApplicationChrome.statusBarColor = 0xFF141517;
            
            ApplicationChrome.navigationBarState = ApplicationChrome.States.VisibleOverContent;
            ApplicationChrome.navigationBarColor = 0xFF141517;
            
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Application.platform == RuntimePlatform.Android) {
        
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    OnBackButtonPressed?.Invoke();
                }
            }
        }
#endif
    }
}
