using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chess.Definitions;

namespace Chess 
{
namespace Piece
{

public class GamePieceBase : MonoBehaviour
{
    public Material standard;
    public Material selected;

    protected BoardPosition position_;

    protected bool is_white_;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Select()
    {
        GetComponentInChildren<Renderer>().material = selected;
    }

    public void Deselect()
    {
        GetComponentInChildren<Renderer>().material = standard;
    }

    public void init(bool is_white, BoardPosition starting_position) {
        position_ = starting_position;
        is_white_ = is_white;
    }

    public Definitions.BoardPosition GetBoardPosition() => this.position_;

    public bool is_white() => this.is_white_;

}

}
}