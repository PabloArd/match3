using System.Collections;
using UnityEngine;

namespace Match3.Utils.MonoAsync
{
    public class MonoAsyncProcessor : MonoBehaviour, IMonoAsyncProcessor
    {
        private bool m_isActive;

        public new void StopCoroutine(Coroutine coroutine)
        {
            if (m_isActive && gameObject.activeSelf && coroutine != null)
                base.StopCoroutine(coroutine);
        }

        public new Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return m_isActive ? base.StartCoroutine(coroutine) : null;
        }

        private void Awake()
        {
            m_isActive = true;
        }

        private void OnDestroy()
        {
            m_isActive = false;
        }
    }
}
