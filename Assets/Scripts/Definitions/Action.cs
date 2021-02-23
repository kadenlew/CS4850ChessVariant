using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

public class Action 
{
    IDictionary<string, int> captureTable = new Dictionary<string, int>(){
        {"King, King", 4}, {"King, Queen", 4}, {"King, Knight", 4}, {"King, Bishop", 4}, {"King, Rook", 5}, {"King, Pawn", 1},
        {"Queen, King", 4}, {"Queen, Queen", 4}, {"Queen, Knight", 4}, {"Queen, Bishop", 4}, {"Queen, Rook", 5}, {"Queen, Pawn", 2},
        {"Knight, King", 6}, {"Knight, Queen", 6}, {"Knight, Knight", 4}, {"Knight, Bishop", 4}, {"Knight, Rook", 5}, {"Knight, Pawn", 2},
        {"Bishop, King", 5}, {"Bishop, Queen", 5}, {"Bishop, Knight", 5}, {"Bishop, Bishop", 4}, {"Bishop, Rook", 5}, {"Bishop, Pawn", 3},
        {"Rook, King", 4}, {"Rook, Queen", 4}, {"Rook, Knight", 5}, {"Rook, Bishop", 5}, {"Rook, Rook", 6}, {"Rook, Pawn", 5},
        {"Pawn, King", 6}, {"Pawn, Queen", 6}, {"Pawn, Knight", 6}, {"Pawn, Bishop", 5}, {"Pawn, Rook", 6}, {"Pawn, Pawn", 4} 
    }

    public AttackResult checkAttack(ref AttackAction){
        // Recieve game piece information
        // Convert game pieces to concated string

        string gamePieces;
        int roll = random.Next(1,7);
        int minRoll;
        bool attack = false;

        if(roll >= captureTable.TryGetValue(gamePieces, out minRoll)){
            attack = true;
        }
        else{
            attack = false;
        }
        return roll, attack;

    }
}



}
}