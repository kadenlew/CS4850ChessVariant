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
    public PlayerBase(bool is_white, Definitions.PrefabCollection prefabs) {
        is_white_ = is_white;
        prefabs_ = prefabs;

        // there will always be 3 commanders
        commanders_ = new List<GameObject>();

        // instantiate the king
        commanders_.Add(
            GameObject.Instantiate(prefabs_.King)
        );        
        // update its properties
        commanders_[0].GetComponent<Piece.CommanderPiece>().commander_init(
            is_white_, 
            new Definitions.BoardPosition(
                5,
                is_white_ ? 1 : 8
            ),
            prefabs_
        );

        // instantiate left bishop
        commanders_.Add(
            GameObject.Instantiate(prefabs_.Bishop)
        );
        // update its properties
        commanders_[1].GetComponent<Piece.CommanderPiece>().commander_init(
            is_white_, 
            new Definitions.BoardPosition(
                3,
                is_white_ ? 1 : 8
            ),
            prefabs_
        );

        // instantiate right bishop
        commanders_.Add(
            GameObject.Instantiate(prefabs_.Bishop)
        );
        // update its properties
        commanders_[2].GetComponent<Piece.CommanderPiece>().commander_init(
            is_white_, 
            new Definitions.BoardPosition(
                6,
                is_white_ ? 1 : 8
            ),
            prefabs_
        );

    } 

    public bool is_white() {
        return is_white_;
    }

    public ref List<GameObject> getCommanders() {
        return ref commanders_;
    }
}

}
}