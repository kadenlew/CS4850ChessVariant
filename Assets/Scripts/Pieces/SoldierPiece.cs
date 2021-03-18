using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Piece
{

// filler function for all standard solider pieces,
// incase they require any specific funcitonality that a commander 
// does not require
public abstract class SoldierPiece : GamePieceBase {
    // forwarding the abstract explore
    public GameObject commander { get; set; }

    public void soldier_init(
        bool is_white, 
        Definitions.BoardPosition starting_position, 
        GameObject commander,
        BoardController controller, 
        Definitions.PrefabCollection prefabs
    ) {
        base.init(
            is_white,
            starting_position,
            controller,
            prefabs
        );

        this.commander = commander;
    }

    public override void set_inactive()
    {
        base.set_inactive();
        
    }

    public override bool expend_energy(uint cost)
    {
        return this.commander.GetComponent<CommanderPiece>().expend_energy(cost);
    }

    public override void kill()
    {
        commander.GetComponent<CommanderPiece>().remove_soldier(this.gameObject);
    }

    public override abstract void Explore(ref HashSet<Definitions.Action> results);
}

} // Piece
} // Chess
