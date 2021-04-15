using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class PawnPiece : SoldierPiece {
    // piece type for dictionary lookup
    public override PieceType type { get; } = PieceType.Pawn;
    public override int material_value { get; } = 1;
    
    // pawns can only move forward, and can only attack in front of it or diagonally forward 
    public override void Explore(ref Definitions.ActionDatabase results) {
        Exploring.ForwardExplore.Explore(this, ref results);
    }

}

} // Piece
} // Chess
