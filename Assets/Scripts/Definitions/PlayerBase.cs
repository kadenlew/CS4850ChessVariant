using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Control
{

public class PlayerBase 
{
    protected List<GameObject> commanders_;

    private List<Definitions.Action> possible_actions_;

    Definitions.PrefabCollection prefabs_;

    private bool is_white_; 

    protected List<(GameObject, Definitions.BoardPosition)> spawnList_;
    
    protected List<(GameObject, List<GameObject>)> pieces_;


    public PlayerBase(bool is_white, Definitions.PrefabCollection prefabs) {
        is_white_ = is_white;
        prefabs_ = prefabs;

        // defines the list of commanders to spawn 
        spawnList_ = new List<(GameObject, Definitions.BoardPosition)>()
        {
            (prefabs_.King,     new Definitions.BoardPosition(5, is_white_ ? 1 : 8)),
            (prefabs_.Bishop,   new Definitions.BoardPosition(3, is_white_ ? 1 : 8)),
            (prefabs_.Bishop,   new Definitions.BoardPosition(6, is_white_ ? 1 : 8))
        };

        // there will always be 3 commanders
        commanders_ = new List<GameObject>(spawnList_.Count);
        pieces_ = new List<(GameObject, List<GameObject>)>(spawnList_.Count);

        // spawn each commander specified above and call its init function
        // store references to all of the pieces that have been spawned grouped
        // by commander for quick reference
        foreach((GameObject piece, Definitions.BoardPosition pos) in spawnList_)
        {
            commanders_.Add(
                GameObject.Instantiate(piece)
            );


            pieces_.Add(
                (
                    commanders_[commanders_.Count - 1], 
                    commanders_[commanders_.Count - 1].GetComponent<Piece.CommanderPiece>().commander_init(
                        is_white,
                        pos,
                        prefabs_
                    )
                )
            );
        }
    } 

    public bool is_white() {
        return is_white_;
    }

    public ref List<GameObject> getCommanders()  => ref commanders_;

    public ref List<(GameObject, List<GameObject>)> getPieces() => ref pieces_;

    
}

}
}