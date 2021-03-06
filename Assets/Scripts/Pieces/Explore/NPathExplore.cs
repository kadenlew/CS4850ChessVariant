using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
namespace Exploring
{

// represents the ability for a piece to go to any space in any direction as long
// as its path length is no greater than N. Can also allow a piece to move and attack
// with a roll_modifer of -1. Used by the King, Queen, and Knight
//
// utilizes DFS to explore the space
public class NPathExplore {
    // Movement to all of the adjacent squares
    public static List<Definitions.BoardVector> adjacent { get; } = new List<Definitions.BoardVector>()
    {
        // horizontal and vertical
        new Definitions.BoardVector(0,1), 
        new Definitions.BoardVector(0,-1),
        new Definitions.BoardVector(1,0),
        new Definitions.BoardVector(-1,0),

        // diagonals
        new Definitions.BoardVector(1,1),
        new Definitions.BoardVector(1,-1),
        new Definitions.BoardVector(-1,1),
        new Definitions.BoardVector(-1,-1)

    };

    // explore function that guarentees that all actions added to results are valid, and that
    // every possible action is added to results
    public static void Explore(
        Piece.GamePieceBase piece, 
        int n, 
        ref Definitions.ActionDatabase results,
        bool move_and_attack = false
    ) {
        // start DFS where this piece currently is
        explore_adjacent(
            piece.position,
            null,
            n,
            0,
            move_and_attack,
            ref piece,
            ref results
        );
    }

    // implementation of DFS in use for exploring any path of length current_length
    // will stop searching a branch if various conditions are met
    public static void explore_adjacent(   
        Definitions.BoardPosition pos,
        Definitions.BoardPosition prev,
        int max_length,
        int current_length,
        bool move_and_attack,
        ref Piece.GamePieceBase piece_ref,
        ref Definitions.ActionDatabase results
    ) {
        // only search paths that are of max_length
        if (current_length > max_length) {
            return;
        }

        // skips the logic for the entry position
        if (current_length > 0) {
            // if the position is not valid, stop searching this branch
            if (!pos.is_valid) {
                return;
            }

            // is this position occupied?
            Piece.GamePieceBase res;
            if(piece_ref.controller_ref.checkPosition(pos, out res)) {
                // it is, is that piece an opponent's piece?
                if ( 
                    piece_ref.is_white != res.is_white
                ) {
                    // it is, can we attack given our current path length?
                    if(current_length == 1) {
                        // yes, we can attack
                        results.add_action(
                            new Definitions.AttackAction(
                                piece_ref,
                                res,
                                0
                        ));
                    }
                    else if (move_and_attack) {
                        results.add_action(
                            new Definitions.AttackMoveAction(
                                piece_ref,
                                res,
                                prev
                            )
                        );
                    }

                }
                else {
                    results.add_hypothetical(
                        new Definitions.MoveAction(
                            piece_ref,
                            res.position
                        )
                    );
                }
                // stop searching after we've hit any piece (since you can't jump over pieces)
                return;
            } 
            else {
                // the space is empty, we can move there
                results.add_action(
                    new Definitions.MoveAction(
                        piece_ref,
                        pos
                ));
                // continue the search since we can move in this direction
            }       
        } 

        // try movement to every possible adjacent square 
        foreach(var move in adjacent) {
            explore_adjacent(
                pos + move,
                pos,
                max_length,
                current_length + 1, // increment the path length
                move_and_attack,
                ref piece_ref,
                ref results
            );
        }
    }
}

} // Explore
} // Pieces
} // Chess  