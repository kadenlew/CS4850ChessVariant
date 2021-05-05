using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Piece
{

// abstract class representing the additional duties a commander piece has 
// over the standard piece
public abstract class CommanderPiece : GamePieceBase {
    // the list of soliders this commander owns

    public List<Piece.SoldierPiece> soldiers_ { get; protected set; }

    // a list that is used on object creation to indicate which soliders 
    // this piece will own, and is used to spawn each of those soliders in
    protected List<(GameObject, Definitions.BoardPosition)> spawnList_;

    // energy for this corp
    public uint energy { get; protected set; } 

    // owner of this corp
    public Control.PlayerBase owner { get; protected set; } 

    // Object init specific to commanders, allowing each of them to 
    // specify what pieces they will command and spawn them in
    public virtual void commander_init(
        bool is_white, 
        Definitions.BoardPosition starting_position,
        Definitions.PrefabCollection prefabs,
        BoardController controller,
        Control.PlayerBase owner
    ) {
        // do the standard piece init as well
        this.init(is_white, starting_position, controller, prefabs);

        this.energy = 1;

        this.owner = owner;
    }

    // given the spawnList set during commander init, spawn in each soldier 
    // specified
    protected void spawn_units(BoardController controller) {
        // reset the soldier list and reserve enough space for our units
        soldiers_ = new List<Piece.SoldierPiece>(spawnList_.Count);

        // for each unit thats been defined, spawn it in and call its init function 
        // with the board position specified
        foreach((GameObject piece, Definitions.BoardPosition pos) in spawnList_)
        {
            // spawn the soldier and store the reference to its script
            soldiers_.Add(
                GameObject.Instantiate(piece).GetComponent<Piece.SoldierPiece>()
            );

            // call its init function to sync it with the board
            soldiers_[soldiers_.Count - 1].soldier_init(
                is_white,
                pos,
                this,
                controller,
                prefabs_
            );
        }
    }

    // function dealing with delegating out a command for each of its soldiers to explore its space
    // also calls its own explore function itself
    public void commander_explore(ref Definitions.ActionDatabase results) {
        // only search if we haven't moved our corp yet
        if(energy <= 0) {
            return;
        }

        // search the entire corp
        foreach(var soldier in soldiers_)
            soldier.Explore(ref results);

        // search ourself as well
        Explore(ref results);

    }

    public override bool expend_energy(uint cost) {
        if(cost <= energy) {
            energy -= cost;
            return true;
        }
        return false;
    }

    public override void kill()
    {
        owner.remove_commander(this);
    }

    public void end_turn() {
        List<SoldierPiece> to_migrate = new List<SoldierPiece>();
        foreach(var soldier in soldiers_)
        {
            if(soldier.temp_commander != null)
            {
                to_migrate.Add(soldier);
                continue;
            }
            soldier.GetComponent<GamePieceBase>().Deselect();
        }
        this.energy = 1;
        this.Deselect();

        foreach(var soldier in to_migrate)
        {
            soldier.commander.soldiers_.Add(soldier);
            soldier.temp_commander.soldiers_.Remove(soldier);
            soldier.temp_commander = null;
        } 
    }

    public void begin_turn() {
    }

    public void remove_soldier(Piece.SoldierPiece soldier) {
        // Debug.Log($"{this}: killing soldier {soldier}");

        // we no longer have this soldier
        soldiers_.Remove(
            soldier
        );

        // kill the game object that represents it
        Destroy(soldier.gameObject);
    }

    // forwarding the abstract piece explore function
    public override abstract void Explore(ref Definitions.ActionDatabase results);
}


} // Piece
} // Chess
