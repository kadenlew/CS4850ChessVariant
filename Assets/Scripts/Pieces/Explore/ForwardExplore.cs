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
        Piece.GamePieceBase piece,
        ref Definitions.ActionDatabase results, 
        int distance = 1
    ) {
        // try each potential change in position
        foreach(var move in moves)
        {
            // get the new position and check if its valid
            var new_position = piece.is_white ? 
                    piece.position + move :
                    piece.position - move;

            if(!new_position.is_valid)
                continue;
            
            // see if the space is occupied
            Piece.GamePieceBase res;
            if(piece.controller_ref.checkPosition(
                new_position,
                out res
            )) {
                // the space is occupied, is it by us or the enemy 
                if(
                    res.is_white != piece.is_white
                ) {
                    // its occupied by the enemy, so we can attack it
                    results.add_action(
                       new Definitions.AttackAction(
                           piece,
                           res
                    )); 
                }
                else {
                    results.add_hypothetical(
                        new Definitions.MoveAction(
                            piece,
                            res.position
                        )
                    );
                }
            }
            else {
                // the space is empty, we can move there
                results.add_action(
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