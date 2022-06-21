using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AudioPlayerTemplate
{
    public class AudioPlayerTest : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private VisualElement _screenView;

        // UI Elements
        public VisualElement Panel1 { get; set; }
        public VisualElement Panel2 { get; set; }

        private Button Button1 { get; set; }
        private Button Button2 { get; set; }
        private VisualElement AudioPlayerView1 { get; set; }
        private VisualElement AudioPlayerView2 { get; set; }

        private AudioPlayer _audioPlayer1;
        private AudioPlayer _audioPlayer2;

        [SerializeField] private AudioClip clip1;
        [SerializeField] private AudioClip clip2; 

        private void Awake()
        {
            _screenView = uiDocument.rootVisualElement.Q<VisualElement>("LoadingScreen");

            Panel1 = _screenView.Q<VisualElement>("Panel1");
            Button1 = Panel1.Q<Button>("DownloadButton");
            AudioPlayerView1 = Panel1.Q<VisualElement>("AudioPlayer");
            _audioPlayer1 = new AudioPlayer(AudioPlayerView1, Button1);
            
            
            Panel2 = _screenView.Q<VisualElement>("Panel2");
            Button2 = Panel2.Q<Button>("DownloadButton");
            AudioPlayerView2 = Panel2.Q<VisualElement>("AudioPlayer");
            _audioPlayer2 = new AudioPlayer(AudioPlayerView2, Button2);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(5);
            _audioPlayer1.SetAudioClip(clip1);
            
            yield return new WaitForSeconds(5);
            _audioPlayer2.SetAudioClip(clip2);
        }

        private void Update()
        {
            // Escape to reload scene
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
            // Right Mouse Button to swap clips
            if (Input.GetMouseButtonDown(1))
            {
                var clip = _audioPlayer1.AudioClip;
                _audioPlayer1.SetAudioClip(_audioPlayer2.AudioClip);
                _audioPlayer2.SetAudioClip(clip);
            }
        }

        private void OnEnable()
        {
            Button1.clicked += _audioPlayer1.Open;
            Button2.clicked += _audioPlayer2.Open;
        }

        private void OnDisable()
        {
            Button1.clicked -= _audioPlayer1.Open;
            Button2.clicked -= _audioPlayer2.Open;
        }
    }
}