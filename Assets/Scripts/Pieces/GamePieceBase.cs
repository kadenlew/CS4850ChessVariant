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
    // the physical space this piece occupies; abstracts the Vector3d of unity 
    // to keep all logic within "Board Space"
    public Definitions.BoardPosition position { get; protected set; }
    
    // determines what side this piece is on
    public bool is_white { get; protected set; }

    public abstract int material_value { get; }
    
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
    public abstract void Explore(ref Definitions.ActionDatabase results);

    public void Select(Color highlight_color) {
        this.GetComponent<Outline>().enabled = true;
        this.GetComponent<Outline>().OutlineColor = highlight_color;
    }

    public void Deselect() {
        this.GetComponent<Outline>().enabled = false;
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

        // set the material color
        this.GetComponentInChildren<Renderer>().material = prefabs_.pieceColors[is_white ? 0 : 1];
    }

    public void move(Definitions.BoardPosition position) {
        this.position = position;
    }

    public abstract bool expend_energy(uint cost);

    public abstract void kill();

    public override string ToString() => $"{(is_white ? "White" : "Black")} {type} {position}";
        
}

} // Piece
} // Chess
