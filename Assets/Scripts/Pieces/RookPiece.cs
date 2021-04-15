using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
    
public class RookPiece : SoldierPiece {
    // piece type for dictionary lookup
    public override PieceType type { get; } = PieceType.Rook;
    public override int material_value { get; } = 10;

    // the rook can attack in a ranged style, at pieces who are up to 3 tiles away (codifed as a gap of 2 in the rules)
    // if the rook is adjacent to what is attacking, it can move to that square. Otherwise, it will not move after it attacks.
    // it can move to any adjacent square as well
    public override void Explore(ref Definitions.ActionDatabase results) {
        Exploring.RangedExplore.Explore(this, 2, ref results);
    }

}

} // Piece
} // Chess
