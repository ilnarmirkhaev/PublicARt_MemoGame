using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour
    {
        private Texture _frontTexture;

        public Texture Texture
        {
            get => _frontTexture;
            private set
            {
                _frontTexture = value;
                _frontRenderer.material.mainTexture = value;
            }
        }
        private MeshRenderer _frontRenderer;

        public bool IsFlipping { get; set; } = true;
        private bool _isFirstFlip = true;

        public bool CanFlip { get; set; } = true;
        
        private Transform _transform;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private List<Task> _tweenList;

        private void Awake()
        {
            _transform = transform;
            _frontRenderer = _transform.Find("Front").GetComponent<MeshRenderer>();
            _initialPosition = _transform.position;
            _initialRotation = _transform.rotation;
        }

        private void OnEnable()
        {
            ResetCard();
        }

        public void SetTexture(Texture texture)
        {
            Texture = texture;
        }

        public void Jump(float duration)
        {
            transform.DOMoveY(3, duration * 0.5f).SetLoops(2, LoopType.Yoyo);
        }
        
        public async Task Flip(float forwardDuration, float backwardDuration, Vector3 destination = default)
        {
            if (IsFlipping || !CanFlip) return;
            
            IsFlipping = true;
            
            if (_isFirstFlip)
            {
                // FlipUp, wait, then move down
                await FlipUp(destination, forwardDuration);

                await Task.Delay(500);
            
                if (destination != Vector3.zero)
                    await FlipDown(backwardDuration);
                _isFirstFlip = false;
            }
            else
            {
                // Flip in place
                await FlipUp(_initialPosition + Vector3.up * 3, forwardDuration);
                await FlipDown(backwardDuration);
            }
            
            IsFlipping = false;
        }

        private async Task FlipUp(Vector3 destination, float duration)
        {
            var tasks = new List<Task>();
            
            if (destination != Vector3.zero)
                tasks.Add(transform.DOMove(destination, duration).AsyncWaitForCompletion());
            tasks.Add(transform.DORotate(new Vector3(180, 0, 0), duration, RotateMode.LocalAxisAdd).AsyncWaitForCompletion());
            
            await Task.WhenAll(tasks);
        }
        
        private async Task FlipDown(float duration)
        {
            if (transform.position != _initialPosition)
                await transform.DOMove(_initialPosition, duration).AsyncWaitForCompletion();
        }

        public async Task ResetCard()
        {
            CanFlip = true;
            _isFirstFlip = true;
            IsFlipping = false;

            // _transform.position = _initialPosition;
            // _transform.rotation = _initialRotation;
            var tasks = new List<Task>();
            
            if (transform.position != _initialPosition)
                tasks.Add(transform.DOMove(_initialPosition, 1).AsyncWaitForCompletion());
            if (transform.rotation != _initialRotation)
                tasks.Add(transform.DORotate(new Vector3(180, 0, 0), 1, RotateMode.LocalAxisAdd).AsyncWaitForCompletion());
            await Task.WhenAll(tasks);
        }
    }
}