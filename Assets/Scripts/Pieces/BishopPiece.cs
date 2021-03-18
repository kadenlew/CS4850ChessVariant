using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
    
public class BishopPiece : CommanderPiece {
    // Used in dictionary lookups
    public override PieceType type { get; } = PieceType.Bishop;

    // Extend the base commander initing to indicate what soldiers 
    // this piece has under it, and where they spawn
    public override List<GameObject> commander_init(
        bool is_white, 
        Definitions.BoardPosition starting_position,
        Definitions.PrefabCollection prefabs,
        BoardController controller,
        Control.PlayerBase owner
    ) {
        // generic commander initialization details
        base.commander_init(
            is_white,
            starting_position,
            prefabs,
            controller,
            owner
        );

        // determine which commander bishop this is
        bool is_left = (this.position.file == 3) ? true : false;

        // create the list of soldiers that this commander will command
        spawnList_ = new List<(GameObject, Definitions.BoardPosition)>()
        {
            (prefabs_.Pawn,     new Definitions.BoardPosition(is_left ? 1 : 8, is_white ? 2 : 7)),
            (prefabs_.Knight,     new Definitions.BoardPosition(is_left ? 2 : 7, is_white ? 2 : 7)),
            (prefabs_.Pawn,     new Definitions.BoardPosition(is_left ? 3 : 6, is_white ? 2 : 7)),
            (prefabs_.Pawn,   new Definitions.BoardPosition(is_left ? 2 : 7, is_white ? 1 : 8))
        };

        // spawn those soldiers
        this.spawn_units(controller); 

        // return for the lookup tables
        return soldiers_;
    }

    // Bishop specific exploring 
    public override void Explore(ref HashSet<Definitions.Action> results) {
        Exploring.ForwardExplore.Explore(this.gameObject, ref results);
    }

}

}
}
