using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

// represents a change in board position. Used to have a constant list of 
// potential paths of motion for each explore function, that can merely be
// added to a boardPosition to generate new positions
public class BoardVector {
    // the magnitude of the file dimension
    public int file_length { get; }
    // the magnitude of the rank dimension
    public int rank_length { get;  }   
    
    // manhattan magnitude of the entire vector
    public int manhattan_mag {
        get { return System.Math.Abs(file_length) + System.Math.Abs(rank_length); }
    }

    public double special_mag {
        get {
            double delta_x = System.Math.Abs(rank_length);
            double delta_y = System.Math.Abs(file_length);
            return System.Math.Min(delta_x, delta_y) * System.Math.Sqrt(2) + System.Math.Abs(delta_x - delta_y);
        }
    }

    public BoardVector(
        int file_length,
        int rank_length
    ) {
        this.file_length = file_length;
        this.rank_length = rank_length;
    }

///////////////////////////////////////////////////////////////////////////
//                              OPERATORS
//////////////////////////////////////////////////////////////////////////

    public static BoardVector operator+ (BoardVector a, BoardVector b) => new BoardVector(
        a.file_length + b.file_length,
        a.rank_length + b.rank_length
    );

    public static BoardVector operator- (BoardVector a, BoardVector b) => new BoardVector(
        a.file_length - b.file_length,
        a.rank_length - b.rank_length
    );
}

// represents a position on the chess board. file represents the "x" position, and rank
// represents the "y" position. both range from [1-8] inclusive in valid positions, but can
// be constructed with invalid positions to aid search. is_valid is provided to add the ability
// to prevent invalid positions from being utilized.
//
// Changes in BoardPosition are best represented with a BoardVector, as is often the use case in
// moves about the board. Any addition or subtraction with a BoardVector will create a new BoardPosition
// and is the prefered way of finding out where a move would land. Subtracting 2 BoardPositions
// will return a BoardVector, indicating the distance between the 2 positions.
public class BoardPosition {
    // the "x" position. valid range is [1-8] inclusive. represented with the 
    // the characters [a-h] in string form
    public int file { get; protected set; }
    // the "y" position. valid range is [1-8] inclusive. 
    public int rank { get; protected set; }
    
    // determines if this current object can represent a physical square on a standard
    // 8x8 chess board
    public bool is_valid {
        get { return !(file < 1 || file > 8 || rank < 1 || rank > 8); }
    }

    // constructor
    public BoardPosition(int file, int rank) {
        this.file = file;
        this.rank = rank;
    }

    // constructor given algebraic notation string
    public BoardPosition(string notation) {
        if(notation.Length != 2)
        {
            throw new System.ArgumentException("Expected standard Chess Algebraic Notation");
        }

        // grab the values from the chess notation
        this.file =  ((int) notation[0] - (int) 'a') + 1;
        this.rank = int.Parse(notation[1].ToString());
    }

    // convert file rank (1 indexed) to chess notation
    public override string ToString() {
        return (
            (char)(
                ((int)'a') - 1 + file)
            ).ToString() + rank;
    }

///////////////////////////////////////////////////////////////////////////
//                              OPERATORS
//////////////////////////////////////////////////////////////////////////

    public static BoardPosition operator+ (BoardPosition a, BoardVector b) => new BoardPosition(
        a.file + b.file_length,
        a.rank + b.rank_length
    );

    public static BoardPosition operator- (BoardPosition a, BoardVector b) => new BoardPosition(
        a.file - b.file_length,
        a.rank - b.rank_length
    );

    public static BoardVector operator- (BoardPosition a, BoardPosition b) => new BoardVector(
        a.file - b.file,
        a.rank - b.rank
    );

    public static bool operator== (BoardPosition a, BoardPosition b) {
        if(object.ReferenceEquals(a, null))
            return object.ReferenceEquals(b, null);
        if(object.ReferenceEquals(b, null))
            return object.ReferenceEquals(a, null);
        return a.file == b.file && a.rank == b.rank;
    }

    public static bool operator!= (BoardPosition a, BoardPosition b) => (
        !(a == b)
    );

    public override bool Equals(System.Object obj){
        if((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        
        BoardPosition pos = (BoardPosition) obj;
        return pos == this;
    }

    public override int GetHashCode() => 10 * this.file + this.rank;

}

}   // Definitions
}   // Chess

