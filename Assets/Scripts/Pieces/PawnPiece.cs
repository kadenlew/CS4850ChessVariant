using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class PawnPiece : SoldierPiece {
    public override void Explore(ref HashSet<Definitions.Action> results) {
        Exploring.ForwardExplore.Explore(this.gameObject, ref results);
    }

    public override PieceType type { get; } = PieceType.Pawn;
}

} // Piece
} // Chess
