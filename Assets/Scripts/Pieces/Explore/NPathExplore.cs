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
    
    public static HashSet<Definitions.Action> results { get; protected set; } = new HashSet<Definitions.Action>();

    public static GameObject piece_ref { get; protected set; }
    
    public static List<Definitions.Action> Explore(
        GameObject piece, 
        int n, 
        bool move_and_attack = false
    ) {
        results.Clear();
        piece_ref = piece;   
        
        explore_adjacent(
            piece.GetComponent<GamePieceBase>().position,
            n,
            0,
            move_and_attack
        );
            
        return new List<Definitions.Action>();
    }

    public static void explore_adjacent(   
        Definitions.BoardPosition pos,
        int max_length,
        int current_length,
        bool move_and_attack
    ) {
        if (current_length > max_length) 
            return;
            
        if (current_length > 0) {
            if (!pos.is_valid) {
                return;
            }

            GameObject res;
            if(piece_ref.GetComponent<GamePieceBase>().controller_ref.checkPositionFull(pos, out res)) {
                if ( 
                    piece_ref.GetComponent<GamePieceBase>().is_white != res.GetComponent<GamePieceBase>().is_white
                ) {
                    if(current_length == 1 || move_and_attack) {
                        results.Add(
                            new Definitions.AttackAction(
                                piece_ref,
                                res,
                                (move_and_attack && current_length > 1) ? -1 : 0
                            )
                        );
                    }
                }

                return;
            } 
            else {
                results.Add(
                    new Definitions.MoveAction(
                        piece_ref,
                        pos
                ));
            }       
        } 
        
        foreach(var move in adjacent) {
            explore_adjacent(
                pos + move,
                max_length,
                current_length + 1,
                move_and_attack
            );
        }
    }
}

} // Explore
} // Pieces
} // Chess  