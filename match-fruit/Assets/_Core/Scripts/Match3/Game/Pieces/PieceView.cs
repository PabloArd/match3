using DG.Tweening;
using System;
using UnityEngine;

namespace Match3.Game.Pieces
{
    public class PieceView : MonoBehaviour, IPieceView
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Ease m_easeMove;
        [SerializeField] private Ease m_easeFall;
        public PieceType Type { get; private set; }

        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }

        private void Start()
        {
            _spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        public void Initialize(PieceType type, Sprite sprite)
        {
            Type = type;
            _spriteRenderer.sprite = sprite;
        }

        public void Select(bool select)
        {
            if (select)
                transform.DOScale(1.2f, 0.2f);
            else
                transform.DOScale(1f, 0.2f);
        }

        public void MoveTo(Vector2 position, float time, bool fall, Action callback)
        {
            Ease ease = fall ? m_easeFall : m_easeMove;
            transform.DOMove(position, time).SetEase(ease)
                .OnComplete(()=>
                {
                    callback?.Invoke();
                });
        }
    }
}