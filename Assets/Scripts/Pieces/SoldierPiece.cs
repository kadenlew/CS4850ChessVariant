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
    public Piece.CommanderPiece commander; 
    public Piece.CommanderPiece temp_commander; 

    public void soldier_init(
        bool is_white, 
        Definitions.BoardPosition starting_position, 
        Piece.CommanderPiece commander,
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

    public override bool expend_energy(uint cost)
    {
        if(temp_commander != null)
        {
            bool success = temp_commander.expend_energy(cost);
            return success;
        } else {
           return commander.expend_energy(cost);
        }
    }

    public void set_temp_commander(CommanderPiece temp) {
        temp_commander = temp;

        // add to temp commander list
        temp.soldiers_.Add(this);
        commander.soldiers_.Remove(this);
    }

    public CommanderPiece get_active_commander() {
        return (temp_commander != null) ? temp_commander : commander;
    }

    public bool is_temp_commander()
    {
        return (temp_commander != null) ? true : false;
    }


    public override void kill()
    {
        commander.remove_soldier(this);
    }

    public override abstract void Explore(ref Definitions.ActionDatabase results);
}

} // Piece
} // Chess
