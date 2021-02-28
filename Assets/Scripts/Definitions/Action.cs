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
    public GameObject agent { get; protected set; }
}



} // Definitions
} // Chess