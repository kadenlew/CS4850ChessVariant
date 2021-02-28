using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess {
public class Tile : MonoBehaviour
{
    public Definitions.BoardPosition position { get; protected set; }
    protected Definitions.PrefabCollection prefabs_;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

} // Chess
