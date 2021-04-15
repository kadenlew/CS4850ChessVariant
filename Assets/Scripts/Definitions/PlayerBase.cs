using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Control
{

// the base implentation of a player, or 1 of the 2 sides involved in the game. Is the owner of 
// all of its pieces. This defines the required communication pipeline that all agents interested
// in interacting with the board require. Can be extended (I.E for AI) to allow it to interact with 
// the board
public class PlayerBase {
    // the commanders this player owns. Each commander owns its soldiers, so the player also owns 
    // those pieces by association
    protected List<Piece.CommanderPiece> commanders_;

    protected Definitions.ActionDatabase possible_actions;

    // Unity prefab information for piece spawning
    protected Definitions.PrefabCollection prefabs_;
    protected BoardController controller_ref { get; set; }

    // whether this player owns the white pieces or the black pieces
    public bool is_white { get; protected set; } 
    
    // A lookup table used by boardController associating each commanders soldiers to the commander itself
    public List<(Piece.CommanderPiece, List<Piece.SoldierPiece>)> pieces { get {
        // allocate the result memory location
        List<(Piece.CommanderPiece, List<Piece.SoldierPiece>)> res = new List<(Piece.CommanderPiece, List<Piece.SoldierPiece>)>(commanders_.Count);
        
        // build each corp
        foreach(var commander in commanders_) {
            // add soldiers to this commanders corp
            res.Add(
                (commander, commander.soldiers_)
            );
        }

        return res;
    } }

    // constructor
    public PlayerBase(
        bool is_white, 
        Definitions.PrefabCollection prefabs, 
        BoardController controller,
        Definitions.ActionDatabase action_database
    ) {
        // board space information
        this.is_white = is_white;

        // unity information
        prefabs_ = prefabs;

        // defines the list of commanders to spawn 
        var spawnList = new List<(GameObject, Definitions.BoardPosition)>()
        {
            (prefabs_.King,     new Definitions.BoardPosition(5, is_white ? 1 : 8)),
            (prefabs_.Bishop,   new Definitions.BoardPosition(3, is_white ? 1 : 8)),
            (prefabs_.Bishop,   new Definitions.BoardPosition(6, is_white ? 1 : 8))
        };

        // allocate the commanders and pieces lists
        commanders_ = new List<Piece.CommanderPiece>(spawnList.Count);

        // spawn each commander specified above and call its init function
        // store references to all of the pieces that have been spawned grouped
        // by commander for quick reference
        foreach((GameObject piece, Definitions.BoardPosition pos) in spawnList)
        {
            // spawn the commander, and store the reference to its script
            commanders_.Add(
                GameObject.Instantiate(piece).GetComponent<Piece.CommanderPiece>()
            );

            // init the script information for this commander
            commanders_[commanders_.Count - 1].commander_init(
                is_white,       // side information
                pos,            // starting position
                prefabs_,       // model + script setup information
                controller,     // board information
                this            // owner information
            );
        }

        possible_actions = action_database;
        controller_ref = controller;
    } 

    // function called by external sources to update its possible actions lookup table
    public void explore_actions() {
        // get the actions each commander knows of (which inturn gets the actions those soldiers know of)
        foreach(var commander in commanders_)
            commander.commander_explore(ref possible_actions);
    }

    public bool remove_commander(Piece.CommanderPiece commander) {
        Debug.Log($"{(is_white ? "WHITE" : "BLACK")}: killing commander {commander}");

        // are we trying to kill our leader?
        if(ReferenceEquals(commander, commanders_[0]))
        {
            controller_ref.end_game(is_white);
            // yes, game over
            return false;
        }

        // copy the soldiers to this players lead commander
        foreach(var soldier in commander.soldiers_) {
            Debug.Log($"{this}: transfering soldier {soldier} to {commanders_[0]}");
            commanders_[0].soldiers_.Add(
                soldier
            );

            soldier.commander = commanders_[0];
        }

        // we no longer have this commander
        commanders_.Remove(commander);

        // kill the game object itself
        UnityEngine.Object.Destroy(commander.gameObject);

        // we transfered the pieces to the leader, continue the game
        return true;
    }

    public virtual void begin_turn() {
        // do the begin turn step for each corp
        foreach(var commander in commanders_)
            commander.begin_turn();
    }

    public virtual void end_turn() {
        // do the end turn step ofr each corp
        foreach(var commander in commanders_)
            commander.end_turn();
    }
}

} // Control
} // Chess
