using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
    
public class RookPiece : SoldierPiece {
    public override List<Definitions.Action> Explore() {
        return Exploring.RangedExplore.Explore(this.gameObject, 2);
    }

    public override PieceType type { get; } = PieceType.Rook;
}

} // Piece
} // Chess
