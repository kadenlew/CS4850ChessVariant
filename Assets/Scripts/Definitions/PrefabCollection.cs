using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{
<<<<<<< Updated upstream

public readonly struct PrefabCollection 
=======
        [System.Serializable]
        public readonly struct PrefabCollection 
>>>>>>> Stashed changes
{
    public GameObject King { get; } 
    public GameObject Queen { get; }
    public GameObject Bishop { get; }
    public GameObject Knight{ get; }
    public GameObject Rook{ get; }
    public GameObject Pawn { get; }

    public PrefabCollection(
        GameObject KingPrefab,
        GameObject QueenPrefab,
        GameObject BishopPrefab,
        GameObject KnightPrefab,
        GameObject RookPrefab,
        GameObject PawnPrefab
    )
    {
        King = KingPrefab;
        Queen = QueenPrefab;
        Bishop = BishopPrefab;
        Knight = KnightPrefab;
        Rook = RookPrefab;
        Pawn = PawnPrefab;
    }

}

}
}