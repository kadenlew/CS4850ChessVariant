using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
namespace Exploring
{

// represents the ability for a piece at a given position to explore any forward (or diagonal space)
// is used by the Bishop and Pawn
public class ForwardExplore {
    // the possible position changes that are either forward or diagonal
    public static List<Definitions.BoardVector> moves { get; } = new List<Definitions.BoardVector>()
    {
        // horizontal and vertical
        new Definitions.BoardVector(-1, 1),
        new Definitions.BoardVector(0, 1),
        new Definitions.BoardVector(1, 1)
    };

    // the function called by a pieces' explore, ensured that all actions appended to results
    // are valid and that all possible actions are appended to results
    public static void Explore(
        GameObject piece,
        ref HashSet<Definitions.Action> results, 
        int distance = 1
    ) {
        // try each potential change in position
        foreach(var move in moves)
        {
            // get the new position and check if its valid
            var new_position = piece.GetComponent<GamePieceBase>().position + move;
            if(!new_position.is_valid)
                continue;
            
            // see if the space is occupied
            GameObject res;
            if(piece.GetComponent<GamePieceBase>().controller_ref.checkPosition(
                new_position,
                out res
            )) {
                // the space is occupied, is it by us or the enemy 
                if(
                    res.GetComponent<GamePieceBase>().is_white != piece.GetComponent<GamePieceBase>().is_white
                ) {
                    // its occupied by the enemy, so we can attack it
                    results.Add(
                       new Definitions.AttackAction(
                           piece,
                           res
                    )); 
                }
            }
            else {
                // the space is empty, we can move there
                results.Add(
                    new Definitions.MoveAction(
                        piece,
                        new_position
                ));
            }
        }
    }
}

} // Explore
} // Pieces
} // Chess  