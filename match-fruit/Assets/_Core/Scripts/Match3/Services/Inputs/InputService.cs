using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Match3.Services.Inputs
{
    public class InputService : IInputService, IInitializable, ITickable
    {
        private const float DRAG_THRESHOLD = 50f;
        private Vector2 _tapStartPosition;
        private bool _isDragging;
        private List<IInputReceiver> _inputReceivers = new();
        private Camera _camera;

        public void Initialize()
        {
            _camera = Camera.main;
        }

        public void Tick()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_OSX
            UpdateEditor();
#else
                UpdateMobile();
#endif
        }

        public void RegisterReceiver(IInputReceiver inputReceiver)
        {
            if (!_inputReceivers.Contains(inputReceiver))
                _inputReceivers.Add(inputReceiver);
        }

        public void UnregisterReceiver(IInputReceiver inputReceiver)
        {
            if (_inputReceivers.Contains(inputReceiver))
                _inputReceivers.Remove(inputReceiver);
        }

        private void UpdateEditor()
        {
            if (Input.GetMouseButtonDown(0))
                Tap(Input.mousePosition);

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                _isDragging = IsDragging(mousePosition);
                if (_isDragging)
                    Drag(mousePosition);
            }
        }

        private void UpdateMobile()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = touch.position;

                if (touch.phase == TouchPhase.Began)
                {
                    Tap(touchPosition);
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    _isDragging = IsDragging(touchPosition);
                    if (_isDragging)
                        Drag(touchPosition);
                }
            }
        }

        public void Tap(Vector2 screenPosition)
        {
            _tapStartPosition = screenPosition;
            foreach (var receiver in _inputReceivers)
            {
                receiver.Tap(_camera.ScreenToWorldPoint(screenPosition));
            }
        }

        public void Drag(Vector2 screenPosition)
        {
            foreach (var receiver in _inputReceivers)
            {
                receiver.Drag(_camera.ScreenToWorldPoint(screenPosition));
            }
        }

        private bool IsDragging(Vector2 screenPosition)
        {
            return Vector2.Distance(_tapStartPosition, screenPosition) > DRAG_THRESHOLD;
        }
    }
}
