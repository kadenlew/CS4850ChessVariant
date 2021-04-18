using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

// reprents an action that is the movement of the agent from its current position
// to a target board position.  
public class MoveAction : Action
{
    // target position the piece will be moving to
    public BoardPosition target { get; }

    // constructor
    public MoveAction(Piece.GamePieceBase agent, BoardPosition target) {
       this.agent = agent;
       this.target = target;
    }
    public override Result Execute(BoardController controller) {
        // spend the energy
        agent.expend_energy(1);

        // move
        agent.move(
            target
        );

        // return default move result
        return new MoveResult();
    }


///////////////////////////////////////////////////////////////////////////
//                              OPERATORS
//////////////////////////////////////////////////////////////////////////

    public static bool operator== (MoveAction a, MoveAction b) => (
        ReferenceEquals(a.agent, b.agent) &&
        a.target == b.target
    );
    
    public static bool operator!= (MoveAction a, MoveAction b) => (
        !(a == b)
    );

    public override bool Equals(object obj)
    {
        if((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;

        MoveAction action = (MoveAction) obj;
        return action == this;
    }

    public override int GetHashCode() => (
        agent.position.GetHashCode() * 1000 + target.GetHashCode()
    );

    public override string ToString() => (
        $"{agent} to {target}"
    );
}

public class MoveResult : Result {
    public MoveResult() {
        was_successful = true;
    }
}

} // Definitions
} // Chess
