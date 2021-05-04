using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chess.Definitions;

namespace Chess
{
namespace Definitions
{

// An action that involves 1 piece attacking a target piece. There is a probability that this attack with succeed given
// the roll of a d6. If the roll passes, the attack carries out, and the piece being attacked is removed from the game.
// if the roll fails, nothing occurs, and the action point used to execute this is wasted. The probability is based on
// an intersection table provided below
public class AttackAction : Action {
    // the target piece that is under attack
    public Piece.GamePieceBase target { get; }
    // represents any positive or negative bonus applied to the random roll before checking that value with the roll table
    int roll_modifer { get; } = 0;

    // used by UI for materiel tracking
    public Piece.PieceType targetType;

    // constructor
    public AttackAction(
        Piece.GamePieceBase agent, 
        Piece.GamePieceBase target,
        int roll_modifer = 0
    ) {
        this.agent = agent;
        this.target = target;
        this.roll_modifer = roll_modifer;
        targetType = target.type;
    }

    public override Result Execute(BoardController controller) {
        // use the corp energy
        agent.expend_energy(1);

        // do the roll
        var result = checkAttack();

        // if we passed our check, kill
        if(result.was_successful)
        {
            // move to the targets board position
            if (agent.type != Piece.PieceType.Rook)
            {
                agent.move(
                    target.position
                );
            }
            // remove that target from the game
            target.kill();
        }

        return result;
    }

///////////////////////////////////////////////////////////////////////////
//                      STATIC FUNCTIONS AND MEMBERS
///////////////////////////////////////////////////////////////////////////

    // given an AttackAction object, check what roll is required for that attack to succeed.
    // if return whether the attack succeeded, and what roll was gotten
    public AttackResult checkAttack() {
        int roll = Random.Range((int)1, (int)7);
        return new AttackResult(
            roll + roll_modifer,
            roll + roll_modifer >= captureTable[(
                agent.type, 
                target.type
            )],
            target.type
        );
    }

    // the Roll table which defines what rolls are required when <AttackingPieceType> is attacking <DefendingPieceType>
    public static IDictionary<(Piece.PieceType, Piece.PieceType), int> captureTable = new Dictionary<(Piece.PieceType, Piece.PieceType), int>() {
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

    public static double get_roll_prob(Piece.PieceType attacking, Piece.PieceType defending) {
        return (6 - captureTable[(attacking, defending)] + 1) / 6.0;
    }

///////////////////////////////////////////////////////////////////////////
//                              OPERATORS
///////////////////////////////////////////////////////////////////////////

    public static bool operator== (AttackAction a, AttackAction b) => (
        ReferenceEquals(a.agent, b.agent) &&
        ReferenceEquals(a.target, b.target)
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
        agent.position.GetHashCode() * 1000 +
        target.position.GetHashCode()
    );

    public override string ToString() => (
        $"{agent} attacks {target}" +
        // $"with a roll modifer of {roll_modifer}"
        $"{((roll_modifer != 0) ? $" with a roll modifier of {roll_modifer}" : "")}"
    );
        
}

public class AttackResult : Result  {
    // what the d6 (plus any modifer) resulted in
    public int roll_result { get; }
    public Piece.PieceType targetType;
    // whether that roll was successful, given the roll table

    // constructor
    public AttackResult(
        int roll_result,
        bool was_successful,
        Piece.PieceType targetType
    ) {
        this.roll_result = roll_result;
        this.was_successful = was_successful;
        this.targetType = targetType;
    }

    public override string ToString() => (
        $"Rolled a {roll_result}. {(was_successful ? "Success!" : "Failed!")}" 
    );
}

} // Definitions
} // Chess
