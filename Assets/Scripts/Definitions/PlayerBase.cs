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
    protected List<GameObject> commanders_;

    // a cache of all the possible actions the pieces this player owns can currently execute. It can
    // be safely assumed that all actions in here are valid, and represent the complete set of all
    // possible actions the pieces this player owns can execute
    protected HashSet<Definitions.Action> possible_actions = new HashSet<Definitions.Action>();

    // Unity prefab information for piece spawning
    protected Definitions.PrefabCollection prefabs_;

    // whether this player owns the white pieces or the black pieces
    public bool is_white { get; } 
    
    // A lookup table used by boardController associating each commanders soldiers to the commander itself
    public List<(GameObject, List<GameObject>)> pieces { get {
        List<(GameObject, List<GameObject>)> res = new List<(GameObject, List<GameObject>)>(commanders_.Count);
        foreach(var commander in commanders_) {
            res.Add(
                (commander, commander.GetComponent<Piece.CommanderPiece>().soldiers_)
            );
        }
        return res;
    } }

    // constructor
    public PlayerBase(bool is_white, Definitions.PrefabCollection prefabs, BoardController controller) {
        // board space information
        this.is_white = is_white;

        // unity information
        prefabs_ = prefabs;

        // defines the list of commanders to spawn 
        var spawnList = new List<(GameObject, Definitions.BoardPosition)>()
        {
            (prefabs_.King,     new Definitions.BoardPosition(5, is_white ? 2 : 7)),
            (prefabs_.Bishop,   new Definitions.BoardPosition(3, is_white ? 1 : 8)),
            (prefabs_.Bishop,   new Definitions.BoardPosition(6, is_white ? 1 : 8))
        };

        // allocate the commanders and pieces lists
        commanders_ = new List<GameObject>(spawnList.Count);

        // spawn each commander specified above and call its init function
        // store references to all of the pieces that have been spawned grouped
        // by commander for quick reference
        foreach((GameObject piece, Definitions.BoardPosition pos) in spawnList)
        {
            // spawn the commander
            commanders_.Add(
                GameObject.Instantiate(piece)
            );

            commanders_[commanders_.Count - 1].GetComponent<Piece.CommanderPiece>().commander_init(
                is_white,
                pos,
                prefabs_,
                controller,
                this
            );
        }
    } 

    // function called by external sources to update its possible actions lookup table
    public void explore_actions() {
        UnityEngine.Profiling.Profiler.BeginSample("Explore all Player Actions");

        // mark the previous actions invalid
        possible_actions.Clear();

        // get the actions each commander knows of (which inturn gets the actions those soldiers know of)
        foreach(var commander in commanders_)
            commander.GetComponent<Piece.CommanderPiece>().commander_explore(ref possible_actions);

        UnityEngine.Profiling.Profiler.EndSample();
        // Debug.Log($"{possible_actions.Count}");
    }

    public bool remove_commander(GameObject commander) {
        Debug.Log($"{(is_white ? "WHITE" : "BLACK")}: killing commander {commander.GetComponent<Piece.GamePieceBase>()}");
        // are we trying to kill our leader?
        if(Object.ReferenceEquals(commander, commanders_[0]))
            // yes, game over
            return false;

        // copy the soldiers to this players lead commander
        foreach(var soldier in commander.GetComponent<Piece.CommanderPiece>().soldiers_) {
            Debug.Log($"{this}: transfering soldier {soldier.GetComponent<Piece.GamePieceBase>()} to {commanders_[0].GetComponent<Piece.GamePieceBase>()}");
            commanders_[0].GetComponent<Piece.CommanderPiece>().soldiers_.Add(
                soldier
            );

            soldier.GetComponent<Piece.SoldierPiece>().commander = commanders_[0];
        }

        commanders_.Remove(commander);

        // kill the game object itself
        UnityEngine.Object.Destroy(commander);

        // we transfered the pieces to the leader, continue the game
        return true;
    }

    public void begin_turn() {
        foreach(var commander in commanders_)
            commander.GetComponent<Piece.CommanderPiece>().begin_turn();
    }

    public void end_turn() {
        foreach(var commander in commanders_)
            commander.GetComponent<Piece.CommanderPiece>().end_turn();
    }

    public HashSet<Definitions.Action> get_possible_actions() => possible_actions;
}

} // Control
} // Chess
