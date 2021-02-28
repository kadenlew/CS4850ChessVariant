using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
namespace Exploring
{

public class RangedExplore {
    public static List<Definitions.Action> Explore(
        GameObject p, 
        int gap
    ) {
        Debug.Log($"I Can Shoot you From Afar with a gap of {gap} between us!");
        return new List<Definitions.Action>();
    }
}

} // Explore
} // Pieces
} // Chess  