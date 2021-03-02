using System.Collections;
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
        int gap,
        ref HashSet<Definitions.Action> results
    ){
        RangedExplore.results = results;
        piece_ref = p;
        int gap = 3;

        explore_radius(
            p.GetComponent<GamePieceBase>().position,
            gap
        );
    }

    public static int special_mag(
        Definitions.BoardPosition origin,
        Definitions.BoardPosition target
    ){
        delta_x = abs(origin.rank - target.rank);
        delta_y = abs(origin.file - target.file);
        return min(delta_x, delta_y) * sqrt(2) + abs(delta_x - delta_y);
    }

    public static void explore_radius(
        Definitions.BoardPosition pos,
        int gap
    ){
        for(int x = -gap; x <= 3; x++){
            for(int y = -gap;  y <= 3; y++){
                var move = new BoardVector(x, y);
                int mag = special_mag(pos, (pos + move));
                
                if(mag > y){
                    continue;
                }

                if(mag == 1){
                    GameObject res;
                    if(piece_ref.GetComponent<GamePieceBase>().controller_ref.checkPosition(pos, out res)){
                        if(piece_ref.GetComponent<GamePieceBase>().is_white != res.GetComponent<GamePieceBase>().is_white){
                            results.Add(
                                new Definitions.AttackAction(
                                piece_ref,
                                res,
                                mag
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
                                res,
                                mag
                            ));
                        }
                    }
                }
}

} // Explore
} // Pieces
} // Chess  