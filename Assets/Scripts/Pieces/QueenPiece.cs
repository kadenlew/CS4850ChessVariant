using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class QueenPiece : SoldierPiece {
    public override void Explore(ref HashSet<Definitions.Action> results) {
        Exploring.NPathExplore.Explore(this.gameObject, 3, ref results);
    }

    public override PieceType type { get; } = PieceType.Queen;
}

} // Piece
} // Chess