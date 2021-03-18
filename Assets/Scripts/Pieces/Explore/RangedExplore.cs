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
    
    public static HashSet<Definitions.Action> results { get; protected set; } = new HashSet<Definitions.Action>();
    
    public static GameObject piece_ref { get; protected set; }

    // explore function called by the pieces, it can be assumed that the actions
    // appended to results are all valid AND that every possible action is 
    // appended to results
    public static void Explore(
        GameObject p,
        int gap,
        ref HashSet<Definitions.Action> results
    ){
        RangedExplore.results = results;
        piece_ref = p;

        explore_radius(
            p.GetComponent<GamePieceBase>().position,
            gap
        );
    }

    public static void explore_radius(
        Definitions.BoardPosition pos,
        int gap
    ){
        for(int x = -(gap + 1); x <= (gap + 1); x++){
            for(int y = -(gap + 1);  y <= (gap + 1); y++){
                var move = new Definitions.BoardVector(x, y);
                double mag = move.special_mag;

                if(mag > gap + 1){
                    continue;
                }

                var new_pos = pos + move;                
                if(!new_pos.is_valid)
                    continue;


                GameObject res;
                if(mag <= 1){
                    if(piece_ref.GetComponent<GamePieceBase>().controller_ref.checkPosition(new_pos, out res)){
                        if(piece_ref.GetComponent<GamePieceBase>().is_white != res.GetComponent<GamePieceBase>().is_white){
                            results.Add(
                                new Definitions.AttackAction(
                                piece_ref,
                                res
                            ));
                        } 
                    }
                    else{
                        results.Add(
                            new Definitions.MoveAction(
                            piece_ref,
                            new_pos
                        ));
                    }
                }
                else if(mag > 1){
                    if(piece_ref.GetComponent<GamePieceBase>().controller_ref.checkPosition(new_pos, out res)){
                        if(piece_ref.GetComponent<GamePieceBase>().is_white != res.GetComponent<GamePieceBase>().is_white){
                            results.Add(
                                new Definitions.AttackAction(
                                piece_ref,
                                res
                            ));
                        } 
                    }
                }
            }
        }
    }
}

} // Explore
} // Pieces
} // Chess  