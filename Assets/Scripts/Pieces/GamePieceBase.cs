using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess 
{
namespace Piece
{

// Describes what the different types of pieces are for use when
// an explicit type needs to be applied to a game object
// mainly used for the roll table 
public enum PieceType {
    King,
    Queen,
    Bishop,
    Rook,
    Knight,
    Pawn,
}

// the abstract base structure of a GamePiece which includes all of the required 
// methods and members of a GamePiece for interaction in a generic collection.
public abstract class GamePieceBase : MonoBehaviour {
    // Material for the renderer to use when the piece is not selected by the user
    public Material standard;
    // Material for the renderer to use when the piece is selected by the user
    public Material selected;
    // the physical space this piece occupies; abstracts the Vector3d of unity 
    // to keep all logic within "Board Space"
    public Definitions.BoardPosition position { get; protected set; }
    
    // determines what side this piece is on
    public bool is_white { get; protected set; }
    
    // the type this piece is for use in dictionary lookups
    public abstract PieceType type { get; }

    // reference the the BoardController for access to the board searching functions
    public BoardController controller_ref { get; set; }

    // Unity prefabs and materials used to correctly represent a piece
    protected Definitions.PrefabCollection prefabs_;

    // the generic access point for all pieces to get all of the valid moves of this piece
    // It can be assumed that only valid moves are appended to results, and the collection
    // of moves added is Exhuastive, meaning that it represents every potential valid move
    // this piece can make
    public abstract void Explore(ref HashSet<Definitions.Action> results);

    public void Select() {
        GetComponentInChildren<Renderer>().material = selected;
    }

    public void Deselect() {
        GetComponentInChildren<Renderer>().material = standard;
    }

    // Function called on object creation, initializing the properties of this gameObject
    // and getting it synched with the overall board state
    public void init(
        bool is_white, 
        Definitions.BoardPosition starting_position, 
        BoardController controller, 
        Definitions.PrefabCollection prefabs
    ) {
        // board space related items
        this.position = starting_position;
        this.is_white = is_white;
        this.controller_ref = controller;

        // unity related items
        this.prefabs_ = prefabs;
        this.standard = prefabs_.pieceColors[is_white ? 0 : 1];
        this.selected = prefabs_.pieceColorsSelected[is_white ? 0 : 1];

        this.GetComponentInChildren<Renderer>().material = this.standard;
    }

    public override string ToString() => $"{(this.is_white ? "White" : "Black")} {this.type} {this.position}";
        
}

} // Piece
} // Chess