using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
    
public class BishopPiece : CommanderPiece
{
    // Start is called before the first frame update

    public override List<GameObject> commander_init(
        bool is_white, 
        Definitions.BoardPosition starting_position,
        Definitions.PrefabCollection prefabs
    ) {
        // generic commander initialization details
        base.commander_init(
            is_white,
            starting_position,
            prefabs
        );

        // determine which commander bishop this is
        bool is_left = (this.position_.get_file() == 3) ? true : false;

        spawnList_ = new List<(GameObject, Definitions.BoardPosition)>()
        {
            (prefabs_.Pawn,     new Definitions.BoardPosition(is_left ? 1 : 8, is_white_ ? 2 : 7)),
            (prefabs_.Pawn,     new Definitions.BoardPosition(is_left ? 2 : 7, is_white_ ? 2 : 7)),
            (prefabs_.Pawn,     new Definitions.BoardPosition(is_left ? 3 : 6, is_white_ ? 2 : 7)),
            (prefabs_.Knight,   new Definitions.BoardPosition(is_left ? 2 : 7, is_white_ ? 1 : 8))
        };

        this.spawn_units(); 
        return soldiers_;
    }

    public override List<Definitions.Action> Explore()
    {
        return new List<Definitions.Action>();
    }
}

}
}
