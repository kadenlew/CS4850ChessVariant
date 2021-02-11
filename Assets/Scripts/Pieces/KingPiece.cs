using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{
    
public class KingPiece : CommanderPiece
{
<<<<<<< Updated upstream
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
=======
    
>>>>>>> Stashed changes

    public override void commander_init(
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

        // initialize this specific corp of pieces
    }
}

}
}
