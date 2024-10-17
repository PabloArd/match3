using UnityEngine;

namespace Match3.Services.Inputs
{
    public interface IInputReceiver
    {
        void Tap(Vector2 worldPosition);
        void Drag(Vector2 worldPosition);
    }
}
