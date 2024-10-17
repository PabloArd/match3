using System.Collections.Generic;
using UnityEngine;

namespace Match3.Services.Match
{
    public interface IMatchService
    {
        List<Vector2Int> CheckMatches();
    }
}