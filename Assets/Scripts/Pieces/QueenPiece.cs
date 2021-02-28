using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class QueenPiece : SoldierPiece {
    public override List<Definitions.Action> Explore() {
        return Exploring.NPathExplore.Explore(this.gameObject, 3);
    }

    public override PieceType type { get; } = PieceType.Queen;
}

} // Piece
} // Chess