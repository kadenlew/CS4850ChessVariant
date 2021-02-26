using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class PawnPiece : SoldierPiece {
    public override List<Definitions.Action> Explore() {
        return Exploring.ForwardExplore.Explore(this);
    }

    public override PieceType type { get; } = PieceType.Pawn;
}

} // Piece
} // Chess
