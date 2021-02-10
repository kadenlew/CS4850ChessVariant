using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

public class CommanderPiece : GamePieceBase
{
    // Start is called before the first frame update
    Definitions.PrefabCollection prefabs_;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void commander_init(
        bool is_white, 
        Definitions.BoardPosition starting_position,
        Definitions.PrefabCollection prefabs
    ) {
        // save the generic information that all commanders will require
        prefabs_ = prefabs;
        this.position_ = starting_position;
    }
}

}
}
