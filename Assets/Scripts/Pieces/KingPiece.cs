using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

// specific implementation for the king piece    
public class KingPiece : CommanderPiece {
    // the piece type for dictionary lookup
    public override PieceType type { get; } = PieceType.King;
    public override int material_value { get; } = 20;
    // the king can move in any direction as long as the path is 3 tiles or shorter,
    // and it can only attack adjacent pieces
    public override void Explore(ref Definitions.ActionDatabase results) {
        Exploring.NPathExplore.Explore(this, 3, ref results);
    }

    // define the soldiers this king will command
    public override void commander_init(
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

        // the soldiers a king will command
        spawnList_ = new List<(GameObject, Definitions.BoardPosition)>()
        {
            (prefabs_.Rook,     new Definitions.BoardPosition(1, is_white ? 1 : 8)),
            (prefabs_.Queen,    new Definitions.BoardPosition(4, is_white ? 1 : 8)),
            (prefabs_.Pawn,     new Definitions.BoardPosition(4, is_white ? 2 : 7)),
            (prefabs_.Pawn,     new Definitions.BoardPosition(5, is_white ? 2 : 7)),
            (prefabs_.Rook,     new Definitions.BoardPosition(8, is_white ? 1 : 8))
        };

        // spawn the soldiers
        this.spawn_units(controller); 
    }

}

} // Piece
} // Chess
