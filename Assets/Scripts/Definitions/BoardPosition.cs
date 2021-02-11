using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

public class BoardPosition
{
    private int file_;
    private int rank_;

    public BoardPosition(int file, int rank) {
        file_ = file;
        rank_ = rank;
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
                ((int)'a') - 1 + file_)
            ).ToString() + rank_;
    }

    public int get_file() {
        return file_;
    }

    public int get_rank() {
        return rank_;
    }

    public void update_position(int file, int rank) {
        // error check
        if(file < 1 || file > 8 || rank < 1 || rank > 8)
        {
            throw new System.ArgumentException("file and rank must both be in the range of 1-8 inclusive");
        }
        
        // update the position
        file_ = file;
        rank_ = rank;
    }
}

}   // Definitions
}   // Chess
