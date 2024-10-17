using System.Collections;
using UnityEngine;

namespace Match3.Utils.MonoAsync
{
    public interface IMonoAsyncProcessor
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
    }
}
