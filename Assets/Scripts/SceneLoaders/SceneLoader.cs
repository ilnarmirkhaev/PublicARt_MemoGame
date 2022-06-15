using System;
using System.Collections;
using System.Threading.Tasks;
using Core;
using LoadingScreen;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using ViewControllers;

namespace SceneLoaders
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        
        [SerializeField] private string scenePath;

        private AsyncOperationHandle<SceneInstance> _sceneLoadingHandle;
        private AsyncOperationHandle _downloadDependenciesHandle;
        public static event Action OnSceneLoaded;
        public static event Action<long> OnDownloadSizeReceived;
        public int progress;

        private bool _clearLoadedScene;
        private SceneInstance _loadedScene;

        private bool _dependenciesDownloaded;

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

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            ViewsManager.OnExited += CloseScene;
            ViewController.OnSceneReloaded += ReloadScene;
            LoaderViewController.OnDownloadRequested += StartDownload;
            SceneLoadingTrigger.OnSceneOpened += DownloadAndStartGame;
        }

        private void OnDisable()
        {
            ViewsManager.OnExited -= CloseScene;
            ViewController.OnSceneReloaded -= ReloadScene;
            LoaderViewController.OnDownloadRequested -= StartDownload;
            SceneLoadingTrigger.OnSceneOpened -= DownloadAndStartGame;
        }

        private void Update()
        {
            GetDownloadProgress(!_dependenciesDownloaded ? _downloadDependenciesHandle : _sceneLoadingHandle);
        }

        private async void DownloadAndStartGame()
        {
            if (_dependenciesDownloaded)
            {
                LoadScene();
                return;
            }
            
            var size = await GetDownloadSize();
            // var size = 123;

            OnDownloadSizeReceived?.Invoke(size);
            Debug.Log($"Size received: {size} bytes");

            if (size <= 0)
            {
                LoadScene();
            }
        }

        private void GetDownloadProgress(AsyncOperationHandle handle)
        {
            if (handle.IsValid() && !handle.IsDone)
                progress = (int)(handle.PercentComplete * 100);
        }

        private async Task<long> GetDownloadSize()
        {
            var getDownloadSize = Addressables.GetDownloadSizeAsync(scenePath);
            await getDownloadSize.Task;
            Debug.Log(getDownloadSize.Status);

            if (getDownloadSize.Status == AsyncOperationStatus.Succeeded)
                return getDownloadSize.Result;

            return -1;
        }

        private void StartDownload() => StartCoroutine(DownloadDependencies());

        private IEnumerator DownloadDependencies()
        {
            if (_dependenciesDownloaded) yield break;
            
            _downloadDependenciesHandle = Addressables.DownloadDependenciesAsync(scenePath);
            progress = (int)(_downloadDependenciesHandle.PercentComplete * 100);

            _downloadDependenciesHandle.Completed += _ =>
            {
                LoadScene();
                _dependenciesDownloaded = true;
                
                Addressables.Release(_downloadDependenciesHandle);
            };
            
            yield return _downloadDependenciesHandle;
        }

        private void LoadScene()
        {
            if (_clearLoadedScene)
                UnloadScene();
            
            _sceneLoadingHandle = Addressables.LoadSceneAsync(scenePath, LoadSceneMode.Additive);

            _sceneLoadingHandle.Completed += (handle) =>
            {
                _dependenciesDownloaded = true;
                _clearLoadedScene = true;
                _loadedScene = handle.Result;
                OnSceneLoaded?.Invoke();
                Debug.Log($"Scene {_loadedScene.Scene.name} loaded");
            };
        }

        private void UnloadScene()
        {
            Debug.Log($"Unloading {_loadedScene.Scene.name} scene");
            Addressables.UnloadSceneAsync(_loadedScene).Completed += _ =>
            {
                _clearLoadedScene = false;
                _loadedScene = new SceneInstance();
            };
        }

        private void CloseScene()
        {
            SceneManager.LoadScene(0);
            Debug.LogWarning("Successfully closed scene.");
            
            UnloadScene();
        }

        private void ReloadScene()
        {
            LoadScene();
            Debug.LogWarning("Successfully reloaded scene.");
        }
    }
}