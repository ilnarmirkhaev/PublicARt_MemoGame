using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoaders
{
    public class MemoPlay : MonoBehaviour
    {
        public void Play() => SceneManager.LoadScene(1);
    }
}