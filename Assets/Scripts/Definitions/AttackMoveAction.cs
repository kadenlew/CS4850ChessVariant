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
    // space that piece will move to should it fail an attack
    public BoardPosition failsafe { get; }

    // constructor
    public AttackMoveAction(
        Piece.GamePieceBase agent, 
        Piece.GamePieceBase target,
        BoardPosition failsafe
    ) {
        this.agent = agent;
        this.target = target;
        targetType = target.type;
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
                failsafe.position
            );
        }

        return result;
    }

}
}
}