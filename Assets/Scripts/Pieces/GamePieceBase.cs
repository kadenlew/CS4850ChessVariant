﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Chess 
{
namespace Piece
{

public enum PieceType {
    King,
    Queen,
    Bishop,
    Rook,
    Knight,
    Pawn,
}

public abstract class GamePieceBase : MonoBehaviour {
    public Material standard;
    public Material selected;
    public Definitions.BoardPosition position { get; protected set; }

    public BoardController controller_ref { get; set; }

    // new function Explore() with a return type of a list of actions, abstract here, implement in the pieces
    // public override of this function in all of the pieces and return a new list object
    // have a way for the commander to store all of the pieces, within commander

    // method Explore(), not sure exactly what input will be or list type will be, so leaving it blank/string for now. can't be abstract unless entire class is abstract
    public abstract List<Definitions.Action> Explore();
        // List<string> pieceActions = new List<string>();
        // loop that will iterate per action, and will terminate once all actions of a piece have been added, something like 
        // foreach (var action in Actions) { run some method that will return an object/action that will be added to list }
        // pieceActions.add(action);
        // return pieceActions;


    public bool is_white { get; protected set; }

    public abstract PieceType type { get; }
    public void Select() {
        GetComponentInChildren<Renderer>().material = selected;
        Debug.Log("I am here");
        this.Explore();
    }

    public void Deselect() {
        GetComponentInChildren<Renderer>().material = standard;
    }

    public void init(bool is_white, Definitions.BoardPosition starting_position, BoardController controller) {
        this.position = starting_position;
        this.is_white = is_white;
        this.controller_ref = controller;
    }

    public override string ToString() => $"{(this.is_white ? "White" : "Black")} {this.type} {this.position}";
        
}

} // Piece
} // Chess