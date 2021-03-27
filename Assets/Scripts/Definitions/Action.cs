using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

// Abstract class representing the ability to execute some action that changes game state
public abstract class Action {
    // all actions are in reference to a piece itself, known as the agent of the action
    public Piece.GamePieceBase agent { get; protected set; }

    // All actions can execute define how they interact with the board directly
    public abstract Result Execute(BoardController controller);
}

public abstract class Result {
    public override string ToString() => "Action was Successful!";
}

public class InvalidResult : Result {
    public override string ToString() => "Action was Unsuccessful!";
}

public class GameOverResult : Result {
    public override string ToString() => "Game Over!";
}



} // Definitions
} // Chess
