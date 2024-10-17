using UnityEngine;

namespace Match3.Services.Inputs
{
    public interface IInputService
    {
        void RegisterReceiver(IInputReceiver inputReceiver);
        void UnregisterReceiver(IInputReceiver inputReceiver);
        void Tap(Vector2 screenPosition);
        void Drag(Vector2 screenPosition);
    }
}
