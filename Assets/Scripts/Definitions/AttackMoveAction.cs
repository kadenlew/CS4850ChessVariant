using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chess.Definitions;

namespace Chess
{
namespace Definitions
{

// An action that involves 1 agent piece attacking a target piece, and reserves a failsafe space to move to
// should the attack roll fail against the target
public class AttackMoveAction : AttackAction {
    // space that knight will move to should it fail a moving attack
    public BoardPosition failsafe { get; }

    // constructor
    public AttackMoveAction(
        Piece.GamePieceBase agent, 
        Piece.GamePieceBase target,
        BoardPosition failsafe
    ) : base(agent, target, -1) {
        this.failsafe = failsafe;
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
            agent.move(
                target.position
            );
            // remove that target from the game
            target.kill();
        }
        // if attack fails then move to the failsafe position
        else
        {
            agent.move(
                failsafe
            );
        }

        return result;
    }

///////////////////////////////////////////////////////////////////////////
//                              OPERATORS
///////////////////////////////////////////////////////////////////////////

    public static bool operator== (AttackMoveAction a, AttackMoveAction b) => (
        ReferenceEquals(a.agent, b.agent) &&
        ReferenceEquals(a.target, b.target) &&
        ReferenceEquals(a.failsafe, b.failsafe)
    );
    
    public static bool operator!= (AttackMoveAction a, AttackMoveAction b) => (
        !(a == b)
    );

    public override bool Equals(object obj)
    {
        if((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;

        AttackMoveAction action = (AttackMoveAction) obj;
        return action == this;
    }

    public override int GetHashCode() => (
        failsafe.GetHashCode() * 100000 +
        agent.position.GetHashCode() * 1000 +
        target.position.GetHashCode()
    );
}

} // Definitions
} // Chess