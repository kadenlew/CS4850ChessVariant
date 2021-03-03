using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess {
namespace Definitions {
// represents a single tile on the chess board
// used to associate a BoardPosition with a specific GameObject
public class Tile : MonoBehaviour
{
    // the boardPosition this tile is representing
    public Definitions.BoardPosition position { get; protected set; }
    
    // reference to the unity prefabs used for unified spawning
    protected Definitions.PrefabCollection prefabs_;

    // called when this tile is created to assign it it's position
    // and get its color set correctly
    public void init(
        Definitions.BoardPosition position,
        Definitions.PrefabCollection prefabs
    )
    {
        this.position = position;
        prefabs_ = prefabs;

        this.GetComponent<Renderer>().material = (
            prefabs_.tileMaterials[
                ((position.file + position.rank) % 2 == 0) 
                ? 1 : 0
            ]
        );
    }
}

} // Definitions
} // Chess
