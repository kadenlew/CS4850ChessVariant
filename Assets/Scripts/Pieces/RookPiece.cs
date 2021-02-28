using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
    
public class RookPiece : SoldierPiece {
    public override void Explore(ref HashSet<Definitions.Action> results) {
        Exploring.RangedExplore.Explore(this.gameObject, 2, ref results);
    }

    public override PieceType type { get; } = PieceType.Rook;
}

} // Piece
} // Chess
