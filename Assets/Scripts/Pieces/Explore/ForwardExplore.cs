using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
namespace Exploring
{

public class ForwardExplore {
    public static List<Definitions.BoardVector> moves { get; } = new List<Definitions.BoardVector>()
    {
        // horizontal and vertical
        new Definitions.BoardVector(-1, 1),
        new Definitions.BoardVector(0, 1),
        new Definitions.BoardVector(1, 1)
    };

    public static void Explore(
        GameObject piece,
        ref HashSet<Definitions.Action> results, 
        int distance = 1
    ) {
        foreach(var move in moves)
        {
            var new_position = piece.GetComponent<GamePieceBase>().position + move;
            if(!new_position.is_valid)
                continue;
            
            GameObject res;
            if(piece.GetComponent<GamePieceBase>().controller_ref.checkPosition(
                new_position,
                out res
            )) {
                if(
                    res.GetComponent<GamePieceBase>().is_white != piece.GetComponent<GamePieceBase>().is_white
                ) {
                   results.Add(
                       new Definitions.AttackAction(
                           piece,
                           res
                    )); 
                }
            }
            else {
                results.Add(
                    new Definitions.MoveAction(
                        piece,
                        new_position
                ));
            }
        }

        // foreach(var action in results)
        // {
        //     Debug.Log($"{action}");
        // }
        // Debug.Log($"{piece.GetComponent<GamePieceBase>()}: {results.Count} possible moves.");
    }
}

} // Explore
} // Pieces
} // Chess  