using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class KnightPiece : SoldierPiece {
    public override List<Definitions.Action> Explore() {
        return Exploring.NPathExplore.Explore(this.gameObject, 5, true);
    }

    public override PieceType type { get; } = PieceType.Knight;
}

} // Piece
} // Chess
