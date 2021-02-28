using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

public class MoveAction : Action
{
    public BoardPosition target { get; }

    public MoveAction(GameObject agent, BoardPosition target) {
       this.agent = agent;
       this.target = target;
    }

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

} // Definitions
} // Chess