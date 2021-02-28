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

    // cache the results object to prevent copying during the recursion 
    public static HashSet<Definitions.Action> results { get; protected set; } = new HashSet<Definitions.Action>();

    // cache the piece in question to prevent copying during recusion
    public static GameObject piece_ref { get; protected set; }
    
    // explore function that guarentees that all actions added to results are valid, and that
    // every possible action is added to results
    public static void Explore(
        GameObject piece, 
        int n, 
        ref HashSet<Definitions.Action> results,
        bool move_and_attack = false
    ) {
        // cache the relevant information
        NPathExplore.results = results;
        piece_ref = piece;   
        
        // start DFS where this piece currently is
        explore_adjacent(
            piece.GetComponent<GamePieceBase>().position,
            n,
            0,
            move_and_attack
        );
    }

    // implementation of DFS in use for exploring any path of length current_length
    // will stop searching a branch if various conditions are met
    public static void explore_adjacent(   
        Definitions.BoardPosition pos,
        int max_length,
        int current_length,
        bool move_and_attack
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
            GameObject res;
            if(piece_ref.GetComponent<GamePieceBase>().controller_ref.checkPosition(pos, out res)) {
                // it is, is that piece an opponent's piece?
                if ( 
                    piece_ref.GetComponent<GamePieceBase>().is_white != res.GetComponent<GamePieceBase>().is_white
                ) {
                    // it is, can we attack given our current path length?
                    if(current_length == 1 || move_and_attack) {
                        // yes, we can attack
                        results.Add(
                            new Definitions.AttackAction(
                                piece_ref,
                                res,
                                (move_and_attack && current_length > 1) ? -1 : 0
                        ));
                    }

                }
                // stop searching after we've hit any piece (since you can't jump over pieces)
                return;
            } 
            else {
                // the space is empty, we can move there
                results.Add(
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
                max_length,
                current_length + 1, // increment the path length
                move_and_attack
            );
        }
    }
}

} // Explore
} // Pieces
} // Chess  