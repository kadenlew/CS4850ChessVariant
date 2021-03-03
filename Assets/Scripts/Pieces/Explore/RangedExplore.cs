using System.Collections;
using System.Collections.Generic;
using System;
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
        RangedExplore.results = results;
        piece_ref = p;

        explore_radius(
            p.GetComponent<GamePieceBase>().position,
            gap
        );
    }

    public static double special_mag(
        Definitions.BoardPosition origin,
        Definitions.BoardPosition target
    ){
        double delta_x = Math.Abs(origin.rank - target.rank);
        double delta_y = Math.Abs(origin.file - target.file);
        return Math.Min(delta_x, delta_y) * Math.Sqrt(2) + Math.Abs(delta_x - delta_y);
    }

    public static void explore_radius(
        Definitions.BoardPosition pos,
        int gap
    ){
        for(int x = -(gap + 1); x <= (gap + 1); x++){
            for(int y = -(gap + 1);  y <= (gap + 1); y++){
                var move = new Definitions.BoardVector(x, y);
                double mag = special_mag(pos, (pos + move));
                
                GameObject res;

                if(mag > gap + 1){
                    continue;
                }

                if(mag == 1){
                    if(piece_ref.GetComponent<GamePieceBase>().controller_ref.checkPosition(pos, out res)){
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
                    if(piece_ref.GetComponent<GamePieceBase>().controller_ref.checkPosition(pos, out res)){
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