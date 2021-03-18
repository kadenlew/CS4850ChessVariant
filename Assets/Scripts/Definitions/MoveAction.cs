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
    public MoveAction(GameObject agent, BoardPosition target) {
       this.agent = agent;
       this.target = target;
    }
    public override Result Execute(BoardController controller) {
        // spend the energy
        agent.GetComponent<Piece.GamePieceBase>().expend_energy(1);

        // move
        agent.GetComponent<Piece.GamePieceBase>().move(
            target
        );

        return new MoveResult();
    }


///////////////////////////////////////////////////////////////////////////
//                              OPERATORS
//////////////////////////////////////////////////////////////////////////

    public static bool operator== (MoveAction a, MoveAction b) => (
        GameObject.ReferenceEquals(a.agent, b.agent) &&
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
        agent.GetComponent<Piece.GamePieceBase>().position.GetHashCode() * 1000 + target.GetHashCode()
    );

    public override string ToString() => (
        $"{agent.GetComponent<Piece.GamePieceBase>()} to {target}"
    );
}

public class MoveResult : Result {}

} // Definitions
} // Chess
