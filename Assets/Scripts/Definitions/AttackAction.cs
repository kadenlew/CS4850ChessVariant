using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chess.Definitions;

namespace Chess
{
namespace Definitions
{

public class AttackAction : Action {
    public GameObject target { get; }
    int roll_modifer { get; } = 0;

    public AttackAction(
        GameObject agent, 
        GameObject target,
        int roll_modifer = 0
    ) {
        this.agent = agent;
        this.target = target;
        this.roll_modifer = roll_modifer;
    }

    // STATIC MEMBERS AND FUNCTIONS
    public static AttackResult checkAttack(ref AttackAction attack) {
        int roll = Random.Range((int)1, (int)7);
        return new AttackResult(
            roll,
            roll >= captureTable[(
                attack.agent.GetComponent<Piece.GamePieceBase>().type, 
                attack.target.GetComponent<Piece.GamePieceBase>().type
            )]   
        );
    }
    static IDictionary<(Piece.PieceType, Piece.PieceType), int> captureTable = new Dictionary<(Piece.PieceType, Piece.PieceType), int>() {
        {(Piece.PieceType.King,     Piece.PieceType.King),      4}, {(Piece.PieceType.King,     Piece.PieceType.Queen),     4}, 
        {(Piece.PieceType.King,     Piece.PieceType.Knight),    4}, {(Piece.PieceType.King,     Piece.PieceType.Bishop),    4}, 
        {(Piece.PieceType.King,     Piece.PieceType.Rook),      5}, {(Piece.PieceType.King,     Piece.PieceType.Pawn),      1},
        {(Piece.PieceType.Queen,    Piece.PieceType.King),      4}, {(Piece.PieceType.Queen,    Piece.PieceType.Queen),     4}, 
        {(Piece.PieceType.Queen,    Piece.PieceType.Knight),    4}, {(Piece.PieceType.Queen,    Piece.PieceType.Bishop),    4}, 
        {(Piece.PieceType.Queen,    Piece.PieceType.Rook),      5}, {(Piece.PieceType.Queen,    Piece.PieceType.Pawn),      2},
        {(Piece.PieceType.Knight,   Piece.PieceType.King),      6}, {(Piece.PieceType.Knight,   Piece.PieceType.Queen),     6}, 
        {(Piece.PieceType.Knight,   Piece.PieceType.Knight),    4}, {(Piece.PieceType.Knight,   Piece.PieceType.Bishop),    4}, 
        {(Piece.PieceType.Knight,   Piece.PieceType.Rook),      5}, {(Piece.PieceType.Knight,   Piece.PieceType.Pawn),      2},
        {(Piece.PieceType.Bishop,   Piece.PieceType.King),      5}, {(Piece.PieceType.Bishop,   Piece.PieceType.Queen),     5}, 
        {(Piece.PieceType.Bishop,   Piece.PieceType.Knight),    5}, {(Piece.PieceType.Bishop,   Piece.PieceType.Bishop),    4}, 
        {(Piece.PieceType.Bishop,   Piece.PieceType.Rook),      5}, {(Piece.PieceType.Bishop,   Piece.PieceType.Pawn),      3},
        {(Piece.PieceType.Rook,     Piece.PieceType.King),      4}, {(Piece.PieceType.Rook,     Piece.PieceType.Queen),     4}, 
        {(Piece.PieceType.Rook,     Piece.PieceType.Knight),    5}, {(Piece.PieceType.Rook,     Piece.PieceType.Bishop),    5}, 
        {(Piece.PieceType.Rook,     Piece.PieceType.Rook),      6}, {(Piece.PieceType.Rook,     Piece.PieceType.Pawn),      5},
        {(Piece.PieceType.Pawn,     Piece.PieceType.King),      6}, {(Piece.PieceType.Pawn,     Piece.PieceType.Queen),     6}, 
        {(Piece.PieceType.Pawn,     Piece.PieceType.Knight),    6}, {(Piece.PieceType.Pawn,     Piece.PieceType.Bishop),    5}, 
        {(Piece.PieceType.Pawn,     Piece.PieceType.Rook),      6}, {(Piece.PieceType.Pawn,     Piece.PieceType.Pawn),      4} 
    };

    public static bool operator== (AttackAction a, AttackAction b) => (
        GameObject.ReferenceEquals(a.agent, b.agent) &&
        GameObject.ReferenceEquals(a.target, b.target)
    );
    
    public static bool operator!= (AttackAction a, AttackAction b) => (
        !(a == b)
    );

    public override bool Equals(object obj)
    {
        if((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;

        AttackAction action = (AttackAction) obj;
        return action == this;
    }

    public override int GetHashCode() => (
        agent.GetComponent<Piece.GamePieceBase>().position.GetHashCode() * 1000 +
        target.GetComponent<Piece.GamePieceBase>().position.GetHashCode()
    );

    public override string ToString() => (
        $"{agent.GetComponent<Piece.GamePieceBase>()} attacks {target.GetComponent<Piece.GamePieceBase>()}" +
        $"{((roll_modifer != 10) ? $" with a roll modifier of {roll_modifer}" : "")}"
    );
        
}

} // Definitions
} // Chess