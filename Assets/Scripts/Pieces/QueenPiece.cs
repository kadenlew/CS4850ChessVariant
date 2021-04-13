using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class QueenPiece : SoldierPiece {
    // piece type for dictionary lookup
    public override PieceType type { get; } = PieceType.Queen;
    // the queen can move in any direction as long as the path is 3 tiles or shorter,
    // and it can only attack adjacent pieces
    public override void Explore(ref Definitions.ActionDatabase results) {
        Exploring.NPathExplore.Explore(this, 3, ref results);
    }

}

} // Piece
} // Chess
