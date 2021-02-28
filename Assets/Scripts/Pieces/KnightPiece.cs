using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class KnightPiece : SoldierPiece {
    public override void Explore(ref HashSet<Definitions.Action> results) {
        Exploring.NPathExplore.Explore(this.gameObject, 5, ref results, true);
    }

    public override PieceType type { get; } = PieceType.Knight;
}

} // Piece
} // Chess
