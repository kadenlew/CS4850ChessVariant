using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class KnightPiece : SoldierPiece {
    // the piece type for dictionary lookup
    public override PieceType type { get; } = PieceType.Knight;
    
    // knights can move in any direction with a path length of 5 or less. They can also
    // move and attack any pice up to a path length of 5 away, however the its roll with
    // be given a -1 penalty. It can still attack adjacent spaces as normal
    public override void Explore(ref Definitions.ActionDatabase results) {
        Exploring.NPathExplore.Explore(this, 5, ref results, true);
    }

}

} // Piece
} // Chess
