using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
namespace Exploring
{

// represents the ability for a piece at a given position to explore in ranged fashion, meaning
// the piece can attack pieces up to gap + 1 spaces away (since the rules codify ranged distances as gaps 
// inbetween pieces). the piece can only move to adjacent squares. If the piece it is attacking is adjacent, it has 
// the option to move onto that space given the attack is successful
// is used by the Rook
public class RangedExplore {
    // explore function called by the pieces, it can be assumed that the actions
    // appended to results are all valid AND that every possible action is 
    // appended to results
    public static void Explore(
        GameObject p, 
        int gap,
        ref HashSet<Definitions.Action> results
    ) {
    }
}

} // Explore
} // Pieces
} // Chess  