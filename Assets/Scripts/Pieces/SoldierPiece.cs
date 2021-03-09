using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Piece
{

public abstract class SoldierPiece : GamePieceBase {
    public GameObject commander { get; protected set; }
    public override abstract List<Definitions.Action> Explore();
}

} // Piece
} // Chess
