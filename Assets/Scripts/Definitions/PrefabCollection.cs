using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

[System.Serializable]
public class PrefabCollection 
{
    public GameObject King;  
    public GameObject Queen; 
    public GameObject Bishop;
    public GameObject Knight; 
    public GameObject Rook;
    public GameObject Pawn;
    public Material[]  pieceColors;

    public Material[] pieceColorsSelected;

    // public PrefabCollection(
    //     GameObject KingPrefab,
    //     GameObject QueenPrefab,
    //     GameObject BishopPrefab,
    //     GameObject KnightPrefab,
    //     GameObject RookPrefab,
    //     GameObject PawnPrefab,
    //     Material[] pieceMaterials
    // )
    // {
    //     King = KingPrefab;
    //     Queen = QueenPrefab;
    //     Bishop = BishopPrefab;
    //     Knight = KnightPrefab;
    //     Rook = RookPrefab;
    //     Pawn = PawnPrefab;
    // }

}

}
}