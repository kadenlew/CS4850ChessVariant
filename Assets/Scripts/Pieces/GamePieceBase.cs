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

    protected Renderer targetRenderer;

    protected BoardPosition position_;

    // new function Explore() with a return type of a list of actions, abstract here, implement in the pieces
    // public override of this function in all of the pieces and return a new list object
    // have a way for the commander to store all of the pieces, within commander

    // method Explore(), not sure exactly what input will be or list type will be, so leaving it blank/string for now. can't be abstract unless entire class is abstract
    public List<string> Explore()
    {
        List<string> pieceActions = new List<string>();
        // loop that will iterate per action, and will terminate once all actions of a piece have been added, something like 
        // foreach (var action in Actions) { run some method that will return an object/action that will be added to list }
        // pieceActions.add(action);
        return pieceActions;
    }

    // Start is called before the first frame update
    void Start()
    {
        targetRenderer = GetComponentInChildren<Renderer>();
        Deselect();

        position_ = new BoardPosition(1, 1);
        Debug.Log(position_);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        targetRenderer.material = selected;
    }

    public void Deselect()
    {
        targetRenderer.material = standard;
    }

    public void init(BoardPosition starting_position) {
        position_ = starting_position;
    }

}

}
}