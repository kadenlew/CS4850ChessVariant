using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Control
{

public class PlayerBase {
    protected List<GameObject> commanders_;

    public HashSet<Definitions.Action> possible_actions = new HashSet<Definitions.Action>();

    protected Definitions.PrefabCollection prefabs_;

    public bool is_white { get; } 
    
    public List<(GameObject, List<GameObject>)> pieces { get; protected set; }


    public PlayerBase(bool is_white, Definitions.PrefabCollection prefabs, BoardController controller) {
        this.is_white = is_white;
        prefabs_ = prefabs;

        // defines the list of commanders to spawn 
        var spawnList = new List<(GameObject, Definitions.BoardPosition)>()
        {
            (prefabs_.King,     new Definitions.BoardPosition(5, is_white ? 2 : 7)),
            (prefabs_.Bishop,   new Definitions.BoardPosition(3, is_white ? 1 : 8)),
            (prefabs_.Bishop,   new Definitions.BoardPosition(6, is_white ? 1 : 8))
        };

        // there will always be 3 commanders
        commanders_ = new List<GameObject>(spawnList.Count);
        pieces = new List<(GameObject, List<GameObject>)>(spawnList.Count);

        // spawn each commander specified above and call its init function
        // store references to all of the pieces that have been spawned grouped
        // by commander for quick reference
        foreach((GameObject piece, Definitions.BoardPosition pos) in spawnList)
        {
            commanders_.Add(
                GameObject.Instantiate(piece)
            );


            pieces.Add(
                (
                    commanders_[commanders_.Count - 1], 
                    commanders_[commanders_.Count - 1].GetComponent<Piece.CommanderPiece>().commander_init(
                        is_white,
                        pos,
                        prefabs_,
                        controller
                    )
                )
            );
        }
    } 

    public void explore_actions() {
        UnityEngine.Profiling.Profiler.BeginSample("Explore all Player Actions");

        possible_actions.Clear();
        foreach(var commander in commanders_)
            commander.GetComponent<Piece.CommanderPiece>().commander_explore(ref possible_actions);

        UnityEngine.Profiling.Profiler.EndSample();
        // Debug.Log($"{possible_actions.Count}");
    }
}

} // Control
} // Chess