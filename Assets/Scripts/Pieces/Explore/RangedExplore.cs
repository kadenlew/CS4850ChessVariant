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

        explore_radius(
            p.GetComponent<GamePieceBase>().position
        );
    }

    public static void explore_radius(
        Definitions.BoardPosition pos,
        int current_gap
    ){
        if(current_gap > 0){
            if (!pos.is_valid){
            return;
            }
        }

        GameObject res;
        if(piece_ref.GetComponent<GamePieceBase>().controller_ref.checkPosition(pos, out res)){
            if(piece_ref.GetComponent<GamePieceBase>().is_white != res.GetComponent<GamePieceBase>().is_white){
                results.Add(
                    new Definitions.AttackAction(
                        piece_ref,
                        res,
                        current_gap
                    ));
            }
        }
        return;

        else{
            results.Add(
                new Definitions.MoveAction(
                    piece_ref,
                    pos
                ));
        }
    }

    for(int i = 0; i <= 3; i++){
        for(int j = 0; j <= 3; j++){
            explore_radius(
                pos + new Definitions.BoardVector(i, j),
                current_gap + 1
            );
        }
    }
}

} // Explore
} // Pieces
} // Chess  