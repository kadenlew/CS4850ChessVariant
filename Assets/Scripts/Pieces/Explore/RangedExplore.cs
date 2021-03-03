using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
namespace Exploring
{

public class RangedExplore {
    
    public static HashSet<Definitions.Action> results { get; protected set; } = new HashSet<Definitions.Action>();
    
    public static GameObject piece_ref { get; protected set; }

    public static void Explore(
        GameObject p,
        int gap
    ){
        results.Clear();
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
                            pos
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