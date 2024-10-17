using Match3.Game.Pieces;
using System;
using UnityEngine;

namespace Match3.ScriptableObjects
{
    [Serializable]
    public class PieceData
    {
        [field: SerializeField] public PieceType Type { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public GameObject MatchVfx { get; private set; }
        [field: SerializeField] public int ScoreBase { get; private set; }
    }

}
