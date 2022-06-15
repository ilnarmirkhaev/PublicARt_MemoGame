using Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using ViewControllers;

namespace SceneLoaders
{
    public class MemoSceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference scene;
        private AsyncOperationHandle<SceneInstance> _asyncOperationHandle;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            ViewsManager.OnExited += CloseScene;
            ViewController.OnSceneReloaded += LoadScene;
        }

        private void OnDisable()
        {
            ViewsManager.OnExited -= CloseScene;
            ViewController.OnSceneReloaded -= LoadScene;
        }

        public void PlayScene()
        {
            if (!scene.RuntimeKeyIsValid())
            {
                Debug.LogError("Invalid key " + scene.RuntimeKey);
                return;
            }

            LoadScene();
        }

        private void LoadScene()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
                Debug.LogWarning("Reopening the scene.");
            
            Addressables.LoadSceneAsync(scene).Completed += OnSceneLoaded;
        }
        private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"Successfully loaded {handle.Result.Scene.name} scene.");
            }
            else
            {
                Debug.LogError("Addressables async scene load failed.");
            }
        }

        private void CloseScene()
        {
            SceneManager.LoadScene(0);
            Debug.LogWarning("Successfully closed scene.");
            
            // SceneManager.LoadSceneAsync(0).completed += _ =>
            // {
            //     Resources.UnloadUnusedAssets();
            //     Debug.LogWarning("Successfully closed scene.");
            // };
        }
    }
}