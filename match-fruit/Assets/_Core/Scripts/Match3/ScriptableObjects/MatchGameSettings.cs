using Match3.Game.Pieces;
using UnityEngine;

namespace Match3.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MatchGameSettings", menuName = "Match3/MatchGameSettings")]
    public class MatchGameSettings : ScriptableObject
    {
        [field: SerializeField] public PieceView PiecePrefab { get; private set; }
        [field: SerializeField] public PieceData[] Pieces { get; private set; }
        [field: SerializeField] public float TimeToMovePieceUnit { get; private set; }
        [field: SerializeField] public float TimeToFallPieceUnit { get; private set; }
        [field: SerializeField] public float GameTime { get; private set; }

        public PieceData GetRandomPiece()
        {
            return Pieces[Random.Range(0, Pieces.Length)];
        }
    }
}
