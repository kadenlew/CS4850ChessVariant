using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
namespace Exploring
{

public class NPathExplore {
    public static List<Definitions.Action> Explore(
        GamePieceBase piece, 
        int n, 
        bool move_and_attack = false
    ) {
        Debug.Log($"I can move in any direction with a path length of {n}! I also {(move_and_attack ? "can" : "can't")} move and attack!");

        return new List<Definitions.Action>();
    }
}

} // Explore
} // Pieces
} // Chess  