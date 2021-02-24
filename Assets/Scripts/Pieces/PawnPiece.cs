using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class PawnPiece : SoldierPiece
{
    public override List<Definitions.Action> Explore()
    {
        return new List<Definitions.Action>();
    }
}

}
}
