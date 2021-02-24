using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Piece
{

public abstract class CommanderPiece : GamePieceBase {
    // Start is called before the first frame update
    protected Definitions.PrefabCollection prefabs_;
    protected List<GameObject> soldiers_;

    protected List<(GameObject, Definitions.BoardPosition)> spawnList_;

    public virtual List<GameObject> commander_init(
        bool is_white, 
        Definitions.BoardPosition starting_position,
        Definitions.PrefabCollection prefabs,
        ref BoardController controller
    ) {
        // save the generic information that all commanders will require
        this.prefabs_ = prefabs;

        // do the standard piece init as well
        this.init(is_white, starting_position, ref controller);

        return soldiers_;
    }

    protected void spawn_units(ref BoardController controller) {
        // reset the soldier list and reserve enough space for our units
        soldiers_ = new List<GameObject>(spawnList_.Count);

        // for each unit thats been defined, spawn it in and call its init function 
        // with the board position specified
        foreach((GameObject piece, Definitions.BoardPosition pos) in spawnList_)
        {
            soldiers_.Add(
                GameObject.Instantiate(piece)
            );

            soldiers_[soldiers_.Count - 1].GetComponent<Piece.GamePieceBase>().init(
                is_white,
                pos,
                ref controller
            );
        }
    }
    public override abstract List<Definitions.Action> Explore();
}


} // Piece
} // Chess
