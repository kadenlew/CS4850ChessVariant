using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
namespace Exploring
{

public class ForwardExplore {
    public static List<Definitions.Action> Explore(
        GameObject piece, 
        int distance = 1
    ) {
        Debug.Log($"I can only move forward {distance} squares! :(");
        return new List<Definitions.Action>();
    }
}

} // Explore
} // Pieces
} // Chess  