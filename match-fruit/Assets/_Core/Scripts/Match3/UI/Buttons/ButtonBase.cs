using UnityEngine;
using UnityEngine.UI;

namespace Match3.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class ButtonBase : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        protected virtual void OnButtonClick() { }
    }
}


