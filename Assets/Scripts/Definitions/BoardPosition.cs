using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

public class BoardVector {
    public int file_length { get; }
    public int rank_length { get;  }   
    public int manhattan_mag {
        get { return System.Math.Abs(file_length) + System.Math.Abs(rank_length); }
    }

    public BoardVector(
        int file_length,
        int rank_length
    ) {
        this.file_length = file_length;
        this.rank_length = rank_length;
    }

    public static BoardVector operator+ (BoardVector a, BoardVector b) => new BoardVector(
        a.file_length + b.file_length,
        a.rank_length + b.rank_length
    );

    public static BoardVector operator- (BoardVector a, BoardVector b) => new BoardVector(
        a.file_length - b.file_length,
        a.rank_length - b.rank_length
    );
}

public class BoardPosition {
    public int file { get; protected set; }
    public int rank { get; protected set; }

    public BoardPosition(int file, int rank) {
        this.file = file;
        this.rank = rank;
    }

    public BoardPosition(string notation) {
        if(notation.Length != 2)
        {
            throw new System.ArgumentException("Expected standard Chess Algebraic Notation");
        }

        // grab the values from the chess notation
        int file_t =  ((int) notation[0] - (int) 'a') + 1;
        int rank_t = int.Parse(notation[1].ToString());

        // use the update position function for boundary checking
        update_position(file_t, rank_t);
    }

    // convert file rank (1 indexed) to chess notation
    public override string ToString() {
        return (
            (char)(
                ((int)'a') - 1 + file)
            ).ToString() + rank;
    }

    public bool update_position(int file, int rank) {
        // error check
        if(file < 1 || file > 8 || rank < 1 || rank > 8)
        {
            // throw new System.ArgumentException("file and rank must both be in the range of 1-8 inclusive");
            return false;
        }
        
        // update the position
        this.file = file;
        this.rank = rank;

        return true;
    }

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

    public static bool operator== (BoardPosition a, BoardPosition b) => (
        a.file == b.file && a.rank == b.rank
    );

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

